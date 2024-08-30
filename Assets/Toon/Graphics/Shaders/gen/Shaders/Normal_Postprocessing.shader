Shader "Custom/SceneNormalLighting"
{
    Properties
    {
        _LightDirection ("Light Direction", Vector) = (0.5, 0.5, -1.0, 0.0)
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            Name "NormalLightingPass"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float4 _LightDirection;  
            float4 _LightColor;      

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_VP, worldPosition);
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal); 
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);

                float3 lightDirection = normalize(_LightDirection.xyz);

                float dotProduct = saturate(dot(normal, lightDirection)); 

                float3 lighting = dotProduct * _LightColor.rgb;

                return float4(lighting, 1.0); 
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
