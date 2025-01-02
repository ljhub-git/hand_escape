Shader "Custom/PuzzlePieceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 메인 텍스처 (정면)
        _Color ("Color", Color) = (1,1,1,1)   // 색상
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb; // 텍스처 컬러
            o.Alpha = c.a;    // 알파값 (투명도)
        }
        ENDCG
    }
    FallBack "Diffuse"
}
