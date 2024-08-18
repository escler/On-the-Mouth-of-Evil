using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NormalLine : ScriptableRendererFeature
{
    [System.Serializable]
    public class Setting
    {
        public LayerMask layer;
        public Material normalTexMat;
        public Material normalLineMat;
        public RenderPassEvent passEvent = RenderPassEvent.AfterRenderingPrePasses;
        [Range(0, 1)]
        public float Edge = 0;
    }

    public Setting setting = new Setting();

    public class NormalTPass: ScriptableRenderPass
    {
        private Setting setting;
        ShaderTagId shaderTag = new ShaderTagId("DepthOnly");
        FilteringSettings filter;
        NormalLine feature;
        public NormalTPass(Setting setting, NormalLine feature)
        {
            this.setting = setting;
            this.feature = feature;

            RenderQueueRange queue = new RenderQueueRange();
            queue.lowerBound = 1000;
            queue.upperBound = 3500;
            filter = new FilteringSettings(queue, setting.layer);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            int temp = Shader.PropertyToID("_NormalTex");
            RenderTextureDescriptor desc = cameraTextureDescriptor;
            cmd.GetTemporaryRT(temp, desc);
            ConfigureTarget(temp);
            ConfigureClear(ClearFlag.All, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("NormalTex");
            var draw = CreateDrawingSettings(shaderTag, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            draw.overrideMaterial = setting.normalTexMat;
            draw.overrideMaterialPassIndex = 0;
            context.DrawRenderers(renderingData.cullResults, ref draw, ref filter);
            CommandBufferPool.Release(cmd);
        }
    }

    public class NormalLinePass : ScriptableRenderPass
    {
        private Setting setting;
        NormalLine feature;

        public NormalLinePass(Setting setting, NormalLine feature)
        {
            this.setting = setting;
            this.feature = feature;
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID("_NormalLineTex"));
            cmd.ReleaseTemporaryRT(Shader.PropertyToID("_NormalTex"));

        }



        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Limite");
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            setting.normalLineMat.SetFloat("Edge", setting.Edge);
            int normalLineID = Shader.PropertyToID("_NormalLineTex");
            cmd.GetTemporaryRT(normalLineID, desc);
            cmd.Blit(normalLineID, normalLineID, setting.normalLineMat,0);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private NormalTPass normalTPass;
    private NormalLinePass normalLinePass;

    public override void Create()
    {
        normalTPass = new NormalTPass(setting, this);
        normalTPass.renderPassEvent = setting.passEvent;
        normalLinePass = new NormalLinePass(setting, this);
        normalLinePass.renderPassEvent = setting.passEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(normalTPass);
        renderer.EnqueuePass(normalLinePass);
    }
}


