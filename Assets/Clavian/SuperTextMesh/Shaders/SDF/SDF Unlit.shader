Shader "Super Text Mesh/SDF/Unlit" { 
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        [HideInInspector] _Cutoff ("Shadow Cutoff", Range(0,1)) = 0.0001 //should never change. can effect blend
        _SDFCutoff ("Cutoff", Range(0,1)) = 0.5
        _OutlineWidth ("Outline Width", Range(0,1)) = 0.2
        _Blend ("Blend Width", Range(0,1)) = 0.025 //would doing all this with a gradient be possible?
	}
    
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting Off Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff addshadow noforwardadd

        sampler2D _MainTex;
        sampler2D _MaskTex;
        float4 _OutlineColor;
        float _OutlineWidth;
        float _SDFCutoff;
        float _Blend;

        struct Input {
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2_MaskTex : TEXCOORD1;
            float4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
            if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                if(_OutlineWidth > 0){//get color from outline, or inside depending on if there's an outline or not
                    surface.Emission = _OutlineColor;
                    surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
                }else{
                    surface.Emission = mask.rgb * IN.color.rgb;
                    surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * mask.a * IN.color.a;
                }
            }
            else if(text.a < _SDFCutoff + _OutlineWidth){ //outline
                surface.Emission = _OutlineColor; //get color from outline
                surface.Alpha = _OutlineColor.a;
            }
            else if(text.a < _SDFCutoff + _OutlineWidth + _Blend){ //blend between outline and inside
                surface.Emission = lerp(_OutlineColor, mask.rgb * IN.color.rgb, (text.a - _SDFCutoff - _OutlineWidth) / _Blend);
                surface.Alpha = lerp(_OutlineColor.a, mask.a * IN.color.a, (text.a - _SDFCutoff - _OutlineWidth) / _Blend);
            }
            else{
                surface.Emission = mask.rgb * IN.color.rgb; //get color from mask & vertex
                surface.Alpha = mask.a * IN.color.a;
            }
        }
        ENDCG
	}
}