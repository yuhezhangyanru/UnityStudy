//我的第一个shader
Shader "MyShader/Sample"
{
    Properties
    {
        _Color("Base Color",Color)=(1,1,1,1)//改变贴图颜色
        _Alpha("Alpha",Range(0,1)) = 1//改变透明度
        _MainTex("Base(RGB)",2D)="white"{}
    }

    SubShader
    {
       tags{"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}

       Blend SrcAlpha OneMinusSrcAlpha

       Pass
       {
          Name "Sample"
          Cull off

          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
          #include "UnityCG.cginc"

          float4 _Color;
          float _Alpha;
          sampler2D _MainTex;

          struct v2f
          {
              float4 pos:POSITION;
              float4 uv:TEXCOORD0;
              float4 col:COLOR;
          };

          v2f vert(appdata_base v)
          {
              v2f o;
              o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
              o.uv=v.texcoord;
              o.col.xyz =v.normal*0.5+0.5;
              o.col.w =1.0;
              return o;
          }

          half4 frag(v2f i):COLOR
          {

              half4 color= tex2D(_MainTex,i.uv.xy) *_Color * i.col;

              //换一种方式   :这语句显示的是法线的值i.col,
              //half4 color=i.col;

              color.a = _Alpha;
              return color;
          }

          ENDCG
        }
    }

}
