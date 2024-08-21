Shader "Custom/CMYKDepthOfField"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CameraDepthTexture ("Depth Texture", 2D) = "white" {}
        _FocusDistance ("Focus Distance", Float) = 1.0
        _FocusArea ("Focus Area", Float) = 0.1
        _OffsetIntensity ("Offset Intensity", Float) = 0.05
        _OffsetMax ("Max Offset", Float) = 2.0
        _Width ("Width", Float) = 1920.0
        _Height ("Height", Float) = 1080.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _CameraDepthTexture;

        float _FocusDistance;
        float _FocusArea;
        float _OffsetIntensity;
        float _OffsetMax;
        float _Width;
        float _Height;

        struct Input
        {
            float2 texcoord;
            float2 uv_MainTex;
        };

        half4 surf (Input i, inout SurfaceOutputStandard o)
        {
            float depth = tex2D(_CameraDepthTexture, i.texcoord).r;

            int isSkybox = 1 - step(LinearEyeDepth(depth), -999.0);

            depth = LinearEyeDepth(depth);
            float intensity = (depth < _FocusDistance + _FocusArea && depth > _FocusDistance - _FocusArea) ? 0.0 : 1.0;
            intensity *= (abs(depth - _FocusDistance) - _FocusArea) * _OffsetIntensity;
            intensity = clamp(ceil(intensity), 0.0, _OffsetMax) * (1.0 - isSkybox);

            half4 originalColor = tex2D(_MainTex, i.texcoord);

            const float w = 1.0 / _Width;
            const float h = 1.0 / _Height;

            half3 colk = originalColor.rgb;
            half resultK = 1.0 - max(colk.r, max(colk.g, colk.b));

            half3 colC = tex2D(_MainTex, i.texcoord + float2(-w * intensity, 0.0)).rgb;
            half resultC = (1.0 - colC.r - resultK) / (1.0 - resultK);

            half3 colM = tex2D(_MainTex, i.texcoord + float2(w * intensity, 0.0)).rgb;
            half resultM = (1.0 - colM.g - resultK) / (1.0 - resultK);

            half3 colY = tex2D(_MainTex, i.texcoord + float2(0.0, h * intensity)).rgb;
            half resultY = (1.0 - colY.b - resultK) / (1.0 - resultK);

            half4 result = half4(
                (1.0 - resultC) * (1.0 - resultK),
                (1.0 - resultM) * (1.0 - resultK),
                (1.0 - resultY) * (1.0 - resultK),
                originalColor.a
            );

            return result;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
