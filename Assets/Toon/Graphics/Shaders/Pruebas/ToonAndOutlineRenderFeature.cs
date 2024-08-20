using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ToonAndOutlineRenderFeature : ScriptableRendererFeature
{
    ToonAndOutlinePass toonAndOutlinePass;

    public override void Create()
    {
        toonAndOutlinePass = new ToonAndOutlinePass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(toonAndOutlinePass);
    }

    class ToonAndOutlinePass : ScriptableRenderPass
    {
        static readonly string k_RenderTag = "Render Toon and Outline Effects";

        // References to the shaders
        Shader toonShader;
        Shader outlineShader;

        Material toonMaterial;
        Material outlineMaterial;

        RTHandle tempTextureHandle;

        public ToonAndOutlinePass()
        {
            toonShader = Shader.Find("Custom/ToonPostProcess");
            if (toonShader == null)
                Debug.Log(toonShader);
            outlineShader = Shader.Find("Hidden/Outline Post Process");
            if (outlineShader == null)
                Debug.Log(outlineShader);
            toonMaterial = CoreUtils.CreateEngineMaterial(toonShader);
            outlineMaterial = CoreUtils.CreateEngineMaterial(outlineShader);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Use RTHandles to allocate the temporary texture
            RenderTextureDescriptor cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            tempTextureHandle = RTHandles.Alloc(cameraTextureDescriptor.width, cameraTextureDescriptor.height, colorFormat: cameraTextureDescriptor.graphicsFormat);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(k_RenderTag);

            var currentTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Apply Toon Shader
            Blit(cmd, currentTarget, tempTextureHandle, toonMaterial);
            Blit(cmd, tempTextureHandle, currentTarget);

            // Apply Outline Shader
            Blit(cmd, currentTarget, tempTextureHandle, outlineMaterial);
            Blit(cmd, tempTextureHandle, currentTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // Release the temporary texture
            tempTextureHandle?.Release();
        }
    }
}
