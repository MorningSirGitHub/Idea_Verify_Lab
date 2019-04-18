Shader "Super Text Mesh/Unlit/Dropshadow" { 
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
        [HideInInspector] _Cutoff ("Cutoff", Range(0,1)) = 0.0001
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _ShadowDistance ("Shadow Distance", Range(0,1)) = 0.05
        _ShadowAngle ("Shadow Angle", Range(0,360)) = 135
	}
    
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting Off Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

        sampler2D _MainTex;
        sampler2D _MaskTex;

        float4 _ShadowColor;
        float _ShadowDistance;
        float _ShadowAngle;

        struct Input {
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2_MaskTex : TEXCOORD1;
            float4 color : COLOR;
        };

        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(_ShadowAngle * MAGIC) * _ShadowDistance;
            v.vertex.y += cos(_ShadowAngle * MAGIC) * _ShadowDistance;
        }

        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
            surface.Emission = mask.rgb * _ShadowColor.rgb;
            surface.Alpha = text.a * mask.a * _ShadowColor.a * IN.color.a;
        }
        ENDCG
//normal text
        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff addshadow noforwardadd

        sampler2D _MainTex;
        sampler2D _MaskTex;

        struct Input {
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2_MaskTex : TEXCOORD1;
            float4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
            surface.Emission = mask.rgb * IN.color.rgb;
            surface.Alpha = text.a * mask.a * IN.color.a;
        }
        ENDCG

	}
}