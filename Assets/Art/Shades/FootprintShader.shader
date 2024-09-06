Shader "Custom/FootprintShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FootprintTex ("Footprint (RGB)", 2D) = "black" {}
        _FootprintDuration ("Footprint Duration", Float) = 5.0
        _TimeSinceStart ("Time Since Start", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Common.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Texture2D _MainTex;
            Texture2D _FootprintTex;
            float _FootprintDuration;
            float _TimeSinceStart;
            SamplerState samplerState;

            Varyings Vert (Attributes v)
            {
                Varyings o;
                float4 worldPosition = mul(unity_ObjectToWorld, v.positionOS);
                o.positionCS = mul(UNITY_MATRIX_VP, worldPosition);
                o.uv = v.uv;
                return o;
            }

            float4 Frag (Varyings i) : SV_Target
            {
                // Fetch the base texture color
                float4 baseColor = _MainTex.Sample(samplerState, i.uv);
                
                // Fetch the footprint texture color
                float4 footprintColor = _FootprintTex.Sample(samplerState, i.uv);
                
                // Implement logic for footprint duration
                float alpha = max(0, 1.0 - _TimeSinceStart / _FootprintDuration);
                
                // Combine base color and footprint color based on alpha
                return lerp(baseColor, footprintColor, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
