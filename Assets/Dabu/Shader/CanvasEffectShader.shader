Shader "Custom/CanvasEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CanvasTex("Displacement", 2D) = "white" {}//噪点图
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
            sampler2D _CanvasTex;
            float _Strength;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 can = tex2D(_CanvasTex, i.uv);
                //uv坐标偏移
                float4 col = tex2D(_MainTex, i.uv);
                return col + can - 1;
            }
            ENDCG
        }
    }
}
