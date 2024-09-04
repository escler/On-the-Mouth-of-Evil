using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
[System.Serializable]
public class MultiPassesFeature : ScriptableRendererFeature
{
    public List<string> lightModePasses = new List<string> { "Toon" };
    private MultiPassesRendererFeature mainPass;
    

    public override void Create()
    {
        mainPass = new MultiPassesRendererFeature(lightModePasses);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        
        renderer.EnqueuePass(mainPass);
        
    }
}
