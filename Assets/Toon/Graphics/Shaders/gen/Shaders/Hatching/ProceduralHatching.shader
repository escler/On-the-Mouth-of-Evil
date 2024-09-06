Shader "Custom/ProceduralHatching"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Rotation ("Rotation", Range(0, 360)) = 45
        _Tiling ("Tiling", Vector) = (1,1,0,0)
        _Offset ("Offset", Vector) = (0,0,0,0)
        _Thickness ("Thickness", Range(0, 1)) = 0.5
        _Height ("Height", Range(0, 1)) = 1.0
        _AOMap ("Ambient Occlusion", 2D) = "white" {}
        _HSVPower("HSV Power", float) = 2.0
        _HSVChannel("HSV Channel", int) = 0
    }

    SubShader
    {
        Tags{"RenderType"="Opaque" "Queue"="Geometry"}
        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode"="UniversalForward"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Rotation;
                float4 _Tiling;
                float4 _Offset;
                float _Thickness;
                float _Height;
                float _HSVPower;
                int _HSVChannel;
            CBUFFER_END

            // Texturas
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_AOMap);
            SAMPLER(sampler_AOMap);

            // Función para convertir de RGB a HSV
            float3 RGBToHSV(float3 rgb)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = rgb.g < rgb.b ? float4(rgb.bg, K.wz) : float4(rgb.gb, K.xy);
                float4 q = rgb.r < p.x ? float4(p.xyw, rgb.r) : float4(rgb.r, p.yzx);

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv.xy;

                // Rotación y transformación UV
                float angle = _Rotation * 3.14159265359 / 180.0;
                float s = sin(angle);
                float c = cos(angle);
                float2x2 rotationMatrix = float2x2(c, -s, s, c);
                uv = mul(rotationMatrix, uv - 0.5) + 0.5;
                uv = uv * _Tiling.xy + _Offset.xy;

                // Samplear texturas
                float4 baseColor = _BaseColor * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                float ao = SAMPLE_TEXTURE2D(_AOMap, sampler_AOMap, uv).r;  // Usar canal rojo de la AO

                // Convertir baseColor de RGB a HSV
                float3 hsv = RGBToHSV(baseColor.rgb);

                // Aplicar la función pow al canal seleccionado
                float hsvChannel = hsv[_HSVChannel];
                float processedChannel = pow(hsvChannel, _HSVPower);

                // Generar patrón de hatching basado en la AO y canal procesado
                float2 hatching = frac(uv);
                float hatchingPattern = step(hatching.x, _Thickness) * step(hatching.y, _Height);

                // Aplicar el patrón solo en las sombras (basado en AO)
                float4 color = lerp(baseColor, float4(0, 0, 0, 1), ao * processedChannel * hatchingPattern);

                return color;
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
