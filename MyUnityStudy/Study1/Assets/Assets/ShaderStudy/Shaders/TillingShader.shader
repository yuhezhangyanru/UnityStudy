//function：重复贴图的shader,detail贴图是纹理
//两张图：detail展示细节，Maintex是大的贴图
//注意： 重复的重点_DetailTex_ST，和函数TRANSFORM_TEX
Shader "MyShader/TillingShader"
{
    Properties
    {
        _Color("Base Color",Color)=(1,1,1,1)//改变贴图颜色 
        _MainTex("Base(RGB)",2D)="white"{} //贴图
        _ColorU("_ColorU",float)=1.0 //调整颜色
        _ColorV("_ColorV",float)=1.0
        _DetailTex("DetailTex",2D)="white"{}//纹理
        _DetailU("_DetailU",float)=1.0  //重复的次数
        _DetailV("_DetailV",float)=1.0
    }

    SubShader
    {
       tags{"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
       Blend SrcAlpha OneMinusSrcAlpha

       Pass
       { 

          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
          #include "UnityCG.cginc"

          float4 _Color;
          float _Alpha;
          sampler2D _MainTex; 
          sampler2D _DetailTex;
          float _DetailU;
          float _DetailV;
          float _ColorU;
          float _ColorV;

          struct v2f
          {
              float4 pos:POSITION;
              float2 uv:TEXCOORD0; 
          };
 
          float4 _DetailTex_ST;

          v2f vert(appdata_base v)
          {
              v2f o;
              o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
              o.uv=v.texcoord.xy;
             // o.uv=TRANSFORM_TEX(v.texcoord,_DetailTex);
              return o;
          }

          half4 frag(v2f i):COLOR
          { 
              half4 color= tex2D(_MainTex,i.uv*float2(_ColorU,_ColorV)) *_Color;
              half4 d = tex2D(_DetailTex,i.uv*float2(_DetailU,_DetailV));
              color=color*d;
              return color;
          }

          ENDCG
        }
    }

}
