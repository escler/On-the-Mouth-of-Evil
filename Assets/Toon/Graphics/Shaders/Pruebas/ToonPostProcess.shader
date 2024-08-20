Shader "Custom/ToonPostProcess"
{
    Properties
    {
        _Color("Color", Color) = (0.5, 0.65, 1, 1)
        _MainTex("Main Texture", 2D) = "white" {}
        [HDR] _AmbientColor("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
        [HDR] _SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
        _Glossiness("Glossiness", Float) = 32
        [HDR] _RimColor("Rim Color", Color) = (1, 1, 1, 1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles xbox360 ps3
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _AmbientColor;
            float4 _SpecularColor;
            float _Glossiness;
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.pos = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = IN.uv.xy;

                // Calculate view direction
                float4 worldPos = mul(unity_ObjectToWorld, IN.vertex);
                OUT.viewDir = normalize(_WorldSpaceCameraPos - worldPos.xyz);

                OUT.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, IN.normal));

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float3 normal = normalize(IN.worldNormal);
                float3 viewDir = normalize(IN.viewDir);

                // Lighting calculations using URP functions
                float3 lightDir;
                float3 lightColor;

                // Get the lighting information from URP
                Light mainLight = GetMainLight();
                lightDir = -mainLight.direction;
                lightColor = mainLight.color.rgb;

                float NdotL = max(dot(normal, lightDir), 0.0);
                float4 ambient = _AmbientColor;

                float3 halfVector = normalize(lightDir + viewDir);
                float NdotH = max(dot(normal, halfVector), 0.0);
                float specularIntensity = pow(NdotH, _Glossiness);
                float4 specular = specularIntensity * _SpecularColor;

                float rimDot = 1.0 - dot(viewDir, normal);
                float rimIntensity = rimDot * pow(max(dot(normal, lightDir), 0.0), _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor;

                float4 sample = tex2D(_MainTex, IN.uv);

                float4 finalColor = (NdotL * _Color + ambient + specular + rim) * sample;

                return finalColor;
            }
            ENDHLSL
        }
    }

    Fallback "Diffuse"
}
