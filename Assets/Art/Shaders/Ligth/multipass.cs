using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CustomRenderFeaturee : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        //private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;
        private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();
        public CustomRenderPass()
        {
            //this.material = material;
            tempTexture.Init("_TemporaryColorTexture");
            m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
            m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //if (material == null)
            //{
            //    Debug.LogWarningFormat("Missing material. {0} render pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
            //    return;
            //}

            CommandBuffer cmd = CommandBufferPool.Get("CustomRenderPass");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            RenderTargetIdentifier destination = tempTexture.Identifier();

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);
           // Blit(cmd, source, destination, material);
            Blit(cmd, destination, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    //[System.Serializable]
    //public class CustomRenderPassSettings
    //{
    //    public Material material = null;
    //}

    //public CustomRenderPassSettings settings = new CustomRenderPassSettings();
    CustomRenderPass customRenderPass;

    public override void Create()
    {
        customRenderPass = new CustomRenderPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customRenderPass); // letting the renderer know which passes will be used before allocation
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        customRenderPass.Setup(renderer.cameraColorTarget);  // use of target after allocation
    }


}

