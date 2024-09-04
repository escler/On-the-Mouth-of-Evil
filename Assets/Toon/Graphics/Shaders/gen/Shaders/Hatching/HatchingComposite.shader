Shader "Hidden/HatchingComposite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Hatch0("Hatch 0 (light)", 2D) = "white" {}
        _Hatch1("Hatch 1", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_Hatch0);
            SAMPLER(sampler_Hatch0);
            TEXTURE2D(_Hatch1);
            SAMPLER(sampler_Hatch1);
            TEXTURE2D(_UVBuffer);
            SAMPLER(sampler_UVBuffer);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float2 uvFlipY : TEXCOORD1;
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                // Use mul(UNITY_MATRIX_MVP, input.positionOS) instead of TransformObjectToHClip
                output.positionCS = mul(UNITY_MATRIX_MVP, input.positionOS);
                output.uv = input.uv;
                output.uvFlipY = output.uv;

                #if defined(UNITY_UV_STARTS_AT_TOP) && !defined(SHADER_API_MOBILE)
                    output.uvFlipY.y = 1.0 - output.uv.y;
                #endif

                return output;
            }

            float3 Hatching(float2 uv, float intensity)
            {
                float3 hatch0 = SAMPLE_TEXTURE2D(_Hatch0, sampler_Hatch0, uv).rgb;
                float3 hatch1 = SAMPLE_TEXTURE2D(_Hatch1, sampler_Hatch1, uv).rgb;

                float3 overbright = max(0, intensity - 1.0);

                float3 weightsA = saturate((intensity * 6.0) + float3(0.0, -1.0, -2.0));
                float3 weightsB = saturate((intensity * 6.0) + float3(-3.0, -4.0, -5.0));

                weightsA.xy -= weightsA.yz;
                weightsA.z   -= weightsB.x;
                weightsB.xy -= weightsB.yz;

                hatch0 = hatch0 * weightsA;
                hatch1 = hatch1 * weightsB;

                float3 hatching = overbright + hatch0.r + hatch0.g + hatch0.b + hatch1.r + hatch1.g + hatch1.b;

                return hatching;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                float4 uv = SAMPLE_TEXTURE2D(_UVBuffer, sampler_UVBuffer, input.uvFlipY);

                float intensity = dot(col.rgb, float3(0.2326, 0.7152, 0.0722));

                float3 hatch = Hatching(uv.xy * 8.0, intensity);

                col.rgb = hatch;

                return col;
            }

            ENDHLSL
        }
    }
}
