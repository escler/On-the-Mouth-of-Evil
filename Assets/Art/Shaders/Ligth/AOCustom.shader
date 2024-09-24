Shader "Custom/AOCustom"
{
     Properties
     {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		
     }

     SubShader
	 {  // UnlitFrameDebugger
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            // -------------------------------------
            // Render State Commands
            ZWrite On
            ColorMask R

            HLSLPROGRAM
            #pragma target 2.0

            // -------------------------------------
            // Shader Stages
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _ALPHATEST_ON

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"

            // -------------------------------------
            // Includes
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // Incluir Core.hlsl para UnityObjectToClipPos
           
             float4 _Color;

            struct a2v
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
            };

         

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
    
                // Obtener el factor de oclusión ambiental
                AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(screenUV);
                half ind = aoFactor.indirectAmbientOcclusion;
                half d = aoFactor.directAmbientOcclusion;
    
                // Generar un patrón de hatching basado en las coordenadas de pantalla
                float hatchingPattern = abs(sin(screenUV.x * 50.0) * sin(screenUV.y * 50.0));

                // Aplicar oclusión ambiental como máscara
                half aoMask = ind * d;

                // Multiplicar el patrón de hatching por la máscara de oclusión ambiental
                half4 color = _Color;
                half4 hatchingColor = half4(aoMask * hatchingPattern, aoMask * hatchingPattern, aoMask * hatchingPattern, color.a);

                return hatchingColor; // Devuelve el color sombreado con el patrón de hatching
            }
            ENDHLSL
        }
     }
  FallBack "Diffuse"
}
         
   
