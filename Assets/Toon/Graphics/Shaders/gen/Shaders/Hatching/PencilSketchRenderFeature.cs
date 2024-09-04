using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PencilSketchRenderFeature : ScriptableRendererFeature
{
    public PencilSketchSettings settings = new PencilSketchSettings();
    private PencilSketchRenderPass renderPass;

    [System.Serializable]
    public class PencilSketchSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public LayerMask effectLayer = ~0;
        public Shader uvReplacementShader;
        public Material compositeMaterial;
        [Range(0.1f, 1.0f)] public float bufferScale = 1.0f;
    }

    public override void Create()
    {
        renderPass = new PencilSketchRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.compositeMaterial == null || settings.uvReplacementShader == null)
        {
            Debug.LogWarningFormat("Missing material or shader in {0}", GetType().Name);
            return;
        }

        renderPass.Setup(renderer);
        renderer.EnqueuePass(renderPass);
    }

    class PencilSketchRenderPass : ScriptableRenderPass
    {
        private PencilSketchSettings settings;
        private RTHandle _cameraColorTarget;
        private RTHandle uvBuffer;
        private bool isReady;

        public PencilSketchRenderPass(PencilSketchSettings settings)
        {
            this.settings = settings;
            this.renderPassEvent = settings.renderPassEvent;
        }

        public void Setup(ScriptableRenderer renderer)
        {
            _cameraColorTarget = renderer.cameraColorTargetHandle;
            isReady = (_cameraColorTarget != null && settings.compositeMaterial != null && settings.uvReplacementShader != null);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            if (!isReady) return;

            int scaledWidth = Mathf.RoundToInt(cameraTextureDescriptor.width * settings.bufferScale);
            int scaledHeight = Mathf.RoundToInt(cameraTextureDescriptor.height * settings.bufferScale);

            var uvBufferDescriptor = cameraTextureDescriptor;
            uvBufferDescriptor.width = scaledWidth;
            uvBufferDescriptor.height = scaledHeight;
            uvBufferDescriptor.depthBufferBits = 24;
            uvBufferDescriptor.colorFormat = RenderTextureFormat.ARGBFloat;

            uvBuffer = RTHandles.Alloc(uvBufferDescriptor.width, uvBufferDescriptor.height, depthBufferBits: (DepthBits)24, colorFormat: (UnityEngine.Experimental.Rendering.GraphicsFormat)RenderTextureFormat.ARGBFloat);

            ConfigureTarget(uvBuffer);
            ConfigureClear(ClearFlag.All, Color.clear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!isReady || uvBuffer == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("PencilSketchPass");
            Camera camera = renderingData.cameraData.camera;

            using (new ProfilingScope(cmd, new ProfilingSampler("PencilSketchPass")))
            {
                cmd.SetViewProjectionMatrices(camera.worldToCameraMatrix, camera.projectionMatrix);
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var drawingSettings = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, SortingCriteria.CommonOpaque);
                drawingSettings.overrideMaterialPassIndex = 0;
                drawingSettings.overrideMaterial = settings.compositeMaterial;

                FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque, settings.effectLayer);
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);

                settings.compositeMaterial.SetTexture("_UVBuffer", uvBuffer);
                Blit(cmd, _cameraColorTarget, _cameraColorTarget, settings.compositeMaterial);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (uvBuffer != null)
            {
                RTHandles.Release(uvBuffer);
                uvBuffer = null;
            }
        }
    }
}
