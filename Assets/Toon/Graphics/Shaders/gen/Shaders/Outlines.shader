Shader "Custom/Outlines"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_outlineLen("outlineLen",float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100


        Pass
        {
			Tags{ "LightMode" = "LightweightForward" }
			Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };
			CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
            float4 _MainTex_ST;
			CBUFFER_END
            v2f vert (appdata v)
            {
                v2f o;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				o.pos = vertexInput.positionCS;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // sample the texture
				half4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDHLSL
        }

		Pass
		{
			Tags{ "LightMode" = "SRPDefaultUnlit" }
			Cull Front
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/core.hlsl"
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal :NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			CBUFFER_START(UnityPerMaterial)
			float _outlineLen;
			CBUFFER_END
			v2f vert(appdata v)
			{
				v2f o;
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				normal.z = -0.5;
				pos = pos + float4(normalize(normal), 0) * _outlineLen;
				o.pos = mul(UNITY_MATRIX_P, pos);


				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				return half4(0,0,0,1);
			}
		ENDHLSL
	}
    }
}
