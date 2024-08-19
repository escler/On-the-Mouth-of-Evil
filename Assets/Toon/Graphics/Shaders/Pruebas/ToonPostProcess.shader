Shader "Custom/ToonPostProcess"
{
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);

    float4 _Color;
    float4 _AmbientColor;
    float _Glossiness;
    float4 _SpecularColor;
    float4 _RimColor;
    float _RimAmount;
    float _RimThreshold;

    float4 ToonEffect(float4 color, float2 uv)
    {
        // Simulación de un efecto Toon basado en los valores de color y UV
        float intensity = dot(color.rgb, float3(0.299, 0.587, 0.114)); // Convertir a escala de grises
        float toonStep = step(0.5, intensity); // Umbral para el efecto Toon

        float4 rimColor = saturate(1.0 - dot(normalize(uv - 0.5), normalize(uv - 0.5))); // Sombreado de borde
        rimColor = rimColor * _RimColor * _RimAmount;

        return lerp(color * _Color, rimColor, _RimThreshold);
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" }
        Pass
        {
            Name "ToonPostProcessPass"
            ZTest Always
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                return ToonEffect(color, IN.uv);
            }
            ENDHLSL
        }
    }
}
