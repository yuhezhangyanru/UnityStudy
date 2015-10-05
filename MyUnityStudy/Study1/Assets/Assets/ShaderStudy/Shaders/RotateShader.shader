//function：旋转的shader，使贴图围绕中心旋转
//注：适用于透明贴图，且贴图的WrapMode=Clamp
Shader "MyShader/RotateShader"
{
    Properties
    {
        _Color("Base Color",Color)=(1,1,1,1)//改变贴图颜色
        _Alpha("Alpha",Range(0,1)) = 1//改变透明度
        _MainTex("Base(RGB)",2D)="white"{}
        _RSpeed("RotateSpeed",Range(1,100)) =30
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
          float _RSpeed;

          struct v2f
          {
              float4 pos:POSITION;
              float4 uv:TEXCOORD0; 
          };

          v2f vert(appdata_base v)
          {
              v2f o;
              o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
              o.uv=v.texcoord; 
              return o;
          }

          half4 frag(v2f i):COLOR
          {
              //默认矩阵相乘旋转的中心点是0,0，减去0.5是为了新的nv中心点正好在旋转原点
              float2 uv = i.uv.xy-float2(0.5,0.5);

              //开始旋转 _Time.x是一个速度，.y.z.w都是不同的速度，根据需要选择
              //float2 rotate = float2(cos(_RSpeed *_Time.x),sin(_RSpeed*_Time.x));
              uv = float2(uv.x*cos(_RSpeed *_Time.x)-uv.y*sin(_RSpeed*_Time.x),
                  uv.x*sin(_RSpeed*_Time.x)+uv.y*cos(_RSpeed *_Time.x));
              uv +=float2(0.5,0.5);

              half4 color= tex2D(_MainTex,uv) *_Color;

              color.a *= _Alpha;
              return color;
          }

          ENDCG
        }
    }

}
