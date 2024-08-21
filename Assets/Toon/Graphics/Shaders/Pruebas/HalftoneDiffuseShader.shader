Shader "Custom/HalftoneDiffuseShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _DiffusePattern ("Halftone Pattern", 2D) = "white" {}
        _ShadowSize ("Shadow Size", Range(0,1)) = 0.5
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ShadingMultiplier ("Shading Multiplier", Range(0,1)) = 0.5
        _CircleDensity ("Circle Density", Range(0,1)) = 0.5
        _Softness ("Softness", Range(0,1)) = 0.5
        _Rotation ("Rotation", Range(0,1)) = 0.5
        _LitThreshold ("Lit Threshold", Range(0,1)) = 0.5
        _FallofThreshold ("Fallof Threshold", Range(0,1)) = 0.5
        //_ScreenSpace bool
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _DiffusePattern;
        half _ShadowSize;

        struct Input
        {
            float2 uv_MainTex;
            float2 diffusePatternUV;
            float3 worldPos;
            float3 worldNormal;
        };

        void HatchingDiffuse(float3 normal, half3 lightDir, half3 lightColor, inout half atten, inout half4 color)
        {
            half NdotL = dot(normal, lightDir);
            NdotL = smoothstep(-_ShadowSize, _ShadowSize, NdotL);

            atten = atten > 0.91 ? 1 : atten;
            NdotL = min(NdotL, atten);

            #ifdef SCREEN_SPACE_UVS
            float2 uv = TRANSFORM_TEX(normal, _DiffusePattern) * 1000;
            float val = max(tex2Dlod(_DiffusePattern, float4(uv, 0, 0)).r, 0.001);
            #else
            float val = tex2D(_DiffusePattern, normal.xy).r;
            #endif

            val = step(1-val, NdotL) - (step(NdotL, 0.001) / 2 * step(1-val, NdotL));

            atten = val;
            color.rgb = color.rgb * lightColor * atten;
            color.a = color.a;
        }

        void surf(Input i, inout SurfaceOutputStandard o)
        {
            half4 c = tex2D(_MainTex, i.uv_MainTex);
            o.Albedo = c.rgb;

            half3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
            half3 lightColor = _LightColor0.rgb;

            half atten = 1.0;
            half4 color = half4(o.Albedo, o.Alpha);
            HatchingDiffuse(i.worldNormal, lightDir, lightColor, atten, color);

            o.Albedo = color.rgb;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
