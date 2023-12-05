Shader "Hidden/TransitionShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {} 
		_EffectMap("EffectMap", 2D) = "white" {}
		_CutoffTex("CutoffTexture", 2D) = "white"{}
		_CutoffTint("CutoffTint", Color) = (1,1,1,1)
		_Cutoff	 ("Cutoff", Range(0,1)) = 0
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
			sampler2D _EffectMap;
			sampler2D _CutoffTex;
			fixed4 _CutoffTint;
			float _Cutoff;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float cOValue= tex2D(_EffectMap, i.uv);
				float colCutoff = tex2D(_CutoffTex, i.uv);

				int lower = max(sign(_Cutoff - cOValue), 0);		// cOValue <_Cutoff
				return lower * colCutoff * _CutoffTint * fixed4(1, 1, 1, 10) + (1 - lower) * col;
			}
			ENDCG
		}
	}
}
