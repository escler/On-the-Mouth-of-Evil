using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class CustomPostProcessRenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader m_LigthShader;
    [SerializeField]
    private Shader m_compositeShader;

    private Material m_bloomMaterial;
    private Material m_compositeMaterial;

    private CustomPostProcessPass m_customPass;

    public override void Create()
    {
        m_bloomMaterial = CoreUtils.CreateEngineMaterial(m_LigthShader);
        m_compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);
        m_customPass = new CustomPostProcessPass(m_bloomMaterial, m_compositeMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        renderer.EnqueuePass(m_customPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Color);
            m_customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_bloomMaterial);
        CoreUtils.Destroy(m_compositeMaterial);
    }
}
