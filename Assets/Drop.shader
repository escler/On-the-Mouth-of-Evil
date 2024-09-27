Shader "Custom/Drop"
{
    Properties
    {
	    
	    [MainColor] _BaseColor("BaseColor", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("BaseMap", 2D) = "white" {}
        _ShadowColor("Color", Color) = (0, 0, 0, 1)
        _LightDir ("Light Direction", Vector) = (0, 1, 0, 0)
        _PlaneY ("Plane Height", Float) = 0	
		_ShadowFalloff ("ShadowFalloff ",Range (0,1)) = 0.5
 
    }
    SubShader
    {
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
	  HLSLINCLUDE
		  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
	         TEXTURE2D(_BaseMap);
             SAMPLER(sampler_BaseMap);
 
           CBUFFER_START(UnityPerMaterial)
		
            float4 _BaseMap_ST;
            half4 _BaseColor;
		    float4 _ShadowColor;
            half3 _LightDir;
			half _PlaneY;
			half _ShadowFalloff;
 
            CBUFFER_END
      ENDHLSL
 
		pass 
		{
			Name "Forward"
       
            Tags {
		  
			"LightMode"="UniversalForward" 
			}
		  HLSLPROGRAM
		    #pragma vertex vert
            #pragma fragment frag
				  #pragma multi_compile_instancing
			  struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				 UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;   
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
			   v2f vert (appdata v)
            {
                v2f o;
				  UNITY_SETUP_INSTANCE_ID(v);
				  UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = TransformObjectToHClip(v.vertex.xyz);;
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
           
                return o;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                 UNITY_SETUP_INSTANCE_ID(i);
             return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * _BaseColor;
            }
	
			ENDHLSL
		}
        Pass
        {
 
		Name "Droping"
		Tags{
	   
	    	"LightMode" = "SRPDefaultUnlit"
		}
		  Stencil
    {
        Ref 1  
        Comp NotEqual   
        Pass Replace  
        Fail Keep    
		
    }
		Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
		
      
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;   
                float4 vertex : SV_POSITION;
		    	float4 color : COLOR;
            };
		
 
        float3 PlanarShadowPos(float3 posWS)
        {
          float3 L = normalize(_LightDir);
          float3 N = float3(0, 1, 0);
 
          float d1 = dot(L, N);
          float d2 = posWS.y - _PlaneY;
 
          float3 offsetByNormal = N * 0.001;
          return posWS - L * (d2 / d1) + offsetByNormal;
 
          }
 
 
            v2f vert (appdata v)
            {
                v2f o;
                float3 posWS = TransformObjectToWorld(v.vertex.xyz);
                posWS = PlanarShadowPos(posWS);
                o.vertex = TransformWorldToHClip(posWS);
                o.uv =v.uv;         
		
		    	float3 center = float3(unity_ObjectToWorld[0].w,  _PlaneY, unity_ObjectToWorld[2].w);
				float falloff = 1 - saturate(distance(posWS, center) * _ShadowFalloff);
 
			   	o.color = _ShadowColor;
				o.color.a *=falloff;
                return o;
            }
 
 
 
            half4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDHLSL
        }
    }
}