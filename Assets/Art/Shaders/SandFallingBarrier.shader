Shader "Custom/SandFallingBarrier"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _HeightMap ("Height Map", 2D) = "white" {}
        _TimeScale ("Time Scale", Float) = 1.0
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            TEXTURE2D(_HeightMap);
            SAMPLER(sampler_HeightMap);

            float _TimeScale;
            float _Cutoff;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.position = TransformObjectToHClip(IN.vertex);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample the main texture
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // Sample the noise texture and animate it over time
                float2 uv = IN.uv;
                uv.y += _TimeScale * _TimeParameters.y; // _TimeParameters.y is a built-in variable in Unity for time in seconds
                float noiseValue = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, uv).r;

                // Sample the height map for accumulation
                float heightValue = SAMPLE_TEXTURE2D(_HeightMap, sampler_HeightMap, IN.uv).r;

                // Blend the noise and height map to simulate sand accumulation
                float blendValue = lerp(noiseValue, heightValue, _Cutoff);

                // Apply the blend to alpha
                float alpha = saturate(blendValue - _Cutoff);

                // Output the final color
                return float4(mainTex.rgb, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
