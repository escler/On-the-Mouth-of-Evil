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
        _customColor ("Custom Color", Color) = (1, 1, 1, 1)
        
        [HDR]
        _AmbientColor ("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
    }

    SubShader
    {
        Tags { "LightMode" = "ForwardBase" "PassFlags" = "OnlyDirectional" "RenderPipeline" = "UniversalRenderPipeline" }

        LOD 100

        Pass
        {
            Cull Off ZWrite Off ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
			#include "AutoLight.cginc"
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

            struct Attributes
            {
                float4 positionOS : POSITION;
                half3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                half3 normalWS : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            // Use unity_WorldToObject matrix which is defined in UnityCG.cginc
            half3 TransformNormalToWorldSpace(half3 normalOS)
            {
                // Transform object space normal to world space normal using unity_WorldToObject
                half3x3 normalMatrix = (half3x3)unity_WorldToObject;
                return normalize(mul(normalMatrix, normalOS));
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = UnityObjectToClipPos(IN.positionOS);
                OUT.normalWS = TransformNormalToWorldSpace(IN.normal);
                return OUT;
            }

            // half4 frag(Varyings IN) : SV_Target
            // {
            //     half4 color = (0.5, 0, 0, 1);
            //     color.rgb = IN.normalWS * 0.5 + 0.5;
                
            //     return color;
            // }

            

            

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

            fixed4 frag(Varyings IN) : SV_Target
            {                
                half4 color = (0.5, 0, 0, 1);
                color.rgb = IN.normalWS * 0.5 + 0.5;
                fixed4 toonColor = ApplyToonShading(color);

                // Direcciones de la luz y la c�mara en espacio mundial
                float3 lightDir = normalize(float3(0.0, 0.0, 1.0)); // Direcci�n de la luz (puedes ajustar esto seg�n tu escena)
                float3 viewDir = normalize(IN.viewDir); // Direcci�n de la c�mara (dirigido hacia el v�rtice)

                // C�lculo de la reflexi�n especular
                float3 reflectDir = reflect(-lightDir, normalize(IN.normalWS)); // Direcci�n de la reflexi�n
                float spec = pow(max(0.0, dot(viewDir, reflectDir)), _Glossiness);
                fixed4 specular = _SpecularColor * spec;

                // Rim Lighting (iluminaci�n de borde)
                float rim = 1.0 - max(0.0, dot(normalize(IN.normalWS), viewDir));
                rim = smoothstep(_RimThreshold - 0.5, _RimThreshold + 0.5, rim);
                fixed4 rimColor = lerp(color, _RimColor, rim * _RimAmount);

                // Iluminaci�n ambiental
                fixed4 ambientColor = _AmbientColor;

                // Color final
                fixed4 finalColor = toonColor + specular + rimColor + ambientColor;
                return finalColor;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
