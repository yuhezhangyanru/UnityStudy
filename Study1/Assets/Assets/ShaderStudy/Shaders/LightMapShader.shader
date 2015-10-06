//function：LightMapShader
Shader "MyShader/LightMapShader"
{
    Properties
    {
        _Color("Base Color",Color)=(1,1,1,1)//改变贴图颜色 
        _MainTex("Base(RGB)",2D)="white"{} 
        _LightMap("LightMap",2D)="white"{}
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
          sampler2D _MainTex; 
          sampler2D _LightMap; 

          struct v2f
          {
              float4 pos:POSITION;
              float4 uv:TEXCOORD; 
              float2 uvLM:TEXCOORD1;
          };

          v2f vert(appdata_full v)
          {
              v2f o;
              o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
              o.uv=v.texcoord; 
              o.uvLM=v.texcoord1.xy;
              return o;
          }

          half4 frag(v2f i):COLOR
          { 
              half4 color= tex2D(_MainTex,i.uv.xy) *_Color;
               half3 l =2.0*tex2D(_LightMap,i.uvLM).rgb;

            //注释的代码是加强光照阴影的效果版，效果不及当前
          //    half4 lm=tex2D(_LightMap,i.uvLM);
           //   half3 l =8.0*lm.a *lm.rgb;
              color.rgb*=l;
              return color;
          }

          ENDCG
        }
    }

}
