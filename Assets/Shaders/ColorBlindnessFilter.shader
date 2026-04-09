// URP post-processing shader voor kleurenblindheid filter.
// Schrijft een eigen fullscreen triangle vertex shader zodat Blit.hlsl
// (en de problematische TEXTURE2D_X macro) niet nodig zijn.
// Compatibel met URP 10+ / Unity 2020.3+.

Shader "Custom/ColorBlindnessFilter"
{
    SubShader
    {
        Tags
        {
            "RenderType"     = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }
        ZWrite Off
        ZTest Always
        Cull Off
        Blend Off

        Pass
        {
            Name "ColorBlindnessPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            // Common.hlsl levert: TEXTURE2D, SAMPLER, SAMPLE_TEXTURE2D,
            // GetFullScreenTriangleVertexPosition, GetFullScreenTriangleTexCoord
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            // Core.hlsl levert URP-specifieke macros (UNITY_UV_STARTS_AT_TOP, etc.)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // _BlitTexture en _BlitScaleBias worden via SetGlobalTexture / SetGlobalVector
            // ingesteld door Blitter.BlitCameraTexture() in het RenderPass.
            TEXTURE2D(_BlitTexture);
            SAMPLER(sampler_BlitTexture);
            float4 _BlitScaleBias; // (1,1,0,0) voor een volledige blit

            // Rijen van de 3x3 kleur-transformatiematrix (daltonisering).
            //   newR = dot(originalRGB, _RRow.xyz)
            //   newG = dot(originalRGB, _GRow.xyz)
            //   newB = dot(originalRGB, _BRow.xyz)
            float4 _RRow;
            float4 _GRow;
            float4 _BRow;

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 texcoord   : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                // GetFullScreenTriangleVertexPosition en GetFullScreenTriangleTexCoord
                // zijn gedefinieerd in Common.hlsl. Ze genereren een fullscreen triangle
                // via SV_VertexID (geen vertex buffer nodig) en handelen
                // UNITY_UV_STARTS_AT_TOP (D3D vs OpenGL y-flip) correct af.
                output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
                output.texcoord   = GetFullScreenTriangleTexCoord(input.vertexID)
                                    * _BlitScaleBias.xy + _BlitScaleBias.zw;

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 src = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, input.texcoord);

                float r = dot(src.rgb, _RRow.xyz);
                float g = dot(src.rgb, _GRow.xyz);
                float b = dot(src.rgb, _BRow.xyz);

                // saturate() knipt waarden af naar [0, 1]
                return half4(saturate(float3(r, g, b)), src.a);
            }
            ENDHLSL
        }
    }
}
