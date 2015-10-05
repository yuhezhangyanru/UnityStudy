//function：实现序列帧动画的shader
Shader "MyShader/UVAnim_Shader"
{
    Properties
    {
        _Color("Base Color",Color)=(1,1,1,1)//改变贴图颜色 
        _MainTex("Base(RGB)",2D)="white"{} 
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

          struct v2f
          {
              float4 pos:POSITION;
              float2 uv:TEXCOORD0; 
          };

          float2 moveUV(float2 vertUV)
          {
              float textureNum = 12.0;
              float timePerFrame = 50;

              float index = frac(_Time.x/textureNum*timePerFrame);
              float2 uvScale = float2(1/textureNum,1);
 

              if(index<=uvScale.x)
              {
                  return vertUV*uvScale;
              }
              else if(index <= 2*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0);
              }
              else if(index <= 3*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*2;
              }
              else if(index <= 4*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*3;
              }
              else if(index <= 5*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*4;
              }
              else if(index <= 6*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*5;
              }
              else if(index <= 7*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*6;
              }
              else if(index <= 8*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*7;
              }
              else if(index <= 9*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*8;
              }
              else if(index <= 10*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*9;
              }
              else if(index <= 11*uvScale.x)
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*10;
              } 
              else 
              {
                  return vertUV*uvScale+float2(uvScale.x,0.0)*11;
              }
          }

          v2f vert(appdata_base v)
          {
              v2f o;
              o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
              o.uv= moveUV(v.texcoord.xy);

              return o;
          }

          half4 frag(v2f i):COLOR
          { 
              half4 color= tex2D(_MainTex,i.uv) *_Color;
 
              return color;
          }

          ENDCG
        }
    }

}
