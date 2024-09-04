Shader "Hidden/Custom/ColorDispersionShadowShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorDispersionStrength ("Dispersion Strength", Range(0, 1)) = 0.1
        _ColorDispersionU ("Dispersion U", Range(-1, 1)) = 0.5
        _ColorDispersionV ("Dispersion V", Range(-1, 1)) = 0.5
        _BlackWhiteThreshold ("Black White Threshold", Range(0, 1)) = 0.5
        _BlackWhiteWidth ("Black White Width", Range(0, 1)) = 0.1
        _BlackWhiteBlackColor ("Black Color", Color) = (0, 0, 0, 1)
        _EnableBlackWhite ("Enable Black White", Float) = 0
        _Transition ("Transition", Range(0, 1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "ShadowCaster" }
        LOD 100

        Pass
        {
            Name "ShadowPass"
            Tags { "LightMode" = "ShadowCaster" }
            ZWrite On
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _ColorDispersionStrength;
            float _ColorDispersionU;
            float _ColorDispersionV;
            float _BlackWhiteThreshold;
            float _BlackWhiteWidth;
            float4 _BlackWhiteBlackColor;
            float _EnableBlackWhite;
            float _Transition;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 deltaUv = float2(_ColorDispersionStrength * _ColorDispersionU, _ColorDispersionStrength * _ColorDispersionV);

                float4 colorDispersionResult;
                float4 tempScreenColor;

                tempScreenColor = tex2D(_MainTex, i.uv + deltaUv);
                colorDispersionResult.r = tempScreenColor.r;

                tempScreenColor = tex2D(_MainTex, i.uv);
                colorDispersionResult.g = tempScreenColor.g;

                tempScreenColor = tex2D(_MainTex, i.uv - deltaUv);
                colorDispersionResult.b = tempScreenColor.b;

                colorDispersionResult.a = 1.0;

                float4 originalColor = tex2D(_MainTex, i.uv);

                if (_EnableBlackWhite > 0.5)
                {
                    float luminosity = dot(colorDispersionResult.rgb, float3(0.299, 0.587, 0.114));
                    float smoothstepResult = smoothstep(_BlackWhiteThreshold, _BlackWhiteThreshold + _BlackWhiteWidth, luminosity);

                    if (smoothstepResult >= 0.5)
                    {
                        colorDispersionResult.rgb = _BlackWhiteBlackColor.rgb;
                        colorDispersionResult.a = originalColor.a; 
                    }
                    else
                    {
                        colorDispersionResult = originalColor;
                    }
                }
                else
                {
                    colorDispersionResult = originalColor;
                }

                return colorDispersionResult;
            }

            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
