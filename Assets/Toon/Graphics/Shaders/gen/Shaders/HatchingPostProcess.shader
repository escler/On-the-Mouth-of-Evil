Shader "Hidden/HatchingPostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Hatch0 ("Hatch Texture 0", 2D) = "white" {}
        _Hatch1 ("Hatch Texture 1", 2D) = "white" {}
        _Intensity ("Intensity", Range(0,2)) = 1.0
        _Dist ("Distance", Range(0.1, 50.0)) = 1.0
    }
    SubShader
    {
        Pass
        {
            Name "HatchingEffect"
            ZTest Always Cull Back ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            TEXTURE2D(_Hatch0);
            TEXTURE2D(_Hatch1);
            TEXTURE2D(_CameraNormalsTexture);
            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_Hatch0);
            SAMPLER(sampler_Hatch1);
            SAMPLER(sampler_CameraNormalsTexture);

            float _Intensity;
            float _Dist;

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

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = mul(UNITY_MATRIX_MVP, v.positionOS);
                o.uv = v.uv;
                return o;
            }

            float3 Hatching_float(float2 _uv, float _intensity, float _dist, Texture2D _Hatch0, Texture2D _Hatch1)
            {
                float log2_dist = log2(_dist);
                float2 floored_log_dist = floor((log2_dist + float2(0.0, 1.0)) * 0.5) * 2.0 - float2(0.0, 1.0);
                float2 uv_scale = min(1, pow(2.0, floored_log_dist));
                float uv_blend = abs(frac(log2_dist * 0.5) * 2.0 - 1.0);
                float2 scaledUVA = _uv / uv_scale.x;
                float2 scaledUVB = _uv / uv_scale.y;
                float3 hatch0A = SAMPLE_TEXTURE2D(_Hatch0, sampler_Hatch0, scaledUVA).rgb;
                float3 hatch1A = SAMPLE_TEXTURE2D(_Hatch1, sampler_Hatch1, scaledUVA).rgb;
                float3 hatch0B = SAMPLE_TEXTURE2D(_Hatch0, sampler_Hatch0, scaledUVB).rgb;
                float3 hatch1B = SAMPLE_TEXTURE2D(_Hatch1, sampler_Hatch1, scaledUVB).rgb;
                float3 hatch0 = lerp(hatch0A, hatch0B, uv_blend);
                float3 hatch1 = lerp(hatch1A, hatch1B, uv_blend);
                float3 overbright = max(0, _intensity - 1.0);
                float3 weightsA = saturate((_intensity * 6.0) + float3(-0, -1, -2));
                float3 weightsB = saturate((_intensity * 6.0) + float3(-3, -4, -5));
                weightsA.xy -= weightsA.yz;
                weightsA.z -= weightsB.x;
                weightsB.xy -= weightsB.yz;
                hatch0 = hatch0 * weightsA;
                hatch1 = hatch1 * weightsB;
                return overbright + hatch0.r + hatch0.g + hatch0.b + hatch1.r + hatch1.g + hatch1.b;
            }
            float4 _WorldSpaceLightPos0;

            half4 frag (Varyings i) : SV_Target
            {
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, i.uv).rgb * 2.0 - 1.0);
                float lightIntensity = saturate(dot(normal, lightDir));
                
                float3 hatching = Hatching_float(i.uv, _Intensity * lightIntensity, _Dist, _Hatch0, _Hatch1);
                return half4(hatching, 1.0);
            }
          
            ENDHLSL
        }
    }
    FallBack Off
}
