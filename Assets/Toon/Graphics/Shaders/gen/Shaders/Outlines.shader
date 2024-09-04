Shader "Custom/Outlines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _outlineLen ("Outline Length", float) = 0.2
        _smoothFactor ("Smooth Factor", float) = 0.02 
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float outlineAmount : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            float _outlineLen;
            float _smoothFactor; 
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
                float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                normal.z = -0.5;
                pos = pos + float4(normalize(normal), 0) * _outlineLen;
                o.pos = mul(UNITY_MATRIX_P, pos);
                
                o.outlineAmount = length(normalize(normal)) * _outlineLen;

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float edgeSmooth = smoothstep(_outlineLen - _smoothFactor, _outlineLen + _smoothFactor, i.outlineAmount);
                return half4(0, 0, 0, edgeSmooth);
            }
            ENDHLSL
        }
    }
}
