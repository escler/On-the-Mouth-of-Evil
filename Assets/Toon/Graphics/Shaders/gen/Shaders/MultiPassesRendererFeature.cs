using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MultiPassesRendererFeature : ScriptableRenderPass
{
    private List<ShaderTagId> m_Tags;

    public MultiPassesRendererFeature(List<string> tags)
    {
        m_Tags = new List<ShaderTagId>();
        foreach (string tag in tags)
        {
            m_Tags.Add(new ShaderTagId(tag));
        }
        this.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }


    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        FilteringSettings filterSettings = FilteringSettings.defaultValue;

        foreach (ShaderTagId tag in m_Tags)
        {
            DrawingSettings drawingSettings = CreateDrawingSettings(tag, ref renderingData, SortingCriteria.CommonTransparent);
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filterSettings);
        }

        context.Submit();
    }

  
}
