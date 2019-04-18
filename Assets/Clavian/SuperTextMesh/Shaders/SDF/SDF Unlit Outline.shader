Shader "Super Text Mesh/SDF/Unlit Outline" { 
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
	//outline
		CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(0 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(0 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(90 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(90 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(180 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(180 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(270 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(270 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(45 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(45 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(135 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(135 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(225 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(225 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf2 Lambert alphatest:_Cutoff noforwardadd vertex:vert

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
        void vert (inout appdata_full v) { //modify vertex data
            const float MAGIC = 3.14159 / 180;
            v.vertex.x += sin(315 * MAGIC) * _OutlineWidth;
            v.vertex.y += cos(315 * MAGIC) * _OutlineWidth;
        }
        void surf2 (Input IN, inout SurfaceOutput surface) {
            half4 text = tex2D (_MainTex, IN.uv_MainTex.xy);
            half4 mask = tex2D (_MaskTex, IN.uv2_MaskTex.xy);
          	if(text.a < _SDFCutoff){
                surface.Alpha = 0; //cut!
            }
            else if(text.a < _SDFCutoff + _Blend){ //blend between nothing and outline
                surface.Emission = _OutlineColor;
                surface.Alpha = (text.a - _SDFCutoff + (_Blend/100)) / _Blend * _OutlineColor.a;
            }
            else{
                surface.Emission = mask.rgb * _OutlineColor.rgb;
                surface.Alpha = mask.a * _OutlineColor.a * IN.color.a;
            }
        }
        ENDCG

    //the actual letter
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
            else if(text.a < _SDFCutoff + _Blend){ //blend between outside and inside
                surface.Emission = lerp(_OutlineColor, mask.rgb * IN.color.rgb, (text.a - _SDFCutoff) / _Blend);
                surface.Alpha = lerp(_OutlineColor.a, mask.a * IN.color.a, (text.a - _SDFCutoff) / _Blend);
            }
            else{
                surface.Emission = mask.rgb * IN.color.rgb; //get color from mask & vertex
                surface.Alpha = mask.a * IN.color.a;
            }
        }
        ENDCG
	}
}