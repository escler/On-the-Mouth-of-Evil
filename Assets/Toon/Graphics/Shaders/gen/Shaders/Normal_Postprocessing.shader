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

            // Propiedades del shader que pueden ser ajustadas desde el script de Render Feature
            float4 _LightDirection;  // Dirección de la luz, ajustable desde el script
            float4 _LightColor;      // Color de la luz

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
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal); // Calcula las normales en el espacio del mundo
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Normaliza la normal en el espacio del mundo
                float3 normal = normalize(i.worldNormal);

                // Utiliza la dirección de la luz proporcionada por la propiedad _LightDirection
                float3 lightDirection = normalize(_LightDirection.xyz);

                // Calcular el producto punto entre la normal y la dirección de la luz
                float dotProduct = saturate(dot(normal, lightDirection)); // saturate clampa el valor entre 0 y 1

                // Convertir el valor del producto punto en un valor de intensidad y multiplicar por el color de la luz
                float3 lighting = dotProduct * _LightColor.rgb;

                // Devuelve un color basado en la intensidad de la luz y el color de la luz
                return float4(lighting, 1.0); // Sombreado basado en la luz y color
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
