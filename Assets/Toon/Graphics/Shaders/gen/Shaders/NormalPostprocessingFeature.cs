using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NormalPostprocessingFeature : ScriptableRendererFeature
{
    class NormalPostprocessingPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetHandle tempTexture;
        private RenderTargetIdentifier currentTarget;

        private Vector3 lightDirection;
        private Color lightColor;

        public NormalPostprocessingPass(Material material)
        {
            this.material = material;
            tempTexture.Init("_TemporaryColorTexture");
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing; 
        }

        public void Setup(RenderTargetIdentifier source, Vector3 direction, Color color)
        {
            currentTarget = source;
            lightDirection = direction;
            lightColor = color;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
            {
                Debug.LogWarning("Missing material. Render pass will not execute.");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("NormalPostprocessingPass");

            Camera camera = renderingData.cameraData.camera;

            camera.depthTextureMode |= DepthTextureMode.DepthNormals;

            Matrix4x4 viewToWorld = camera.cameraToWorldMatrix;
            material.SetMatrix("_viewToWorld", viewToWorld);

            material.SetVector("_LightDirection", lightDirection);
            material.SetColor("_LightColor", lightColor);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);

            Blit(cmd, currentTarget, tempTexture.Identifier(), material);
            Blit(cmd, tempTexture.Identifier(), currentTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    [System.Serializable]
    public class NormalPostprocessingSettings
    {
        public Material material = null;
        public Vector3 lightDirection = new Vector3(0.5f, 0.5f, -1.0f); 
        public Color lightColor = Color.white;
    }

    public NormalPostprocessingSettings settings = new NormalPostprocessingSettings();
    NormalPostprocessingPass normalPostprocessingPass;

    public override void Create()
    {
        normalPostprocessingPass = new NormalPostprocessingPass(settings.material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        normalPostprocessingPass.Setup(renderer.cameraColorTarget, settings.lightDirection, settings.lightColor);
        renderer.EnqueuePass(normalPostprocessingPass);
    }
}
