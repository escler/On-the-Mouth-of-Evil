Shader "Custom/BowlerVision"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CameraDepthTexture ("Depth Texture", 2D) = "white" {}
        _CameraDepthNormalsTexture ("Depth Normals Texture", 2D) = "white" {}
        _OutlineThickness ("Outline Thickness", Float) = 1.0
        _DepthSensitivity ("Depth Sensitivity", Float) = 1.0
        _NormalsSensitivity ("Normals Sensitivity", Float) = 1.0
        _Width ("Width", Float) = 1920.0
        _Height ("Height", Float) = 1080.0
        _BackgroundOutlineColor ("Background Outline Color", Color) = (0,0,0,1)
        _ForegroundOutlineColor ("Foreground Outline Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _CameraDepthTexture;
        sampler2D _CameraDepthNormalsTexture;

        float _OutlineThickness;
        float _DepthSensitivity;
        float _NormalsSensitivity;
        float _Width;
        float _Height;

        float4 _BackgroundOutlineColor;
        float4 _ForegroundOutlineColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        half4 surf (Input i, inout SurfaceOutputStandard o)
        {
            float halfScaleFloor = floor(_OutlineThickness * 0.5);
            float halfScaleCeil = ceil(_OutlineThickness * 0.5);
            float2 Texel = 1.0 / float2(_Width, _Height);

            float2 uvSamples[4];
            uvSamples[0] = i.uv_MainTex - float2(Texel.x, Texel.y) * halfScaleFloor;
            uvSamples[1] = i.uv_MainTex + float2(Texel.x, Texel.y) * halfScaleCeil;
            uvSamples[2] = i.uv_MainTex + float2(Texel.x * halfScaleCeil, -Texel.y * halfScaleFloor);
            uvSamples[3] = i.uv_MainTex + float2(-Texel.x * halfScaleFloor, Texel.y * halfScaleCeil);

            float depthSamples[4];
            float3 normalSamples[4];
            for (int j = 0; j < 4; j++)
            {
                depthSamples[j] = tex2D(_CameraDepthTexture, uvSamples[j]).r;
                normalSamples[j] = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture, uvSamples[j]));
            }

            half4 original = tex2D(_MainTex, i.uv_MainTex);
            half4 OutlineColor = (_BackgroundOutlineColor * step(0.75, original.a)) + (_ForegroundOutlineColor * step(0.75, 1.0 - original.a));

            float depthFiniteDifference0 = depthSamples[1] - depthSamples[0];
            float depthFiniteDifference1 = depthSamples[3] - depthSamples[2];
            half edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100.0;
            half depthThreshold = (1.0 / _DepthSensitivity) * depthSamples[0];
            edgeDepth = edgeDepth > depthThreshold ? 1.0 : 0.0;

            half3 normalFiniteDifference0 = normalSamples[1] - normalSamples[0];
            half3 normalFiniteDifference1 = normalSamples[3] - normalSamples[2];
            half edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
            edgeNormal = edgeNormal > (1.0 / _NormalsSensitivity) ? 1.0 : 0.0;

            half edge = max(edgeDepth, edgeNormal);

            return half4((1.0 - edge) * original.rgb + edge * lerp(original.rgb, OutlineColor.rgb, OutlineColor.a), original.a);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
