Shader "Hidden/Custom/BlackWhiteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ToonSteps ("Toon Steps", Range(1, 9)) = 2
        _RampThreshold ("Ramp Threshold", Range(0.1, 1)) = 0.5
        _RampSmooth ("Ramp Smooth", Range(0, 1)) = 0.1
        
        [HDR]
        _SpecularColor ("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
        _Glossiness ("Glossiness", Float) = 32
        
        [HDR]
        _RimColor ("Rim Color", Color) = (1, 1, 1, 1)
        _RimAmount ("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold ("Rim Threshold", Range(0, 1)) = 0.1
        
        [HDR]
        _AmbientColor ("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Cull Off ZWrite Off ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma target 3.0

            sampler2D _MainTex;
            float _ToonSteps;
            float _RampThreshold;
            float _RampSmooth;
            
            float4 _SpecularColor;
            float _Glossiness;
            
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;
            
            float4 _AmbientColor;

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

            float linearstep(float min, float max, float t)
            {
                return saturate((t - min) / (max - min));
            }

            fixed4 ApplyToonShading(fixed4 color)
            {
                float ramp = smoothstep(_RampThreshold - _RampSmooth * 0.5, _RampThreshold + _RampSmooth * 0.5, color.rgb);
                float interval = 1.0 / _ToonSteps;
                float level = round(ramp * _ToonSteps) / _ToonSteps;
                ramp = interval * linearstep(level - _RampSmooth * 0.5 * interval, level + _RampSmooth * 0.5 * interval, ramp) + level - interval;
                ramp = max(0, ramp);
                return color * ramp;
            }

            fixed4 frag(v2f i) : SV_Target
 {
    fixed4 color = tex2D(_MainTex, i.uv);
    
    fixed4 toonColor = ApplyToonShading(color);
    
    float spec = pow(max(0.0, dot(normalize(i.uv), normalize(float3(0.0, 0.0, 1.0)))), _Glossiness);
    fixed4 specular = _SpecularColor * spec;

    float rim = 1.0 - max(0.0, dot(normalize(i.uv), float3(0.0, 0.0, 1.0)));
    rim = smoothstep(_RimThreshold - 0.5, _RimThreshold + 0.5, rim);
    fixed4 rimColor = lerp(color, _RimColor, rim * _RimAmount); // Mezcla del rim con el color base

    fixed4 ambientColor = _AmbientColor;

    fixed4 finalColor = toonColor + specular + rimColor + ambientColor;
    
    return finalColor;
}

            ENDCG
        }
    }
    FallBack "Diffuse"
}

