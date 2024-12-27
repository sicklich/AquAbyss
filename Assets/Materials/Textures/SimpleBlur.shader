Shader "Custom/SimpleBlurGlass"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Blur("Blur Amount", Range(0, 10)) = 1 // 放宽模糊强度范围
        _Distortion("Distortion", Range(0, 5)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }

        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 grabUv : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float _Blur;
            float _Distortion;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grabUv = UNITY_PROJ_COORD(ComputeGrabScreenPos(o.vertex));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = 0;
                float2 projUv = i.grabUv.xy / i.grabUv.w;

                // 高斯模糊：采样点数增加
                const int numSamples = 16; // 增加采样点数量
                float blurStrength = _Blur * 0.05; // 增强模糊范围
                float2 offsets[numSamples] = {
                    float2(-1, -1), float2(-1,  0), float2(-1,  1),
                    float2( 0, -1), float2( 0,  1),
                    float2( 1, -1), float2( 1,  0), float2( 1,  1),
                    float2(-2, -2), float2(-2,  0), float2(-2,  2),
                    float2( 0, -2), float2( 0,  2),
                    float2( 2, -2), float2( 2,  0), float2( 2,  2)
                };

                // 累加采样
                for (int j = 0; j < numSamples; j++)
                {
                    float2 sampleUv = projUv + offsets[j] * blurStrength;
                    col += tex2D(_GrabTexture, sampleUv);
                }

                col /= numSamples; // 平均化采样结果

                // 添加轻微失真效果
                col.xy += _Distortion * 0.01;

                return col;
            }
            ENDCG
        }
    }
}
