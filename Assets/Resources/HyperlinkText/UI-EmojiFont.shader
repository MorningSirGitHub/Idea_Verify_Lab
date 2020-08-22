Shader "UI/EmojiFont" 
{
	Properties 
	{
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		
		[Space(15)]
		_EmojiTex ("Emoji Texture", 2D) = "white" {}
		_EmojiSize ("Emoji Size", Range(0, 1)) = 0.125
		_LineCount ("Line Count",float) = 8
		_FrameSpeed ("FrameSpeed",Range(0,10)) = 2
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	#include "UnityUI.cginc"

	struct appdata_t
	{
		float4 vertex			: POSITION;
		float4 color			: COLOR;
		float2 texcoord			: TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f
	{
	    float4 vertex			: SV_POSITION;
	    fixed4 color			: COLOR;
	    float2 texcoord			: TEXCOORD0;
		float2 framecoord		: TEXCOORD1;
	    float4 worldPosition	: TEXCOORD2;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	fixed4 _Color;
	fixed4 _TextureSampleAdd;
	float4 _ClipRect;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		UNITY_SETUP_INSTANCE_ID(IN);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
		OUT.worldPosition = IN.vertex;
		OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
		OUT.framecoord = fixed2(floor(IN.texcoord.x * 0.1), floor(IN.texcoord.y * 0.1));
		OUT.texcoord = IN.texcoord - OUT.framecoord * 10;

		#ifdef UNITY_HALF_TEXEL_OFFSET
		OUT.vertex.xy += (_ScreenParams.zw - 1.0) * fixed2(-1, 1) * 1;//OUT.vertex.w = 1
		#endif

		OUT.color = IN.color * _Color;
		return OUT;	
	}

	sampler2D _MainTex;
	sampler2D _EmojiTex;
	float _EmojiSize;
	float _LineCount;
	float _FrameSpeed;

	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 color;
		if (IN.framecoord.y > 0)
		{
			fixed lineIndex = IN.framecoord.x + abs(fmod(floor(_Time.y * _FrameSpeed), IN.framecoord.y));
			fixed lineNum = floor(lineIndex / _LineCount);
			color = tex2D(_EmojiTex, (float2(lineIndex - lineNum * _LineCount, lineNum) + IN.texcoord) * _EmojiSize) * IN.color.a;
		} 
		else 
		{
			color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
		}

		#ifdef UNITY_UI_CLIP_RECT
        color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
		#endif
		
		#ifdef UNITY_UI_ALPHACLIP
		clip (color.a - 0.001);
		#endif
		
		return color;
	}
    ENDCG
	
	SubShader
	{
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
		
		Pass
        {
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
			ENDCG
        }
	}
}
