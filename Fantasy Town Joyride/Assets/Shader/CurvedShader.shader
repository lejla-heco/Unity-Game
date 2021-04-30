Shader "Custom/Curved" {
   Properties {
      _Color ("Main Color", Color) = (1, 1, 1, 1)
      _MainTex ("Base (RGB)", 2D) = "white" {}
      _QOffset ("Offset", Vector) = (0,0,0,0)
      _Dist ("Distance", Float) = 100.0
   }
   SubShader {
      Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
      Pass
      {
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0
         #include "Lighting.cginc"
         #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
         #include "AutoLight.cginc"
         
            sampler2D _MainTex;
         float4 _Color;
         float4 _QOffset;
         float _Dist;
         
         struct v2f {
             float4 pos : SV_POSITION;
             float4 uv : TEXCOORD0;
            float4 diff: COLOR0;
            fixed3 ambient : COLOR1;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
         };
         v2f vert (appdata_base v)
         {
             v2f o;
             float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
             float zOff = vPos.z/_Dist;
             vPos += _QOffset*zOff*zOff;
             o.pos = mul (UNITY_MATRIX_P, vPos);
             o.uv = v.texcoord;
            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
            o.diff = nl * _LightColor0;
            o.diff.rgb += ShadeSH9(half4(worldNormal,1));
            o.ambient = ShadeSH9(half4(worldNormal,1));
            TRANSFER_SHADOW(o)
             return o;
         }
         half4 frag (v2f i) : COLOR
         {
             half4 col = tex2D(_MainTex, i.uv.xy) * _Color;
            fixed shadow = SHADOW_ATTENUATION(i);
            fixed3 lighting = i.diff * shadow + i.ambient;
            // col *= i.diff;
            col.rgb *= lighting;
             return col;
         }
         ENDCG
      }
   }
   FallBack "Diffuse"
}