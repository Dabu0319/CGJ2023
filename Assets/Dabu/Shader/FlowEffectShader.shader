Shader "Custom/FlowEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DisplaceTex("DIsplacement", 2D) = "white" {}//噪点图
        _Magnitude("Magnitude", Range(0, 0.1)) = 1//偏移强度
        _Strength("strength", Range(0,1)) = 1//流动速度
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _DisplaceTex;
            float _Magnitude;
            float _Strength;

            fixed4 frag (v2f i) : SV_Target
            {                
                //纹理采样，只要xy;    i.uv - _Time.xy是为了流动感
                float2 disp = tex2D(_DisplaceTex, i.uv - _Time.xy * _Strength).xy;
                //disp的范围是0-1，这步操作让其变成-1 到 1 乘以 参数
                disp = ((disp * 2) - 1) * _Magnitude;
                //uv坐标偏移
                float4 col = tex2D(_MainTex, i.uv + disp);
                return col;
            }
            ENDCG
        }
    }
}