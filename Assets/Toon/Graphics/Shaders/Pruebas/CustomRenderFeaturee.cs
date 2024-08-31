using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderFeaturee : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RTHandle tempTexture;
        private int tempTextureID; // Add this line

        public CustomRenderPass(Material material)
        {
            this.material = material;
            tempTextureID = Shader.PropertyToID("tempTexture"); // Move this line before allocating tempTexture
            tempTexture = RTHandles.Alloc(Vector2.zero);
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
            {
                Debug.LogWarningFormat("Missing material. {0} render pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("CustomRenderPass");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            RenderTargetIdentifier destination = tempTexture;

            cmd.GetTemporaryRT(tempTextureID, opaqueDesc); // Replace tempTexture.name with tempTextureID

            cmd.Blit(source, destination, material);
            cmd.Blit(destination, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTextureID); // Replace tempTexture.name with tempTextureID
        }
    }

    [System.Serializable]
    public class CustomRenderPassSettings
    {
        public Material material = null;
    }

    public CustomRenderPassSettings settings = new CustomRenderPassSettings();
    CustomRenderPass customRenderPass;

    public override void Create()
    {
        customRenderPass = new CustomRenderPass(settings.material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customRenderPass); // letting the renderer know which passes will be used before allocation
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        customRenderPass.Setup(renderer.cameraColorTargetHandle);  // use of target after allocation
    }
}
