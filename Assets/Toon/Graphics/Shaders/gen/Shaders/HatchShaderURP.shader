Shader "Custom/HatchShaderURP"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _HatchSize ("Hatch Size", Float) = 1.0
        _HatchAngle ("Hatch Angle", Float) = 45.0
        _ExponentialFalloff ("Exponential Falloff", Float) = 1.0
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Emission ("Emission", Color) = (0,0,0,0)
        _EmissionIntensity ("Emission Intensity", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "Lit"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "Forward"
            Tags {"LightMode" = "UniversalForward"}

            Cull Back
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex ForwardPassVertex
            #pragma fragment ForwardPassFragment

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile _ _FORWARD_PLUS
            #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            #pragma shader_feature_local_fragment _DOUBLE_SIDED
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _NORMAL_MAP
            #pragma shader_feature_local_fragment _SPECULAR
            #pragma shader_feature_local_fragment _RIM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Vertex Shader
            void ForwardPassVertex(
                inout VertexPositionOutput position : POSITION,
                inout float3 normal : NORMAL,
                inout float2 uv : TEXCOORD0)
            {
                // Transform vertex position
                position = TransformObjectToHClip(position);
                normal = normalize(mul(normal, (float3x3)UNITY_MATRIX_IT_MV));
            }

            // Fragment Shader
            half4 ForwardPassFragment(
                float2 uv : TEXCOORD0,
                float3 normal : NORMAL) : SV_Target
            {
                // Sample base color and normal map
                half4 baseColor = _Color;
                half3 normalMap = tex2D(_NormalMap, uv).rgb;
                half3 worldNormal = normalize(normal + normalMap);

                // Compute light direction and intensity
                half3 lightDir = normalize(UnityWorldSpaceLightDir(worldNormal));
                half lightIntensity = max(0.0, dot(worldNormal, lightDir));
                
                // Compute diffuse lighting
                half3 diffuseColor = baseColor.rgb * lightIntensity;

                // Compute UV position for hatching effect
                float hatchAngle = _HatchAngle * 3.14159 / 180.0;
                float2 hatchUV = uv;
                hatchUV = float2(hatchUV.x * cos(hatchAngle) - hatchUV.y * sin(hatchAngle),
                                 hatchUV.x * sin(hatchAngle) + hatchUV.y * cos(hatchAngle));
                
                // Create hatch pattern
                float hatchPattern = abs(sin(hatchUV.x * _HatchSize)) * abs(sin(hatchUV.y * _HatchSize));
                
                // Apply exponential falloff to pattern
                float falloff = exp(-_ExponentialFalloff * (1.0 - lightIntensity));
                hatchPattern *= falloff;

                // Combine with diffuse color
                half4 color = half4(diffuseColor * hatchPattern, baseColor.a);
                
                return color;
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShadowCaster.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DepthOnly.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "DepthNormals"
            Tags{"LightMode" = "DepthNormals"}

            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DepthNormals.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "Outline"
            Tags {"LightMode" = "SRPDefaultUnlit"}

            Cull Front

            HLSLPROGRAM
            #pragma vertex OutlinePassVertex
            #pragma fragment OutlinePassFragment

            #include "ToonInput.hlsl"
            #include "ToonOutlinePass.hlsl"

            ENDHLSL
        }        
    }

    FallBack "Diffuse"
}
