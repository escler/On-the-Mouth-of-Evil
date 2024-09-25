Shader "Custom/AOCustom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _DiffusePattern ("Hatching Pattern", 2D) = "white" {}  
        _ShadowSize ("Shadow Size", Range(0, 1)) = 0.5  
    }

    SubShader
    {  
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            ZWrite On
            ColorMask R

            HLSLPROGRAM
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #pragma shader_feature_local _ALPHATEST_ON

            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl" 

            struct DepthOnlyAttributes
            {
                float4 position : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct DepthOnlyVaryings
            {
                #if defined(_ALPHATEST_ON)
                    float2 uv : TEXCOORD0;
                #endif
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            DepthOnlyVaryings DepthOnlyVertex(DepthOnlyAttributes input)
            {
                DepthOnlyVaryings output = (DepthOnlyVaryings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                #if defined(_ALPHATEST_ON)
                    output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
                #endif
                float4x4 model = UNITY_MATRIX_M;
                float4x4 view = UNITY_MATRIX_V; 
                float4x4 proj = UNITY_MATRIX_P; 
                output.positionCS = mul(proj, mul(view, mul(model, input.position)));
                return output;
            }

            half DepthOnlyFragment(DepthOnlyVaryings input) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                #if defined(_ALPHATEST_ON)
                    Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_MainTex, sampler_MainTex)).a, _Color, _Cutoff);
                #endif

                #if defined(LOD_FADE_CROSSFADE)
                    LODFadeCrossFade(input.positionCS);
                #endif

                return input.positionCS.z;
            }

            ENDHLSL
        }     
        
        Pass
        {
            Name "DepthNormals"
            Tags{"LightMode" = "DepthNormals"}

            ZWrite On
            Cull[_Cull]

            HLSLPROGRAM

            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION 
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" 
           
            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _ShadowSize;
            CBUFFER_END

            TEXTURE2D(_DiffusePattern);
            SAMPLER(sampler_DiffusePattern);

            struct a2v
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
            };

            struct CustomSurface
            {
                float3 Normal;
                float Alpha;
                float2 diffusePatternUV;
            };

            void HatchingDiffuse(CustomSurface s, inout half atten, inout half4 color)
            {
                
                float3 lightDir = normalize(float3(0.5, 0.5, 0.5));  

                half NdotL = dot(s.Normal, lightDir);

                NdotL = smoothstep(-_ShadowSize, _ShadowSize, NdotL);
                atten = atten > 0.91 ? 1 : atten;
                NdotL = min(NdotL, atten);

                #ifdef SCREEN_SPACE_UVS
                    float2 uv = TRANSFORM_TEX(s.diffusePatternUV, _DiffusePattern) * 1000;
                    float val = max(tex2Dlod(_DiffusePattern, sampler_DiffusePattern, float4(uv, 0, 0)).r, 0.001);  
                #else
                    float val = tex2D(_DiffusePattern, sampler_DiffusePattern, s.diffusePatternUV).r;  
                #endif

                val = step(1 - val, NdotL) - (step(NdotL, 0.001) / 2 * step(1 - val, NdotL));

                atten = val;
                color.rgb += _Color.rgb * atten;
                color.a = s.Alpha;
            }

            v2f vert(a2v v)
            {
                v2f o;
                float4x4 model = UNITY_MATRIX_M;
                float4x4 view = UNITY_MATRIX_V;
                float4x4 proj = UNITY_MATRIX_P;
                o.positionCS = mul(proj, mul(view, mul(model, v.vertex))); 
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 screenUV = float2(i.positionCS.x, i.positionCS.y) / i.positionCS.w;
                
                AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(screenUV);
                half ind = aoFactor.indirectAmbientOcclusion;
                half d = aoFactor.directAmbientOcclusion;

                float hatchingPattern = abs(sin(screenUV.x * 50.0) * sin(screenUV.y * 50.0));

                half aoMask = ind * d;

                half atten = 1.0;
                half4 color = _Color;

                CustomSurface s;
                s.Normal = i.normalWS; 
                s.Alpha = 1.0;           
                s.diffusePatternUV = screenUV; 
                HatchingDiffuse(s, atten, color);  

                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
