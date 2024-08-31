using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HatchingPostProcessFeature : ScriptableRendererFeature
{
    class HatchingRenderPass : ScriptableRenderPass
    {
        public Material hatchingMaterial = null;
        private RenderTargetIdentifier source { get; set; }
        private RenderTargetHandle tempTexture;

        public HatchingRenderPass(Material material)
        {
            this.hatchingMaterial = material;
            tempTexture.Init("_TempTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Hatching Effect");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc, FilterMode.Bilinear);
            Blit(cmd, source, tempTexture.Identifier(), hatchingMaterial, 0);
            Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    HatchingRenderPass hatchingRenderPass;
    public Material hatchingMaterial;

    public override void Create()
    {
        hatchingRenderPass = new HatchingRenderPass(hatchingMaterial);
        hatchingRenderPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        hatchingRenderPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(hatchingRenderPass);
    }
}
