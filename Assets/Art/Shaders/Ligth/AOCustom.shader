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

             v2f vert (a2v v)
            {
                o.positionCS = ComputeScreenPos(o.positionHS);               
                return o.positionCS;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 screenUV = float2(input.positionCS.x, input.positionCS.y) / input.positionCS.w;
                return screenUV;                
            }
            ENDHLSL

           
          
            AmbientOcclusionFactor aoFactor = GetScreenSpaceAmbientOcclusion(screenUV);
            half ind = aoFactor.indirectAmbientOcclusion;
            half d = aoFactor.directAmbientOcclusion;
        }
     }
  FallBack "Diffuse"
}
         
   
