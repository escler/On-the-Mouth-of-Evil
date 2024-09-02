//using UnityEngine;
//using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;
//using System;

//public class HatchingPostProcessFeature : ScriptableRendererFeature, IDisposable
//{
//    class HatchingRenderPass : ScriptableRenderPass
//    {
//        public Material hatchingMaterial = null;
//        private RTHandle tempTexture;
//        private RTHandle source;

//        public HatchingRenderPass(Material material)
//        {
//            this.hatchingMaterial = material;
//        }

//        private void AllocateTempTexture(RenderTextureDescriptor descriptor)
//        {
//            if (tempTexture != null)
//            {
//                RTHandles.Release(tempTexture);
//            }
//            tempTexture = RTHandles.Alloc(descriptor, filterMode: FilterMode.Bilinear, name: "_TempTexture");
//        }

//        public override void FrameCleanup(CommandBuffer cmd)
//        {
//            if (tempTexture != null)
//            {
//                RTHandles.Release(tempTexture);
//                tempTexture = null;
//            }
//        }

//        public void Setup(RTHandle source)
//        {
//            this.source = source;
//        }

//        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
//        {
//            RenderTextureDescriptor descriptor = cameraTextureDescriptor;
//            descriptor.depthBufferBits = 0; // No depth buffer needed

//            AllocateTempTexture(descriptor);

//            cmd.SetGlobalTexture("_MainTex", source);
//        }

//        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
//        {
//            if (hatchingMaterial == null || source == null || tempTexture == null)
//            {
//                Debug.LogError("HatchingRenderPass: Missing material or render textures.");
//                return;
//            }

//            CommandBuffer cmd = CommandBufferPool.Get("Hatching Effect");

//            Blit(cmd, source, tempTexture, hatchingMaterial, 0);
//            Blit(cmd, tempTexture, source);

//            context.ExecuteCommandBuffer(cmd);
//            CommandBufferPool.Release(cmd);
//        }

//        public void Dispose()
//        {
//            if (tempTexture != null)
//            {
//                RTHandles.Release(tempTexture);
//                tempTexture = null;
//            }
//        }

//        ~HatchingRenderPass()
//        {
//            Dispose();
//        }
//    }

//    HatchingRenderPass hatchingRenderPass;
//    public Material hatchingMaterial;

//    public override void Create()
//    {
//        hatchingRenderPass = new HatchingRenderPass(hatchingMaterial);
//    }

//    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
//    {
//        renderer.EnqueuePass(hatchingRenderPass);
//    }

//    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
//    {
//        hatchingRenderPass.Setup(renderer.cameraColorTargetHandle);
//    }

//    protected override void Dispose(bool disposing)
//    {
//        base.Dispose(disposing);
//        if (disposing)
//        {
//            hatchingRenderPass.Dispose();
//        }
//    }

//    void OnDisable()
//    {
//        Dispose();
//    }

//    ~HatchingPostProcessFeature()
//    {
//        Dispose();
//    }
//}
