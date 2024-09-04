Shader "Custom/AO"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {} // Textura para modificar el AO
        _AOIntensity("AO Intensity", Range(0, 1)) = 1.0 // Intensidad del AO
        _Scale("Texture Scale", Vector) = (1, 1, 1, 1) // Escala de la textura
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            Name "TextureBasedAO"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Textura y parámetros del shader
            sampler2D _MainTex;
            float _AOIntensity;
            float4 _Scale;

            // Estructura de datos de entrada del vértice
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Estructura de datos de salida del vértice
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            // Función de vértice
            Varyings vert(Attributes v)
            {
                Varyings o;
                // Transformación del vértice a espacio de pantalla
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv * _Scale.xy;
                o.worldPos = TransformObjectToWorld(v.positionOS).xyz;
                return o;
            }

            // Función de fragmento
            half4 frag(Varyings i) : SV_Target
            {
                // Calcula la distancia del fragmento a la cámara
                float3 viewDir = _WorldSpaceCameraPos - i.worldPos;
                float distanceToCamera = length(viewDir);

                // Calcula el AO básico basado en la distancia (inverso)
                float ao = 1.0 - saturate(distanceToCamera * _AOIntensity);

                // Aplica la textura para modificar el AO
                half4 texColor = tex2D(_MainTex, i.uv);

                // Multiplica el AO básico por el valor de la textura (asumiendo textura en escala de grises)
                ao *= texColor.r;

                // Retorna el resultado
                return half4(ao, ao, ao, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack "Diffuse"
}
