Shader "Custom/StencilMask"
{
    Properties
    {
       [intRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"= "Geometry-1" "RenderPipeline"="UniversalPipeline" }

        Pass
		{
            Blend Zero One
            ZWrite Off

			Stencil
			{
				Ref [_StencilID]
				Comp always
				Pass replace
			}
		}
    }
}
