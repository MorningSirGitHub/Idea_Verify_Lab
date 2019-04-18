// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:34159,y:32999,varname:node_9361,prsc:2|custl-5445-OUT,olwid-8212-OUT,olcol-3409-OUT;n:type:ShaderForge.SFN_NormalVector,id:7914,x:31085,y:32673,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4192,x:31328,y:32720,varname:node_4192,prsc:2,dt:0|A-7914-OUT,B-7709-OUT;n:type:ShaderForge.SFN_Multiply,id:620,x:31531,y:32720,varname:node_620,prsc:2|A-4192-OUT,B-1856-OUT;n:type:ShaderForge.SFN_Vector1,id:1856,x:31328,y:32892,varname:node_1856,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:2085,x:31690,y:32815,varname:node_2085,prsc:2|A-620-OUT,B-1856-OUT;n:type:ShaderForge.SFN_LightVector,id:7709,x:31085,y:32837,varname:node_7709,prsc:2;n:type:ShaderForge.SFN_Slider,id:8397,x:31192,y:33058,ptovrint:False,ptlb:fo_position,ptin:_fo_position,varname:_fo_position,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.411233,max:3;n:type:ShaderForge.SFN_Power,id:320,x:31541,y:33007,varname:node_320,prsc:2|VAL-2085-OUT,EXP-8397-OUT;n:type:ShaderForge.SFN_Clamp01,id:1157,x:31667,y:33007,varname:node_1157,prsc:2|IN-320-OUT;n:type:ShaderForge.SFN_ComponentMask,id:2781,x:31484,y:33162,varname:node_2781,prsc:2,cc1:0,cc2:0,cc3:-1,cc4:-1|IN-1157-OUT;n:type:ShaderForge.SFN_Tex2d,id:2023,x:31844,y:33163,ptovrint:False,ptlb:falloff,ptin:_falloff,varname:_falloff,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a26819273a8efde48816e76a4378a81d,ntxv:0,isnm:False|UVIN-2781-OUT;n:type:ShaderForge.SFN_TexCoord,id:9866,x:31891,y:32784,varname:node_9866,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Color,id:1089,x:32262,y:32854,ptovrint:False,ptlb:base_color,ptin:_base_color,varname:_base_color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1315,x:32735,y:32643,varname:node_1315,prsc:2|A-2270-RGB,B-1089-RGB;n:type:ShaderForge.SFN_Color,id:1066,x:31958,y:32988,ptovrint:False,ptlb:shadow_color,ptin:_shadow_color,varname:_shadow_color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9921569,c2:0.8392157,c3:0.8392157,c4:1;n:type:ShaderForge.SFN_Multiply,id:2654,x:32791,y:32923,varname:node_2654,prsc:2|A-2270-RGB,B-5574-OUT;n:type:ShaderForge.SFN_Lerp,id:5105,x:33113,y:33033,varname:node_5105,prsc:2|A-1315-OUT,B-2654-OUT,T-2023-RGB;n:type:ShaderForge.SFN_Color,id:598,x:32817,y:33414,ptovrint:False,ptlb:outline_color,ptin:_outline_color,varname:_outline_color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:3409,x:33182,y:33475,varname:node_3409,prsc:2|A-7340-OUT,B-598-RGB;n:type:ShaderForge.SFN_Multiply,id:7326,x:33037,y:33793,varname:node_7326,prsc:2|A-7719-RGB,B-8576-OUT,C-7096-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8212,x:33275,y:33793,varname:node_8212,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-7326-OUT;n:type:ShaderForge.SFN_Vector1,id:8576,x:32724,y:33870,varname:node_8576,prsc:2,v1:0.001;n:type:ShaderForge.SFN_Slider,id:7096,x:32676,y:34007,ptovrint:False,ptlb:outline_width,ptin:_outline_width,varname:_outline_width,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:3.970363,max:10;n:type:ShaderForge.SFN_Tex2d,id:7719,x:32598,y:33526,ptovrint:False,ptlb:outline_sample,ptin:_outline_sample,varname:_outline_sample,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9866-UVOUT;n:type:ShaderForge.SFN_Multiply,id:7340,x:32681,y:33221,varname:node_7340,prsc:2|A-2270-RGB,B-2270-RGB;n:type:ShaderForge.SFN_Dot,id:2800,x:32010,y:33763,varname:node_2800,prsc:2,dt:1|A-8325-OUT,B-2199-OUT;n:type:ShaderForge.SFN_Power,id:2246,x:32212,y:33863,cmnt:Specular Light,varname:node_2246,prsc:2|VAL-2800-OUT,EXP-9409-OUT;n:type:ShaderForge.SFN_HalfVector,id:2199,x:31801,y:33823,varname:node_2199,prsc:2;n:type:ShaderForge.SFN_Slider,id:5318,x:31314,y:33743,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Gloss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:569,x:31801,y:33980,varname:node_569,prsc:2|A-2337-OUT,B-9116-OUT;n:type:ShaderForge.SFN_Vector1,id:9116,x:31633,y:34068,varname:node_9116,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:2337,x:31633,y:33918,varname:node_2337,prsc:2|A-5318-OUT,B-3355-OUT;n:type:ShaderForge.SFN_Vector1,id:3355,x:31220,y:34001,varname:node_3355,prsc:2,v1:10;n:type:ShaderForge.SFN_Exp,id:9409,x:31972,y:33980,varname:node_9409,prsc:2,et:1|IN-569-OUT;n:type:ShaderForge.SFN_NormalVector,id:8325,x:31801,y:33684,prsc:2,pt:True;n:type:ShaderForge.SFN_Multiply,id:3534,x:32562,y:33754,varname:node_3534,prsc:2|A-4577-B,B-2246-OUT,C-2270-RGB,D-2572-OUT;n:type:ShaderForge.SFN_Tex2d,id:4577,x:32090,y:33305,ptovrint:False,ptlb:ao,ptin:_ao,varname:_ao,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9866-UVOUT;n:type:ShaderForge.SFN_Add,id:8147,x:33467,y:33196,varname:node_8147,prsc:2|A-5105-OUT,B-3534-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:5445,x:33744,y:33130,ptovrint:False,ptlb:gloss_switch,ptin:_gloss_switch,varname:_gloss_switch,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-5105-OUT,B-8147-OUT;n:type:ShaderForge.SFN_Tex2d,id:2270,x:32114,y:32691,ptovrint:True,ptlb:base,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9866-UVOUT;n:type:ShaderForge.SFN_Add,id:5574,x:32413,y:33231,varname:node_5574,prsc:2|A-1066-RGB,B-4577-G;n:type:ShaderForge.SFN_Vector1,id:2572,x:32344,y:33897,varname:node_2572,prsc:2,v1:1;proporder:8397-2023-1089-1066-598-7096-7719-5318-4577-5445-2270;pass:END;sub:END;*/

Shader "Game/Hero" {
    Properties {
        _fo_position ("fo_position", Range(0, 3)) = 0.411233
        _falloff ("falloff", 2D) = "white" {}
        _base_color ("base_color", Color) = (1,1,1,1)
        _shadow_color ("shadow_color", Color) = (0.9921569,0.8392157,0.8392157,1)
        _outline_color ("outline_color", Color) = (0,0,0,1)
        _outline_width ("outline_width", Range(0, 10)) = 3.970363
        _outline_sample ("outline_sample", 2D) = "white" {}
        _Gloss ("Gloss", Range(0, 1)) = 0
        _ao ("ao", 2D) = "white" {}
        [MaterialToggle] _gloss_switch ("gloss_switch", Float ) = 0
        _MainTex ("base", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _outline_color;
            uniform float _outline_width;
            uniform sampler2D _outline_sample; uniform float4 _outline_sample_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 _outline_sample_var = tex2Dlod(_outline_sample,float4(TRANSFORM_TEX(o.uv0, _outline_sample),0.0,0));
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*(_outline_sample_var.rgb*0.001*_outline_width).r,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                return fixed4(((_MainTex_var.rgb*_MainTex_var.rgb)*_outline_color.rgb),0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float _fo_position;
            uniform sampler2D _falloff; uniform float4 _falloff_ST;
            uniform float4 _base_color;
            uniform float4 _shadow_color;
            uniform float _Gloss;
            uniform sampler2D _ao; uniform float4 _ao_ST;
            uniform fixed _gloss_switch;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _ao_var = tex2D(_ao,TRANSFORM_TEX(i.uv0, _ao));
                float node_1856 = 0.5;
                float2 node_2781 = saturate(pow(((dot(i.normalDir,lightDirection)*node_1856)+node_1856),_fo_position)).rr;
                float4 _falloff_var = tex2D(_falloff,TRANSFORM_TEX(node_2781, _falloff));
                float3 node_5105 = lerp((_MainTex_var.rgb*_base_color.rgb),(_MainTex_var.rgb*(_shadow_color.rgb+_ao_var.g)),_falloff_var.rgb);
                float3 finalColor = lerp( node_5105, (node_5105+(_ao_var.b*pow(max(0,dot(normalDirection,halfDirection)),exp2(((_Gloss*10.0)+0.5)))*_MainTex_var.rgb*1.0)), _gloss_switch );
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float _fo_position;
            uniform sampler2D _falloff; uniform float4 _falloff_ST;
            uniform float4 _base_color;
            uniform float4 _shadow_color;
            uniform float _Gloss;
            uniform sampler2D _ao; uniform float4 _ao_ST;
            uniform fixed _gloss_switch;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _ao_var = tex2D(_ao,TRANSFORM_TEX(i.uv0, _ao));
                float node_1856 = 0.5;
                float2 node_2781 = saturate(pow(((dot(i.normalDir,lightDirection)*node_1856)+node_1856),_fo_position)).rr;
                float4 _falloff_var = tex2D(_falloff,TRANSFORM_TEX(node_2781, _falloff));
                float3 node_5105 = lerp((_MainTex_var.rgb*_base_color.rgb),(_MainTex_var.rgb*(_shadow_color.rgb+_ao_var.g)),_falloff_var.rgb);
                float3 finalColor = lerp( node_5105, (node_5105+(_ao_var.b*pow(max(0,dot(normalDirection,halfDirection)),exp2(((_Gloss*10.0)+0.5)))*_MainTex_var.rgb*1.0)), _gloss_switch );
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
