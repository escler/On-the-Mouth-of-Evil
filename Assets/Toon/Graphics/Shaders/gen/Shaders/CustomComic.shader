Shader "Custom/CustomComic"
{
    Properties
    {
        [NoScaleOffset]Texture2D_DC6DF663("Base Texture", 2D) = "white" {}
        Color_DAC4E11B("Color", Color) = (1, 1, 1, 0)
        Vector2_CFCD19EF("Texture Tiling", Vector) = (0, 0, 0, 0)
        Vector1_ADAD9479("Smoothness", Range(0, 1)) = 0.5
        Vector1_EE54776A("Cross Hatch Frequency", Float) = 200
        Vector1_46F4FBA0("Dots Density", Float) = 250
        Vector1_29C6AB4C("Rim Light Falloff", Range(0, 1)) = 0.9
        Vector1_E023A9A8("Slope Mask Max", Range(0, 1)) = 0.6
        _Rotation("Rotation", Float) = 45
        _frecuency("frecuency", Float) = 5
        _offset("offset", Float) = 5
        _Thickness("Thickness", Float) = 0.5
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "CustomRenderType" ="Toon" 
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Unlit"
            "Queue"="AlphaTest"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
            
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        AlphaToMask On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        #define VARYINGS_NEED_TEXCOORD2
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 WorldSpaceViewDirection;
             float3 ViewSpacePosition;
             float3 WorldSpacePosition;
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float4 uv1;
             float4 uv2;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 texCoord2 : INTERP2;
             float3 positionWS : INTERP3;
             float3 normalWS : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.texCoord2.xyzw = input.texCoord2;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.texCoord2 = input.texCoord2.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 Texture2D_DC6DF663_TexelSize;
        float2 Vector2_CFCD19EF;
        float4 Color_DAC4E11B;
        float Vector1_ADAD9479;
        float Vector1_EE54776A;
        float Vector1_46F4FBA0;
        float Vector1_29C6AB4C;
        float Vector1_E023A9A8;
        float _Rotation;
        float _frecuency;
        float _offset;
        float _Thickness;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_DC6DF663);
        SAMPLER(samplerTexture2D_DC6DF663);
        
        // Graph Includes
        #include "Assets/Toon/Graphics/Shaders/Pruebas/ShaderGraphs/CustomLighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        struct Bindings_MainLight_970d962ef25c3084987fd10643902eed_float
        {
        };
        
        void SG_MainLight_970d962ef25c3084987fd10643902eed_float(Bindings_MainLight_970d962ef25c3084987fd10643902eed_float IN, out float3 Direction_1, out float3 Colour_2, out float CullingMask_3)
        {
        float3 _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_Direction_1_Vector3;
        float3 _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_Colour_2_Vector3;
        float _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_DistanceAtten_3_Float;
        MainLight_float(_MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_Direction_1_Vector3, _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_Colour_2_Vector3, _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_DistanceAtten_3_Float);
        Direction_1 = _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_Direction_1_Vector3;
        Colour_2 = _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_Colour_2_Vector3;
        CullingMask_3 = _MainLightCustomFunction_3be2118d2270ff818ec5f0f1353e249f_DistanceAtten_3_Float;
        }
        
        void Unity_DotProduct_float3(float3 A, float3 B, out float Out)
        {
            Out = dot(A, B);
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
        Out = A * B;
        }
        
        struct Bindings_MainLightDiffuse_54e5956b307254749804a49de2643ea4_float
        {
        float3 WorldSpaceNormal;
        };
        
        void SG_MainLightDiffuse_54e5956b307254749804a49de2643ea4_float(float3 _Normal, bool _Normal_62e59cd14b7d4dc09c6989fd7428f0b5_IsConnected, Bindings_MainLightDiffuse_54e5956b307254749804a49de2643ea4_float IN, out float3 Diffuse_3)
        {
        float3 _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3 = _Normal;
        bool _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3_IsConnected = _Normal_62e59cd14b7d4dc09c6989fd7428f0b5_IsConnected;
        float3 _BranchOnInputConnection_c06880a914344f87a1e7dc0c7f98f8d5_Out_3_Vector3 = _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3_IsConnected ? _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3 : IN.WorldSpaceNormal;
        Bindings_MainLight_970d962ef25c3084987fd10643902eed_float _MainLight_a11a73eba1b4eb8699ff9346f7a84fda;
        float3 _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Direction_1_Vector3;
        float3 _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Colour_2_Vector3;
        float _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_CullingMask_3_Float;
        SG_MainLight_970d962ef25c3084987fd10643902eed_float(_MainLight_a11a73eba1b4eb8699ff9346f7a84fda, _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Direction_1_Vector3, _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Colour_2_Vector3, _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_CullingMask_3_Float);
        float _DotProduct_008c8bbd970f3d8d999b2ff0c459d4ec_Out_2_Float;
        Unity_DotProduct_float3(_BranchOnInputConnection_c06880a914344f87a1e7dc0c7f98f8d5_Out_3_Vector3, _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Direction_1_Vector3, _DotProduct_008c8bbd970f3d8d999b2ff0c459d4ec_Out_2_Float);
        float _Saturate_d002d01fed54407384c164ab19f9507d_Out_1_Float;
        Unity_Saturate_float(_DotProduct_008c8bbd970f3d8d999b2ff0c459d4ec_Out_2_Float, _Saturate_d002d01fed54407384c164ab19f9507d_Out_1_Float);
        float3 _Multiply_47c9454c229743b19d34826967c01572_Out_2_Vector3;
        Unity_Multiply_float3_float3(_MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Colour_2_Vector3, (_MainLight_a11a73eba1b4eb8699ff9346f7a84fda_CullingMask_3_Float.xxx), _Multiply_47c9454c229743b19d34826967c01572_Out_2_Vector3);
        float3 _Multiply_d68b503fcf6f43fc9e44d53916c33ae5_Out_2_Vector3;
        Unity_Multiply_float3_float3((_Saturate_d002d01fed54407384c164ab19f9507d_Out_1_Float.xxx), _Multiply_47c9454c229743b19d34826967c01572_Out_2_Vector3, _Multiply_d68b503fcf6f43fc9e44d53916c33ae5_Out_2_Vector3);
        Diffuse_3 = _Multiply_d68b503fcf6f43fc9e44d53916c33ae5_Out_2_Vector3;
        }
        
        void Unity_Reflection_float3(float3 In, float3 Normal, out float3 Out)
        {
            Out = reflect(In, Normal);
        }
        
        void Unity_Negate_float3(float3 In, out float3 Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Exponential2_float(float In, out float Out)
        {
            Out = exp2(In);
        }
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        struct Bindings_MainLightSpecularHighlights_0c0d4473bed74ba4bbba1f27d6666667_float
        {
        float3 WorldSpaceNormal;
        float3 WorldSpaceViewDirection;
        };
        
        void SG_MainLightSpecularHighlights_0c0d4473bed74ba4bbba1f27d6666667_float(float Vector1_418D9C5F, float3 _Normal, bool _Normal_62e59cd14b7d4dc09c6989fd7428f0b5_IsConnected, int _Model, Bindings_MainLightSpecularHighlights_0c0d4473bed74ba4bbba1f27d6666667_float IN, out float Specular_3)
        {
        Bindings_MainLight_970d962ef25c3084987fd10643902eed_float _MainLight_a11a73eba1b4eb8699ff9346f7a84fda;
        float3 _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Direction_1_Vector3;
        float3 _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Colour_2_Vector3;
        float _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_CullingMask_3_Float;
        SG_MainLight_970d962ef25c3084987fd10643902eed_float(_MainLight_a11a73eba1b4eb8699ff9346f7a84fda, _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Direction_1_Vector3, _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Colour_2_Vector3, _MainLight_a11a73eba1b4eb8699ff9346f7a84fda_CullingMask_3_Float);
        float3 _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3 = _Normal;
        bool _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3_IsConnected = _Normal_62e59cd14b7d4dc09c6989fd7428f0b5_IsConnected;
        float3 _BranchOnInputConnection_c06880a914344f87a1e7dc0c7f98f8d5_Out_3_Vector3 = _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3_IsConnected ? _Property_88244bcc1ec342a189c75b9d64d198f2_Out_0_Vector3 : IN.WorldSpaceNormal;
        float3 _Reflection_4a28a766916e460e9fdc882fc6507480_Out_2_Vector3;
        Unity_Reflection_float3(_MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Direction_1_Vector3, _BranchOnInputConnection_c06880a914344f87a1e7dc0c7f98f8d5_Out_3_Vector3, _Reflection_4a28a766916e460e9fdc882fc6507480_Out_2_Vector3);
        float3 _Negate_f67ea12148664e93b83f6a752eb81e1e_Out_1_Vector3;
        Unity_Negate_float3(_Reflection_4a28a766916e460e9fdc882fc6507480_Out_2_Vector3, _Negate_f67ea12148664e93b83f6a752eb81e1e_Out_1_Vector3);
        float _DotProduct_33c685e11a31435c80a76250bcbd8827_Out_2_Float;
        Unity_DotProduct_float3(_Negate_f67ea12148664e93b83f6a752eb81e1e_Out_1_Vector3, IN.WorldSpaceViewDirection, _DotProduct_33c685e11a31435c80a76250bcbd8827_Out_2_Float);
        float3 _Add_4c027eb047038b8c97203d16def61b12_Out_2_Vector3;
        Unity_Add_float3(_MainLight_a11a73eba1b4eb8699ff9346f7a84fda_Direction_1_Vector3, IN.WorldSpaceViewDirection, _Add_4c027eb047038b8c97203d16def61b12_Out_2_Vector3);
        float3 _Normalize_34edc3d07919da8b890baa7bf05ad4ef_Out_1_Vector3;
        Unity_Normalize_float3(_Add_4c027eb047038b8c97203d16def61b12_Out_2_Vector3, _Normalize_34edc3d07919da8b890baa7bf05ad4ef_Out_1_Vector3);
        float _DotProduct_008c8bbd970f3d8d999b2ff0c459d4ec_Out_2_Float;
        Unity_DotProduct_float3(_Normalize_34edc3d07919da8b890baa7bf05ad4ef_Out_1_Vector3, _BranchOnInputConnection_c06880a914344f87a1e7dc0c7f98f8d5_Out_3_Vector3, _DotProduct_008c8bbd970f3d8d999b2ff0c459d4ec_Out_2_Float);
        float _Model_61ba182e187045b88fe40b0454ac38d6_Out_0_Float;
        if (_Model == 0)
        {
        _Model_61ba182e187045b88fe40b0454ac38d6_Out_0_Float = _DotProduct_33c685e11a31435c80a76250bcbd8827_Out_2_Float;
        }
        else if (_Model == 1)
        {
        _Model_61ba182e187045b88fe40b0454ac38d6_Out_0_Float = _DotProduct_008c8bbd970f3d8d999b2ff0c459d4ec_Out_2_Float;
        }
        else
        {
        _Model_61ba182e187045b88fe40b0454ac38d6_Out_0_Float = _DotProduct_33c685e11a31435c80a76250bcbd8827_Out_2_Float;
        }
        float _Saturate_cec09b6dc1cf8c889a0df4287aa092fe_Out_1_Float;
        Unity_Saturate_float(_Model_61ba182e187045b88fe40b0454ac38d6_Out_0_Float, _Saturate_cec09b6dc1cf8c889a0df4287aa092fe_Out_1_Float);
        float _Property_8abf074a48543d89ae3ae8359fc3942d_Out_0_Float = Vector1_418D9C5F;
        float _Multiply_dcedefef20f3a9848abf83c8d289c102_Out_2_Float;
        Unity_Multiply_float_float(_Property_8abf074a48543d89ae3ae8359fc3942d_Out_0_Float, 10, _Multiply_dcedefef20f3a9848abf83c8d289c102_Out_2_Float);
        float _Add_9c471db1dfaab7899348f3ed25c3b613_Out_2_Float;
        Unity_Add_float(_Multiply_dcedefef20f3a9848abf83c8d289c102_Out_2_Float, 1, _Add_9c471db1dfaab7899348f3ed25c3b613_Out_2_Float);
        float _Exponential_16bce0c49655728e839f1289bdfdb8b4_Out_1_Float;
        Unity_Exponential2_float(_Add_9c471db1dfaab7899348f3ed25c3b613_Out_2_Float, _Exponential_16bce0c49655728e839f1289bdfdb8b4_Out_1_Float);
        float _Power_417fdd63824c58869981d453e27ae1af_Out_2_Float;
        Unity_Power_float(_Saturate_cec09b6dc1cf8c889a0df4287aa092fe_Out_1_Float, _Exponential_16bce0c49655728e839f1289bdfdb8b4_Out_1_Float, _Power_417fdd63824c58869981d453e27ae1af_Out_2_Float);
        Specular_3 = _Power_417fdd63824c58869981d453e27ae1af_Out_2_Float;
        }
        
        void Unity_ColorspaceConversion_RGB_HSV_float(float3 In, out float3 Out)
        {
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float  E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            Out = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_OneMinus_float4(float4 In, out float4 Out)
        {
            Out = 1 - In;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_Voronoi_RandomVector_Deterministic_float (float2 UV, float offset)
        {
        Hash_Tchou_2_2_float(UV, UV);
        return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float t = 8.0;
        float3 res = float3(8.0, 0.0, 0.0);
        for (int y = -1; y <= 1; y++)
        {
        for (int x = -1; x <= 1; x++)
        {
        float2 lattice = float2(x, y);
        float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
        float d = distance(lattice + offset, f);
        if (d < res.x)
        {
        res = float3(d, offset.x, offset.y);
        Out = res.x;
        Cells = res.y;
        }
        }
        }
        }
        
        void Unity_Comparison_LessOrEqual_float(float A, float B, out float Out)
        {
            Out = A <= B ? 1 : 0;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
        Out = A * B;
        }
        
        struct Bindings_Dots2_cd4f5053a8a2b5643b5346c288777044_float
        {
        float2 NDCPosition;
        };
        
        void SG_Dots2_cd4f5053a8a2b5643b5346c288777044_float(float2 _Tiling, float _OffsetX, float _OffsetY, float _Size, float _Cut, float4 _Color, Bindings_Dots2_cd4f5053a8a2b5643b5346c288777044_float IN, out float4 OutVector4_1)
        {
        float4 _ScreenPosition_449d297018ea49a9b581f91c3ab1c1d0_Out_0_Vector4 = frac(float4((IN.NDCPosition.x * 2 - 1) * _ScreenParams.x / _ScreenParams.y, IN.NDCPosition.y * 2 - 1, 0, 0));
        float2 _Property_a23336e2d4f248568b0134555d9042a5_Out_0_Vector2 = _Tiling;
        float _Property_bbb63e4be64b4a899a3539854f2b78d5_Out_0_Float = _OffsetX;
        float _Property_ecd00d63441d4df8ba47bfb93d223a4e_Out_0_Float = _OffsetY;
        float2 _Vector2_6cf89aded256472c8ee7763a06c619ca_Out_0_Vector2 = float2(_Property_bbb63e4be64b4a899a3539854f2b78d5_Out_0_Float, _Property_ecd00d63441d4df8ba47bfb93d223a4e_Out_0_Float);
        float2 _TilingAndOffset_49475895bd75436789ff47609cb64b80_Out_3_Vector2;
        Unity_TilingAndOffset_float((_ScreenPosition_449d297018ea49a9b581f91c3ab1c1d0_Out_0_Vector4.xy), _Property_a23336e2d4f248568b0134555d9042a5_Out_0_Vector2, _Vector2_6cf89aded256472c8ee7763a06c619ca_Out_0_Vector2, _TilingAndOffset_49475895bd75436789ff47609cb64b80_Out_3_Vector2);
        float _Property_3f4f5558402444f2894f52cdf581baa3_Out_0_Float = _Size;
        float _Voronoi_b7a35e7526f44d2889184fda3450d8c4_Out_3_Float;
        float _Voronoi_b7a35e7526f44d2889184fda3450d8c4_Cells_4_Float;
        Unity_Voronoi_Deterministic_float(_TilingAndOffset_49475895bd75436789ff47609cb64b80_Out_3_Vector2, 0, _Property_3f4f5558402444f2894f52cdf581baa3_Out_0_Float, _Voronoi_b7a35e7526f44d2889184fda3450d8c4_Out_3_Float, _Voronoi_b7a35e7526f44d2889184fda3450d8c4_Cells_4_Float);
        float _Property_a067b76d30fa4242868ef356b6838ad3_Out_0_Float = _Cut;
        float _Comparison_799c453d7f804816a297cccd9b14ffa4_Out_2_Boolean;
        Unity_Comparison_LessOrEqual_float(_Voronoi_b7a35e7526f44d2889184fda3450d8c4_Out_3_Float, _Property_a067b76d30fa4242868ef356b6838ad3_Out_0_Float, _Comparison_799c453d7f804816a297cccd9b14ffa4_Out_2_Boolean);
        float4 _Property_133f03f5b1f242c8abba77584082b088_Out_0_Vector4 = _Color;
        float4 _Multiply_91feb0e7390f484fa0fca06e919edc71_Out_2_Vector4;
        Unity_Multiply_float4_float4((((float) _Comparison_799c453d7f804816a297cccd9b14ffa4_Out_2_Boolean).xxxx), _Property_133f03f5b1f242c8abba77584082b088_Out_0_Vector4, _Multiply_91feb0e7390f484fa0fca06e919edc71_Out_2_Vector4);
        float4 _OneMinus_3b838471930c4727b9a46c41faccb5d9_Out_1_Vector4;
        Unity_OneMinus_float4(_Multiply_91feb0e7390f484fa0fca06e919edc71_Out_2_Vector4, _OneMinus_3b838471930c4727b9a46c41faccb5d9_Out_1_Vector4);
        OutVector4_1 = _OneMinus_3b838471930c4727b9a46c41faccb5d9_Out_1_Vector4;
        }
        
        void Unity_ColorspaceConversion_HSV_RGB_float(float3 In, out float3 Out)
        {
            float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P = abs(frac(In.xxx + K.xyz) * 6.0 - K.www);
            Out = In.z * lerp(K.xxx, saturate(P - K.xxx), In.y);
        }
        
        void Unity_BakedGIScale_float(float3 Normal, out float3 Out, float3 Position, float2 StaticUV, float2 DynamicUV)
        {
            Out = SHADERGRAPH_BAKED_GI(Position, Normal, StaticUV, DynamicUV, true);
        }
        
        void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            Rotation = Rotation * (3.1415926f/180.0f);
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Rectangle_Fastest_float(float2 UV, float Width, float Height, out float Out)
        {
            float2 d = abs(UV * 2 - 1) - float2(Width, Height);
        #if defined(SHADER_STAGE_RAY_TRACING)
            d = saturate((1 - saturate(d * 1e7)));
        #else
            d = saturate(1 - d / fwidth(d));
        #endif
            Out = min(d.x, d.y);
        }
        
        struct Bindings_Stripes_43e352e91791bb841806d53c0f6085a0_float
        {
        float3 ViewSpacePosition;
        };
        
        void SG_Stripes_43e352e91791bb841806d53c0f6085a0_float(float _Rotation, float _Frequency, float _Offset, float _Thickness, Bindings_Stripes_43e352e91791bb841806d53c0f6085a0_float IN, out float OutVector1_1)
        {
        float _Property_e50fa2850cbb452f91866696acc3d6f1_Out_0_Float = _Rotation;
        float2 _Rotate_b2b0fe5189c64adb8fbcbf1419a5d456_Out_3_Vector2;
        Unity_Rotate_Degrees_float((IN.ViewSpacePosition.xy), float2 (0.5, 0.5), _Property_e50fa2850cbb452f91866696acc3d6f1_Out_0_Float, _Rotate_b2b0fe5189c64adb8fbcbf1419a5d456_Out_3_Vector2);
        float _Property_eebbc4850ab546e1af1238bb62608304_Out_0_Float = _Frequency;
        float2 _Vector2_e400cc4375ea405c8f72b4d708cc2efa_Out_0_Vector2 = float2(_Property_eebbc4850ab546e1af1238bb62608304_Out_0_Float, 1);
        float _Property_92d9153a5e74477c97ecccb33e44204d_Out_0_Float = _Offset;
        float2 _Vector2_6f595304fecd4ad8a24a01e674b1e8b5_Out_0_Vector2 = float2(_Property_92d9153a5e74477c97ecccb33e44204d_Out_0_Float, 0);
        float2 _TilingAndOffset_1a5ecd59f07647f99b59187e63ad126b_Out_3_Vector2;
        Unity_TilingAndOffset_float(_Rotate_b2b0fe5189c64adb8fbcbf1419a5d456_Out_3_Vector2, _Vector2_e400cc4375ea405c8f72b4d708cc2efa_Out_0_Vector2, _Vector2_6f595304fecd4ad8a24a01e674b1e8b5_Out_0_Vector2, _TilingAndOffset_1a5ecd59f07647f99b59187e63ad126b_Out_3_Vector2);
        float2 _Fraction_f82c4d10a52f4ed386472c3f674f1b97_Out_1_Vector2;
        Unity_Fraction_float2(_TilingAndOffset_1a5ecd59f07647f99b59187e63ad126b_Out_3_Vector2, _Fraction_f82c4d10a52f4ed386472c3f674f1b97_Out_1_Vector2);
        float _Property_2ba72d2d492342548b7878e838982120_Out_0_Float = _Thickness;
        float _Rectangle_ade8ab79effe41b5a774d02bb8bd768a_Out_3_Float;
        Unity_Rectangle_Fastest_float(_Fraction_f82c4d10a52f4ed386472c3f674f1b97_Out_1_Vector2, _Property_2ba72d2d492342548b7878e838982120_Out_0_Float, 1, _Rectangle_ade8ab79effe41b5a774d02bb8bd768a_Out_3_Float);
        OutVector1_1 = _Rectangle_ade8ab79effe41b5a774d02bb8bd768a_Out_3_Float;
        }
        
        void Unity_Negate_float(float In, out float Out)
        {
            Out = -1 * In;
        }
        
        void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
        {
            Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
        }
        
        // void Unity_Power_float3(float3 A, float3 B, out float3 Out)
        // {
        //     Out = pow(A, B);
        // }

        void Unity_Power_float3(float3 A, float3 B, out float3 Out)
        {
          Out = pow(abs(A), B);
        }

        
        void Unity_Smoothstep_float3(float3 Edge1, float3 Edge2, float3 In, out float3 Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Saturate_float3(float3 In, out float3 Out)
        {
            Out = saturate(In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_1490f7625ad2cc8e9cedc9e9dc78f710_Out_0_Float = Vector1_46F4FBA0;
            float _Property_cc035d9d2fd34f13a8a0fa32b90e65da_Out_0_Float = Vector1_ADAD9479;
            Bindings_MainLightDiffuse_54e5956b307254749804a49de2643ea4_float _MainLightDiffuse_dd9264b117004692ad7d83d8ac884262;
            _MainLightDiffuse_dd9264b117004692ad7d83d8ac884262.WorldSpaceNormal = IN.WorldSpaceNormal;
            float3 _MainLightDiffuse_dd9264b117004692ad7d83d8ac884262_Diffuse_3_Vector3;
            SG_MainLightDiffuse_54e5956b307254749804a49de2643ea4_float(IN.WorldSpaceNormal, true, _MainLightDiffuse_dd9264b117004692ad7d83d8ac884262, _MainLightDiffuse_dd9264b117004692ad7d83d8ac884262_Diffuse_3_Vector3);
            Bindings_MainLightSpecularHighlights_0c0d4473bed74ba4bbba1f27d6666667_float _MainLightSpecularHighlights_ad6312dec6c44000b7e05e2eb27d2f28;
            _MainLightSpecularHighlights_ad6312dec6c44000b7e05e2eb27d2f28.WorldSpaceNormal = IN.WorldSpaceNormal;
            _MainLightSpecularHighlights_ad6312dec6c44000b7e05e2eb27d2f28.WorldSpaceViewDirection = IN.WorldSpaceViewDirection;
            float _MainLightSpecularHighlights_ad6312dec6c44000b7e05e2eb27d2f28_Specular_3_Float;
            SG_MainLightSpecularHighlights_0c0d4473bed74ba4bbba1f27d6666667_float(_Property_cc035d9d2fd34f13a8a0fa32b90e65da_Out_0_Float, _MainLightDiffuse_dd9264b117004692ad7d83d8ac884262_Diffuse_3_Vector3, true, 1, _MainLightSpecularHighlights_ad6312dec6c44000b7e05e2eb27d2f28, _MainLightSpecularHighlights_ad6312dec6c44000b7e05e2eb27d2f28_Specular_3_Float);
            float3 _ColorspaceConversion_7b1b768b639dcc81870e0f8a35157f2f_Out_1_Vector3;
            Unity_ColorspaceConversion_RGB_HSV_float((_MainLightSpecularHighlights_ad6312dec6c44000b7e05e2eb27d2f28_Specular_3_Float.xxx), _ColorspaceConversion_7b1b768b639dcc81870e0f8a35157f2f_Out_1_Vector3);
            float _Split_df4520876b48138aac4685979ffaaee3_R_1_Float = _ColorspaceConversion_7b1b768b639dcc81870e0f8a35157f2f_Out_1_Vector3[0];
            float _Split_df4520876b48138aac4685979ffaaee3_G_2_Float = _ColorspaceConversion_7b1b768b639dcc81870e0f8a35157f2f_Out_1_Vector3[1];
            float _Split_df4520876b48138aac4685979ffaaee3_B_3_Float = _ColorspaceConversion_7b1b768b639dcc81870e0f8a35157f2f_Out_1_Vector3[2];
            float _Split_df4520876b48138aac4685979ffaaee3_A_4_Float = 0;
            float4 _SampleGradient_504fffb42a0a40cb9329b366f03a77a6_Out_2_Vector4;
            Unity_SampleGradientV1_float(NewGradient(0, 2, 2, float4(1, 1, 1, 0),float4(1, 1, 1, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0)), _Split_df4520876b48138aac4685979ffaaee3_B_3_Float, _SampleGradient_504fffb42a0a40cb9329b366f03a77a6_Out_2_Vector4);
            float4 _OneMinus_ad5f8e13aef1d08cbcec975397a5bc37_Out_1_Vector4;
            Unity_OneMinus_float4(_SampleGradient_504fffb42a0a40cb9329b366f03a77a6_Out_2_Vector4, _OneMinus_ad5f8e13aef1d08cbcec975397a5bc37_Out_1_Vector4);
            Bindings_Dots2_cd4f5053a8a2b5643b5346c288777044_float _Dots2_443e3fcc0efc4c3db644c258c07e9ec3;
            _Dots2_443e3fcc0efc4c3db644c258c07e9ec3.NDCPosition = IN.NDCPosition;
            float4 _Dots2_443e3fcc0efc4c3db644c258c07e9ec3_OutVector4_1_Vector4;
            SG_Dots2_cd4f5053a8a2b5643b5346c288777044_float((_Property_1490f7625ad2cc8e9cedc9e9dc78f710_Out_0_Float.xx), 0.25, 1, (_OneMinus_ad5f8e13aef1d08cbcec975397a5bc37_Out_1_Vector4).x, 1, float4 (0, 0, 0, 0), _Dots2_443e3fcc0efc4c3db644c258c07e9ec3, _Dots2_443e3fcc0efc4c3db644c258c07e9ec3_OutVector4_1_Vector4);
            float4 _OneMinus_67fee3076ef19886958c3be1f758b40d_Out_1_Vector4;
            Unity_OneMinus_float4(_Dots2_443e3fcc0efc4c3db644c258c07e9ec3_OutVector4_1_Vector4, _OneMinus_67fee3076ef19886958c3be1f758b40d_Out_1_Vector4);
            float3 _Vector3_c6ab0760dba8e4808823d397ffe22838_Out_0_Vector3 = float3(_Split_df4520876b48138aac4685979ffaaee3_R_1_Float, _Split_df4520876b48138aac4685979ffaaee3_G_2_Float, (_SampleGradient_504fffb42a0a40cb9329b366f03a77a6_Out_2_Vector4).x);
            float3 _ColorspaceConversion_ffd8139ad42e9d8bbb8715d47f09de29_Out_1_Vector3;
            Unity_ColorspaceConversion_HSV_RGB_float(_Vector3_c6ab0760dba8e4808823d397ffe22838_Out_0_Vector3, _ColorspaceConversion_ffd8139ad42e9d8bbb8715d47f09de29_Out_1_Vector3);
            float3 _Multiply_e9da0cd8ba06558eaace96ff3346b680_Out_2_Vector3;
            Unity_Multiply_float3_float3((_OneMinus_67fee3076ef19886958c3be1f758b40d_Out_1_Vector4.xyz), _ColorspaceConversion_ffd8139ad42e9d8bbb8715d47f09de29_Out_1_Vector3, _Multiply_e9da0cd8ba06558eaace96ff3346b680_Out_2_Vector3);
            float3 _BakedGI_238325ac614272838232628a055a0bcc_Out_1_Vector3;
            Unity_BakedGIScale_float(IN.WorldSpaceNormal, _BakedGI_238325ac614272838232628a055a0bcc_Out_1_Vector3, IN.WorldSpacePosition, IN.uv1.xy, IN.uv2.xy);
            float3 _ColorspaceConversion_cca4684fbf69fc83b80a3131babaf9d7_Out_1_Vector3;
            Unity_ColorspaceConversion_RGB_HSV_float(_MainLightDiffuse_dd9264b117004692ad7d83d8ac884262_Diffuse_3_Vector3, _ColorspaceConversion_cca4684fbf69fc83b80a3131babaf9d7_Out_1_Vector3);
            float _Split_afa5974de947d980ba55f3d6d2010e97_R_1_Float = _ColorspaceConversion_cca4684fbf69fc83b80a3131babaf9d7_Out_1_Vector3[0];
            float _Split_afa5974de947d980ba55f3d6d2010e97_G_2_Float = _ColorspaceConversion_cca4684fbf69fc83b80a3131babaf9d7_Out_1_Vector3[1];
            float _Split_afa5974de947d980ba55f3d6d2010e97_B_3_Float = _ColorspaceConversion_cca4684fbf69fc83b80a3131babaf9d7_Out_1_Vector3[2];
            float _Split_afa5974de947d980ba55f3d6d2010e97_A_4_Float = 0;
            float4 _SampleGradient_cd0a65432491568189e15cec0ec3e68f_Out_2_Vector4;
            Unity_SampleGradientV1_float(NewGradient(1, 6, 2, float4(0, 0, 0, 0.01176471),float4(0.2735849, 0.2735849, 0.2735849, 0.04411383),float4(0.4433962, 0.4433962, 0.4433962, 0.1205921),float4(0.745283, 0.745283, 0.745283, 0.4441138),float4(0.9056604, 0.9056604, 0.9056604, 0.7088273),float4(1, 1, 1, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0)), _Split_afa5974de947d980ba55f3d6d2010e97_B_3_Float, _SampleGradient_cd0a65432491568189e15cec0ec3e68f_Out_2_Vector4);
            float3 _Vector3_378a8bd25a238e8dbab669930fdea6be_Out_0_Vector3 = float3(_Split_afa5974de947d980ba55f3d6d2010e97_R_1_Float, _Split_afa5974de947d980ba55f3d6d2010e97_G_2_Float, (_SampleGradient_cd0a65432491568189e15cec0ec3e68f_Out_2_Vector4).x);
            float3 _ColorspaceConversion_e8176222117e168abab50644addb4f50_Out_1_Vector3;
            Unity_ColorspaceConversion_HSV_RGB_float(_Vector3_378a8bd25a238e8dbab669930fdea6be_Out_0_Vector3, _ColorspaceConversion_e8176222117e168abab50644addb4f50_Out_1_Vector3);
            float _Float_3423d9d791070c8a8074c67e592f4444_Out_0_Float = 45;
            float _Property_ca972b8f46e2a083b8ce965473ed47e3_Out_0_Float = Vector1_EE54776A;
            float _Power_f19f4e84c645b78fb7b14f34463626e9_Out_2_Float;
            Unity_Power_float(_Split_afa5974de947d980ba55f3d6d2010e97_B_3_Float, 2, _Power_f19f4e84c645b78fb7b14f34463626e9_Out_2_Float);
            float4 _SampleGradient_f6be79fa28c52a86866ed42bfa52edfe_Out_2_Vector4;
            Unity_SampleGradientV1_float(NewGradient(0, 3, 2, float4(0.2169811, 0.2169811, 0.2169811, 0),float4(0.75, 0.75, 0.75, 0.20589),float4(0.97, 0.97, 0.97, 0.5823606),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0)), _Power_f19f4e84c645b78fb7b14f34463626e9_Out_2_Float, _SampleGradient_f6be79fa28c52a86866ed42bfa52edfe_Out_2_Vector4);
            Bindings_Stripes_43e352e91791bb841806d53c0f6085a0_float _Stripes_85a57884f4104644ba81af1c397524bf;
            _Stripes_85a57884f4104644ba81af1c397524bf.ViewSpacePosition = IN.ViewSpacePosition;
            float _Stripes_85a57884f4104644ba81af1c397524bf_OutVector1_1_Float;
            SG_Stripes_43e352e91791bb841806d53c0f6085a0_float(_Float_3423d9d791070c8a8074c67e592f4444_Out_0_Float, _Property_ca972b8f46e2a083b8ce965473ed47e3_Out_0_Float, 0, (_SampleGradient_f6be79fa28c52a86866ed42bfa52edfe_Out_2_Vector4).x, _Stripes_85a57884f4104644ba81af1c397524bf, _Stripes_85a57884f4104644ba81af1c397524bf_OutVector1_1_Float);
            float _Negate_22f128bbccc3ca89945e8b083500d2e6_Out_1_Float;
            Unity_Negate_float(_Float_3423d9d791070c8a8074c67e592f4444_Out_0_Float, _Negate_22f128bbccc3ca89945e8b083500d2e6_Out_1_Float);
            float4 _SampleGradient_bab137f5985d8c8a8eca8b440a2bfed9_Out_2_Vector4;
            Unity_SampleGradientV1_float(NewGradient(0, 2, 2, float4(0.5188679, 0.5188679, 0.5188679, 0),float4(0.99, 0.99, 0.99, 0.1499962),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0)), _Power_f19f4e84c645b78fb7b14f34463626e9_Out_2_Float, _SampleGradient_bab137f5985d8c8a8eca8b440a2bfed9_Out_2_Vector4);
            Bindings_Stripes_43e352e91791bb841806d53c0f6085a0_float _Stripes_bbb25da840ef44a4bac8ea169eeec44f;
            _Stripes_bbb25da840ef44a4bac8ea169eeec44f.ViewSpacePosition = IN.ViewSpacePosition;
            float _Stripes_bbb25da840ef44a4bac8ea169eeec44f_OutVector1_1_Float;
            SG_Stripes_43e352e91791bb841806d53c0f6085a0_float(_Negate_22f128bbccc3ca89945e8b083500d2e6_Out_1_Float, _Property_ca972b8f46e2a083b8ce965473ed47e3_Out_0_Float, 0, (_SampleGradient_bab137f5985d8c8a8eca8b440a2bfed9_Out_2_Vector4).x, _Stripes_bbb25da840ef44a4bac8ea169eeec44f, _Stripes_bbb25da840ef44a4bac8ea169eeec44f_OutVector1_1_Float);
            float _Multiply_0076b5d9f9162288a48ceb95dd0d0854_Out_2_Float;
            Unity_Multiply_float_float(_Stripes_85a57884f4104644ba81af1c397524bf_OutVector1_1_Float, _Stripes_bbb25da840ef44a4bac8ea169eeec44f_OutVector1_1_Float, _Multiply_0076b5d9f9162288a48ceb95dd0d0854_Out_2_Float);
            float3 _Multiply_446c90f0322a858d860ec0594c5a9ca7_Out_2_Vector3;
            Unity_Multiply_float3_float3(_ColorspaceConversion_e8176222117e168abab50644addb4f50_Out_1_Vector3, (_Multiply_0076b5d9f9162288a48ceb95dd0d0854_Out_2_Float.xxx), _Multiply_446c90f0322a858d860ec0594c5a9ca7_Out_2_Vector3);
            float3 _Add_d7531825361c3a82962ba04bf6ee40c4_Out_2_Vector3;
            Unity_Add_float3(_BakedGI_238325ac614272838232628a055a0bcc_Out_1_Vector3, _Multiply_446c90f0322a858d860ec0594c5a9ca7_Out_2_Vector3, _Add_d7531825361c3a82962ba04bf6ee40c4_Out_2_Vector3);
            UnityTexture2D _Property_667d1d5b5a9a5c8c8b5940a68b940522_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(Texture2D_DC6DF663);
            float2 _Property_a750d6f5b11c3685923e377fee78645e_Out_0_Vector2 = Vector2_CFCD19EF;
            float2 _TilingAndOffset_0db02468182cb18a922793adeb44f2b5_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_a750d6f5b11c3685923e377fee78645e_Out_0_Vector2, float2 (0, 0), _TilingAndOffset_0db02468182cb18a922793adeb44f2b5_Out_3_Vector2);
            float4 _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_667d1d5b5a9a5c8c8b5940a68b940522_Out_0_Texture2D.tex, _Property_667d1d5b5a9a5c8c8b5940a68b940522_Out_0_Texture2D.samplerstate, _Property_667d1d5b5a9a5c8c8b5940a68b940522_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_0db02468182cb18a922793adeb44f2b5_Out_3_Vector2) );
            float _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_R_4_Float = _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_RGBA_0_Vector4.r;
            float _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_G_5_Float = _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_RGBA_0_Vector4.g;
            float _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_B_6_Float = _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_RGBA_0_Vector4.b;
            float _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_A_7_Float = _SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_RGBA_0_Vector4.a;
            float3 _Multiply_ca1ede823d5837839f75926516925dac_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Add_d7531825361c3a82962ba04bf6ee40c4_Out_2_Vector3, (_SampleTexture2D_d3d8e5dd37beaa8faf3efb759553211d_RGBA_0_Vector4.xyz), _Multiply_ca1ede823d5837839f75926516925dac_Out_2_Vector3);
            float4 _Property_516ba759f76ed2879cca710b1d9751fc_Out_0_Vector4 = Color_DAC4E11B;
            float3 _Multiply_79a9021c1e665e84b9fda537f6366f38_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Multiply_ca1ede823d5837839f75926516925dac_Out_2_Vector3, (_Property_516ba759f76ed2879cca710b1d9751fc_Out_0_Vector4.xyz), _Multiply_79a9021c1e665e84b9fda537f6366f38_Out_2_Vector3);
            float3 _Add_b9c2ef2afe4f2e80877e867502bf7662_Out_2_Vector3;
            Unity_Add_float3(_Multiply_e9da0cd8ba06558eaace96ff3346b680_Out_2_Vector3, _Multiply_79a9021c1e665e84b9fda537f6366f38_Out_2_Vector3, _Add_b9c2ef2afe4f2e80877e867502bf7662_Out_2_Vector3);
            float _Property_474cb0b2cacd37828abc4534bcf39f0d_Out_0_Float = Vector1_29C6AB4C;
            float _FresnelEffect_48920939693b3c8e8b31d28171098072_Out_3_Float;
            Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, 1, _FresnelEffect_48920939693b3c8e8b31d28171098072_Out_3_Float);
            float _Float_408d0f02b7a0098bb5cc1f4b68eff2b0_Out_0_Float = 0.15;
            float3 _Power_a15bb7fdfd4bf889aeb1e45cfb80ccb4_Out_2_Vector3;
            Unity_Power_float3(_MainLightDiffuse_dd9264b117004692ad7d83d8ac884262_Diffuse_3_Vector3, (_Float_408d0f02b7a0098bb5cc1f4b68eff2b0_Out_0_Float.xxx), _Power_a15bb7fdfd4bf889aeb1e45cfb80ccb4_Out_2_Vector3);
            float3 _Multiply_f6c7d3881b68b18da0982e92ddc9310a_Out_2_Vector3;
            Unity_Multiply_float3_float3((_FresnelEffect_48920939693b3c8e8b31d28171098072_Out_3_Float.xxx), _Power_a15bb7fdfd4bf889aeb1e45cfb80ccb4_Out_2_Vector3, _Multiply_f6c7d3881b68b18da0982e92ddc9310a_Out_2_Vector3);
            float3 _Smoothstep_9422e72bee6ba380bdec23452e1fd5c2_Out_3_Vector3;
            Unity_Smoothstep_float3((_Property_474cb0b2cacd37828abc4534bcf39f0d_Out_0_Float.xxx), float3(1, 1, 1), _Multiply_f6c7d3881b68b18da0982e92ddc9310a_Out_2_Vector3, _Smoothstep_9422e72bee6ba380bdec23452e1fd5c2_Out_3_Vector3);
            float3 _Vector3_d11bc66441d730818b757f111e2cde6c_Out_0_Vector3 = float3(0, 1, 0);
            float _DotProduct_fb53d4a1698ebc8e80b3d8726595c1c8_Out_2_Float;
            Unity_DotProduct_float3(IN.WorldSpaceNormal, _Vector3_d11bc66441d730818b757f111e2cde6c_Out_0_Vector3, _DotProduct_fb53d4a1698ebc8e80b3d8726595c1c8_Out_2_Float);
            float _Float_8b54f2d83a656885ac7c182a728e4ae1_Out_0_Float = 1;
            float _Subtract_04abea9d0c16d78199c6200bc793540a_Out_2_Float;
            Unity_Subtract_float(_DotProduct_fb53d4a1698ebc8e80b3d8726595c1c8_Out_2_Float, _Float_8b54f2d83a656885ac7c182a728e4ae1_Out_0_Float, _Subtract_04abea9d0c16d78199c6200bc793540a_Out_2_Float);
            float _Property_1e8aea2a3006c587b94489b9342462c6_Out_0_Float = Vector1_E023A9A8;
            float _Float_b921998a363cc18380c4933730923810_Out_0_Float = _Property_1e8aea2a3006c587b94489b9342462c6_Out_0_Float;
            float _Subtract_1b784cc35a1c68868e169934be498172_Out_2_Float;
            Unity_Subtract_float(_Float_b921998a363cc18380c4933730923810_Out_0_Float, _Float_8b54f2d83a656885ac7c182a728e4ae1_Out_0_Float, _Subtract_1b784cc35a1c68868e169934be498172_Out_2_Float);
            float _Divide_445a9bee09d0948da2d61b869aa7d020_Out_2_Float;
            Unity_Divide_float(_Subtract_04abea9d0c16d78199c6200bc793540a_Out_2_Float, _Subtract_1b784cc35a1c68868e169934be498172_Out_2_Float, _Divide_445a9bee09d0948da2d61b869aa7d020_Out_2_Float);
            float _Saturate_8be6411ce431738e970ac3cabe85f019_Out_1_Float;
            Unity_Saturate_float(_Divide_445a9bee09d0948da2d61b869aa7d020_Out_2_Float, _Saturate_8be6411ce431738e970ac3cabe85f019_Out_1_Float);
            float3 _Lerp_6787e1afa4dd5b8d966d761a7fcae2ba_Out_3_Vector3;
            Unity_Lerp_float3(float3(0, 0, 0), _Smoothstep_9422e72bee6ba380bdec23452e1fd5c2_Out_3_Vector3, (_Saturate_8be6411ce431738e970ac3cabe85f019_Out_1_Float.xxx), _Lerp_6787e1afa4dd5b8d966d761a7fcae2ba_Out_3_Vector3);
            float3 _Add_ee48b2bbb0fb13809a45db1fcfbe9b98_Out_2_Vector3;
            Unity_Add_float3(_Add_b9c2ef2afe4f2e80877e867502bf7662_Out_2_Vector3, _Lerp_6787e1afa4dd5b8d966d761a7fcae2ba_Out_3_Vector3, _Add_ee48b2bbb0fb13809a45db1fcfbe9b98_Out_2_Vector3);
            float3 _Saturate_84b07742a86eea8180db5df154e1eddb_Out_1_Vector3;
            Unity_Saturate_float3(_Add_ee48b2bbb0fb13809a45db1fcfbe9b98_Out_2_Vector3, _Saturate_84b07742a86eea8180db5df154e1eddb_Out_1_Vector3);
            surface.BaseColor = _Saturate_84b07742a86eea8180db5df154e1eddb_Out_1_Vector3;
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
        
        
            output.WorldSpaceViewDirection = GetWorldSpaceNormalizeViewDir(input.positionWS);
            output.WorldSpacePosition = input.positionWS;
            output.ViewSpacePosition = TransformWorldToView(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 = input.texCoord0;
            output.uv1 = input.texCoord1;
            output.uv2 = input.texCoord2;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask R
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 Texture2D_DC6DF663_TexelSize;
        float2 Vector2_CFCD19EF;
        float4 Color_DAC4E11B;
        float Vector1_ADAD9479;
        float Vector1_EE54776A;
        float Vector1_46F4FBA0;
        float Vector1_29C6AB4C;
        float Vector1_E023A9A8;
        float _Rotation;
        float _frecuency;
        float _offset;
        float _Thickness;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_DC6DF663);
        SAMPLER(samplerTexture2D_DC6DF663);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 Texture2D_DC6DF663_TexelSize;
        float2 Vector2_CFCD19EF;
        float4 Color_DAC4E11B;
        float Vector1_ADAD9479;
        float Vector1_EE54776A;
        float Vector1_46F4FBA0;
        float Vector1_29C6AB4C;
        float Vector1_E023A9A8;
        float _Rotation;
        float _frecuency;
        float _offset;
        float _Thickness;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_DC6DF663);
        SAMPLER(samplerTexture2D_DC6DF663);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 Texture2D_DC6DF663_TexelSize;
        float2 Vector2_CFCD19EF;
        float4 Color_DAC4E11B;
        float Vector1_ADAD9479;
        float Vector1_EE54776A;
        float Vector1_46F4FBA0;
        float Vector1_29C6AB4C;
        float Vector1_E023A9A8;
        float _Rotation;
        float _frecuency;
        float _offset;
        float _Thickness;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_DC6DF663);
        SAMPLER(samplerTexture2D_DC6DF663);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 Texture2D_DC6DF663_TexelSize;
        float2 Vector2_CFCD19EF;
        float4 Color_DAC4E11B;
        float Vector1_ADAD9479;
        float Vector1_EE54776A;
        float Vector1_46F4FBA0;
        float Vector1_29C6AB4C;
        float Vector1_E023A9A8;
        float _Rotation;
        float _frecuency;
        float _offset;
        float _Thickness;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_DC6DF663);
        SAMPLER(samplerTexture2D_DC6DF663);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 Texture2D_DC6DF663_TexelSize;
        float2 Vector2_CFCD19EF;
        float4 Color_DAC4E11B;
        float Vector1_ADAD9479;
        float Vector1_EE54776A;
        float Vector1_46F4FBA0;
        float Vector1_29C6AB4C;
        float Vector1_E023A9A8;
        float _Rotation;
        float _frecuency;
        float _offset;
        float _Thickness;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(Texture2D_DC6DF663);
        SAMPLER(samplerTexture2D_DC6DF663);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}
