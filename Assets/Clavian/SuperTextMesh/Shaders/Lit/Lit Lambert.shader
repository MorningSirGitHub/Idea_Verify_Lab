Shader "Super Text Mesh/Lit/Lambert" { 
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
        [HideInInspector] _Cutoff ("Cutoff", Range(0,1)) = 0.0001
	}
    
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting On Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
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
            surface.Albedo = mask.rgb * IN.color.rgb;
            surface.Alpha = text.a * mask.a * IN.color.a;
        }
        ENDCG
	}
}