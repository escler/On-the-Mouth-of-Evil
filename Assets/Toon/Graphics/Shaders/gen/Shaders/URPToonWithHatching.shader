Shader "URPToonWithHatching"
{
    Properties
    {
        [Header(General)]
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)

        [Header(Hatching)]
        _Hatch0("Hatch Texture 0", 2D) = "white" {}
        _Hatch1("Hatch Texture 1", 2D) = "white" {}
        _HatchingIntensity("Hatching Intensity", Float) = 1.0
        _HatchingDistance("Hatching Distance", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "Forward"
            Tags {"LightMode" = "UniversalForward"}

            Cull Off
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _BaseMap; 
            float4 _BaseColor;

            sampler2D _Hatch0; 
            sampler2D _Hatch1;
            float _HatchingIntensity;
            float _HatchingDistance;

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS); 
                output.uv = input.uv;
                return output;
            }

            float3 ApplyHatching(float2 uv, float intensity, float dist, sampler2D hatch0, sampler2D hatch1)
            {
                float log2_dist = log2(dist);
                float2 floored_log_dist = floor((log2_dist + float2(0.0, 1.0)) * 0.5) * 2.0 - float2(0.0, 1.0);
                float2 uv_scale = min(float2(1.0, 1.0), pow(float2(2.0, 2.0), floored_log_dist)); 
                float uv_blend = abs(frac(log2_dist * 0.5) * 2.0 - 1.0);

                float2 scaledUVA = uv / uv_scale.x;
                float2 scaledUVB = uv / uv_scale.y;

                float3 hatch0A = tex2D(hatch0, scaledUVA).rgb;
                float3 hatch1A = tex2D(hatch1, scaledUVA).rgb;

                float3 hatch0B = tex2D(hatch0, scaledUVB).rgb;
                float3 hatch1B = tex2D(hatch1, scaledUVB).rgb;

                float3 hatch0Final = lerp(hatch0A, hatch0B, uv_blend);
                float3 hatch1Final = lerp(hatch1A, hatch1B, uv_blend);

                float3 overbright = max(0, intensity - 1.0);

                float3 weightsA = saturate((intensity * 6.0) + float3(-0.0, -1.0, -2.0));
                float3 weightsB = saturate((intensity * 6.0) + float3(-3.0, -4.0, -5.0));

                weightsA.xy -= weightsA.yz;
                weightsA.z -= weightsB.x;
                weightsB.xy -= weightsB.yz;

                hatch0Final = hatch0Final * weightsA;
                hatch1Final = hatch1Final * weightsB;

                return overbright + hatch0Final.r + hatch0Final.g + hatch0Final.b + hatch1Final.r + hatch1Final.g + hatch1Final.b;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                float4 baseColor = tex2D(_BaseMap, input.uv) * _BaseColor;

                float3 hatching = ApplyHatching(input.uv, _HatchingIntensity, _HatchingDistance, _Hatch0, _Hatch1);

                return float4(baseColor.rgb * hatching, baseColor.a);
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
