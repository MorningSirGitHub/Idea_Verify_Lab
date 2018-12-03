// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BurnToFadeOut" {
	Properties{
		_StartColor("Start Color", Color) = (1,1,1,1)
		_EndColor("End Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_Emit("Emit level", Range(1,100)) = 0
		_Range("Range", Range(0,1)) = 0
		_AlphaCutOff("AlphaCutOff",Range(0,1)) = 0
	}
		SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 100

		Lighting Off
		Cull Off

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		half2 texcoord : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed4 _StartColor;
	fixed4 _EndColor;
	fixed _Cutoff;
	half _Emit;
	half _Range;
	uniform float _AlphaCutOff;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		fixed4 col = tex2D(_MainTex, i.texcoord);
	clip(col.a - _AlphaCutOff);

	fixed a = dot(col.xyz, fixed3(0.3, 0.59, 0.11));
	col.a = a;
	clip(a - _Cutoff);
	if (a < _Cutoff + _Range)
		col.xyz = lerp(_StartColor.xyz, _EndColor.xyz, (saturate(a - _Cutoff) / _Range)) * _Emit;



	return col;
	}
		ENDCG
	}
	}

		SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 100

		Pass{
		Lighting Off
		Alphatest Greater[_Cutoff]
		SetTexture[_MainTex]{ combine texture }
	}
	}
}