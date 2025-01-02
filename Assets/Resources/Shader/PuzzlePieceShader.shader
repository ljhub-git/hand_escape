Shader "Custom/PuzzlePieceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // ���� �ؽ�ó (����)
        _Color ("Color", Color) = (1,1,1,1)   // ����
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
            o.Albedo = c.rgb; // �ؽ�ó �÷�
            o.Alpha = c.a;    // ���İ� (����)
        }
        ENDCG
    }
    FallBack "Diffuse"
}
