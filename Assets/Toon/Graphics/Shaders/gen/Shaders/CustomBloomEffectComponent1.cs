using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

[VolumeComponentMenuForRenderPipeline("CustomBloomEffect", typeof(UniversalRenderPipeline))]
public class CustomBloomEffectComponent1 : VolumeComponent, IPostProcessComponent
{
    //bloom settings copy from Unity default Bloom
    [Header("Bloom Settings")]
    public FloatParameter threshold = new FloatParameter(0.9f, true);
    public FloatParameter intensity = new FloatParameter(1, true);
    public ClampedFloatParameter scatter = new ClampedFloatParameter(0.7f, 0, 1, true);
    public IntParameter clamp = new IntParameter(65472, true);
    public ClampedIntParameter maxIterations = new ClampedIntParameter(6, 0, 10);
    public NoInterpColorParameter tint = new NoInterpColorParameter(Color.white);

    //Custom Bloom Dots
    [Header("Dots")]
    public IntParameter dotsDensity = new IntParameter(10, true);
    public ClampedFloatParameter dotsCutoff = new ClampedFloatParameter(0.4f, 0, 1, true);
    public Vector2Parameter scrollDirection = new Vector2Parameter(new Vector2());

    [Header("AOLines")]
    public ClampedFloatParameter linesWidth = new ClampedFloatParameter(0.001f, 0.001f, 0.01f, true);
    public ClampedFloatParameter linesIntensity = new ClampedFloatParameter(0.05f, 0, 0.05f, true);
    public ColorParameter linesColor = new ColorParameter(Color.black, true, true, true);
    public FloatParameter linesAngle = new FloatParameter(30f, true);

    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
