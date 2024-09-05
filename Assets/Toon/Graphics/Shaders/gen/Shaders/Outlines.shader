Shader "Custom/Outlines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Header(Outline)]
        [ToggleUI] _UseSmoothNormal("Use Smooth Normal", Float) = 0
        _OutlineWidth("Outline Width", Float) = 1
        _OutlineWidthParams("Outline Width Params", Vector) = (0,1,0,1)
        _OutlineZOffset("Outline Z Offset", Float) = 0
        _ScreenOffset("Screen Offset", Vector) = (0,0,0,0)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineColor2("Outline Color 2", Color) = (1,0,0,1)
        _OutlineColor3("Outline Color 3", Color) = (0,1,0,1)
        _EdgeSmoothFactor("Edge Smooth Factor", Float) = 0.1
        _RampThreshold1("Ramp Threshold 1", Float) = 0.33
        _RampThreshold2("Ramp Threshold 2", Float) = 0.66
        [Header(Line Pattern)]
        _LinePatternTex ("Line Pattern Texture", 2D) = "white" // Textura para patrón de línea discontinua
        _UseLinePattern ("Use Line Pattern", Float) = 0 // Habilitar patrón de línea discontinua
        _PatternScale ("Pattern Scale", Float) = 1 // Escala del patrón de línea
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Front
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float outlineAmount : TEXCOORD0;
                float depth : TEXCOORD1;
                float2 uv : TEXCOORD2; // Para la textura de patrón
            };

            CBUFFER_START(UnityPerMaterial)
            float _UseSmoothNormal;
            float _OutlineWidth;
            float4 _OutlineWidthParams;
            float _OutlineZOffset;
            float4 _ScreenOffset;
            half4 _OutlineColor;
            half4 _OutlineColor2;
            half4 _OutlineColor3;
            float _EdgeSmoothFactor;
            float _RampThreshold1;
            float _RampThreshold2;
            float _UseLinePattern;
            float _PatternScale;
            CBUFFER_END

            TEXTURE2D(_LinePatternTex);
            SAMPLER(sampler_LinePatternTex);

            v2f vert(appdata v)
            {
                v2f o;
                float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
                float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);

                if (_UseSmoothNormal > 0.5)
                {
                    normal = normalize(normal);
                }

                normal.z += _OutlineZOffset;
                pos += float4(normal, 0) * _OutlineWidth;
                o.pos = mul(UNITY_MATRIX_P, pos);

                o.pos.xy += _ScreenOffset.xy * o.pos.w;
                o.depth = o.pos.z / o.pos.w;

                o.outlineAmount = length(normal) * _OutlineWidth;
                o.uv = o.pos.xy * _PatternScale;

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float edgeSmooth = smoothstep(_OutlineWidth - _EdgeSmoothFactor, _OutlineWidth + _EdgeSmoothFactor, i.outlineAmount);

                // Crear un gradiente basado en el valor de edgeSmooth
                float gradientValue1 = saturate((edgeSmooth - _RampThreshold1) / (_RampThreshold2 - _RampThreshold1));
                float gradientValue2 = saturate((edgeSmooth - _RampThreshold2) / (1.0 - _RampThreshold2));

                // Aplicar colores en base a los gradientes
                half4 finalColor = _OutlineColor * (1.0 - gradientValue1) + _OutlineColor2 * gradientValue1;
                finalColor = lerp(finalColor, _OutlineColor3, gradientValue2);

                // Aplicar patrón de línea discontinua si está habilitado
                if (_UseLinePattern > 0.5)
                {
                    float pattern = SAMPLE_TEXTURE2D(_LinePatternTex, sampler_LinePatternTex, i.uv).r;
                    finalColor *= pattern;
                }

                finalColor *= edgeSmooth; // Aplicar suavidad del borde

                return finalColor;
            }
            ENDHLSL
        }
    }
}
