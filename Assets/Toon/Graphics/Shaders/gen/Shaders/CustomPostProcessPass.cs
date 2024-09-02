using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;
using static Unity.VisualScripting.Member;

public class CustomPostProcessPass : ScriptableRenderPass
{
    private Material m_bloomMaterial;
    private Material m_compositeMaterial;

    private RTHandle m_CameraColorTarget;
    private RTHandle m_CameraDepthTarget;

    const int k_MaxPyramidSize = 16;
    private int[] _BloomMipUp;
    private int[] _BloomMipDown;
    private RTHandle[] m_BloomMipUp;
    private RTHandle[] m_BloomMipDown;
    private GraphicsFormat hdrFormat;
    private CustomBloomEffectComponent1 m_BloomEffect;
    private RenderTextureDescriptor m_Descriptor;
    private static readonly int ScreenSpaceOcclusionTexture = Shader.PropertyToID("_ScreenSpaceOcclusionTexture");
    private bool isReady = false;

    public void SetTarget(RTHandle cameraColorTarget, RTHandle cameraDepthTarget)
    {
        this.m_CameraColorTarget = cameraColorTarget;
        this.m_CameraDepthTarget = cameraDepthTarget;
        isReady = !(m_CameraColorTarget == null || m_CameraDepthTarget == null);
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        m_Descriptor = renderingData.cameraData.cameraTargetDescriptor;
    }

    public CustomPostProcessPass(Material bloomMaterial, Material compositeMaterial)
    {
        this.m_bloomMaterial = bloomMaterial;
        this.m_compositeMaterial = compositeMaterial;

        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

        _BloomMipUp = new int[k_MaxPyramidSize];
        _BloomMipDown = new int[k_MaxPyramidSize];
        m_BloomMipUp = new RTHandle[k_MaxPyramidSize];
        m_BloomMipDown = new RTHandle[k_MaxPyramidSize];

        for (int i = 0; i < k_MaxPyramidSize; i++)
        {
            _BloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i);
            _BloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i);

            m_BloomMipUp[i] = RTHandles.Alloc(_BloomMipUp[i], name: "_BloomMipUp" + i);
            m_BloomMipDown[i] = RTHandles.Alloc(_BloomMipDown[i], name: "_BloomMipDown" + i);
        }

        const FormatUsage usage = FormatUsage.Linear | FormatUsage.Render;
        if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, usage)) 
        {
            hdrFormat = GraphicsFormat.B10G11R11_UFloatPack32;
        }
        else
        {
            hdrFormat = QualitySettings.activeColorSpace == ColorSpace.Linear ? GraphicsFormat.R8G8B8_SRGB : GraphicsFormat.R8G8B8_UNorm;
        }
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!isReady) return;
        VolumeStack stack = VolumeManager.instance.stack;
        m_BloomEffect = stack.GetComponent<CustomBloomEffectComponent1>();

        CommandBuffer cmd = CommandBufferPool.Get();

       

        using (new ProfilingScope(cmd, new ProfilingSampler("Custom Post Process Effect")))
        {
            Texture ssaoTex = Shader.GetGlobalTexture(ScreenSpaceOcclusionTexture);
            m_compositeMaterial.SetTexture("_SSAOTexture", ssaoTex);

            SetupBloom(cmd, m_CameraColorTarget);

            m_compositeMaterial.SetFloat("_Cutoff", m_BloomEffect.dotsCutoff.value);
            m_compositeMaterial.SetFloat("_Density", m_BloomEffect.dotsDensity.value);
            m_compositeMaterial.SetVector("_Direction", m_BloomEffect.scrollDirection.value);
            m_compositeMaterial.SetFloat("_LineWidth", m_BloomEffect.linesWidth.value);
            m_compositeMaterial.SetFloat("_LineIntensity", m_BloomEffect.linesIntensity.value);
            m_compositeMaterial.SetColor("_LineColor", m_BloomEffect.linesColor.value);
            m_compositeMaterial.SetFloat("_LineAngle", m_BloomEffect.linesAngle.value);

            Blitter.BlitCameraTexture(cmd, m_CameraColorTarget, m_CameraColorTarget, m_compositeMaterial, 0);
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    private void SetupBloom(CommandBuffer cmd, RTHandle source)
    {
       

      

       


        int downres = 1;
        int tw = m_Descriptor.width >> downres;
        int th = m_Descriptor.height >> downres;

        int maxSize = Mathf.Max(tw, th);
        int iterations = Mathf.FloorToInt(Mathf.Log(maxSize, 2f) - 1);
        int mipCount = Mathf.Clamp(iterations, 1, m_BloomEffect.maxIterations.value);

        float clamp = m_BloomEffect.clamp.value;
        float threshold = Mathf.GammaToLinearSpace(m_BloomEffect.threshold.value);
        float thresholdKnee = threshold * 0.5f;

        float scatter = Mathf.Lerp(0.05f, 0.95f, m_BloomEffect.scatter.value);
        var bloomMaterial = m_bloomMaterial;
        bloomMaterial.SetVector("_Params", new Vector4(scatter, clamp, threshold, thresholdKnee));

        var desc = GetCompatibleDescriptor(tw, th, hdrFormat);
        for (int i = 0; i < mipCount; i++)
        {
            RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipUp[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: m_BloomMipUp[i].name);
            RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipDown[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp,
                name: m_BloomMipDown[i].name);
            desc.width = Mathf.Max(1, desc.width >> 1);
            desc.height = Mathf.Max(i, desc.height >> i);
        }

        Blitter.BlitCameraTexture(cmd, source, m_BloomMipDown[0], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 0);

        var lastdown = m_BloomMipDown[0];
        for (int i = 0; i < mipCount - 1; i++)
        {
            Blitter.BlitCameraTexture(cmd, lastdown, m_BloomMipUp[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 1);
            Blitter.BlitCameraTexture(cmd, m_BloomMipUp[i], m_BloomMipDown[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 2);

            lastdown = m_BloomMipDown[i];
        }

        for (int i = mipCount - 2; i >= 0; i--)
        {

            var lowMip = (i == mipCount - 2) ? m_BloomMipDown[i + 1] : m_BloomMipUp[i + 1];
            var highMip = m_BloomMipDown[i];
            var dst = m_BloomMipUp[i];

            cmd.SetGlobalTexture("_SourceTexLowMip", lowMip);

            Blitter.BlitCameraTexture(cmd, highMip, dst, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 3);
        }

        m_compositeMaterial.SetTexture("_Bloom_Texture", m_BloomMipUp[0]);
        m_compositeMaterial.SetFloat("_BloomIntensity", m_BloomEffect.intensity.value);
        cmd.SetGlobalFloat("_BloomIntensity", m_BloomEffect.intensity.value);
    }

    private RenderTextureDescriptor GetCompatibleDescriptor()
    {
        return GetCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, m_Descriptor.graphicsFormat);
    }

    private RenderTextureDescriptor GetCompatibleDescriptor(int tw, int th, GraphicsFormat graphicsFormat, DepthBits depthBufferBits = DepthBits.None)
    {
        return GetCompatibleDescriptor(m_Descriptor, tw, th, graphicsFormat, depthBufferBits);
    }

    internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int tw, int th, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
    {
        desc.depthBufferBits = (int)depthBufferBits;
        desc.width = tw;
        desc.height = th;
        desc.graphicsFormat = format;
        desc.msaaSamples = 1;
        return desc;
    }

}

