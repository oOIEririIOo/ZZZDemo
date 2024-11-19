// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FxClass/AdditiveMask"
{
	Properties
	{
		[HDR]_MainColor1("主色调", Color) = (0.6792453,0.6792453,0.6792453,0)
		_MainTex1("主贴图", 2D) = "white" {}
		_MainTexUspeed1("主贴图U速度", Float) = 0
		_MianTexVspeed1("主贴图V速度", Float) = 0
		_SecondTex1("纹理贴图", 2D) = "white" {}
		_SecTexUspeed1("纹理贴图U速度", Float) = 0
		_SecTexVspeed1("纹理贴图V速度", Float) = 0
		_MaskTex1("遮罩贴图", 2D) = "white" {}
		_Softedge1("软粒子", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float4 _MainColor1;
		uniform sampler2D _SecondTex1;
		SamplerState sampler_SecondTex1;
		uniform float _SecTexUspeed1;
		uniform float _SecTexVspeed1;
		uniform sampler2D _MaskTex1;
		uniform float4 _MaskTex1_ST;
		uniform sampler2D _MainTex1;
		SamplerState sampler_MainTex1;
		uniform float _MainTexUspeed1;
		uniform float _MianTexVspeed1;
		uniform float4 _MainTex1_ST;
		SamplerState sampler_MaskTex1;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Softedge1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult35 = (float2(_SecTexUspeed1 , _SecTexVspeed1));
			float2 uv_MaskTex1 = i.uv_texcoord * _MaskTex1_ST.xy + _MaskTex1_ST.zw;
			float2 panner36 = ( 1.0 * _Time.y * appendResult35 + uv_MaskTex1);
			float2 appendResult32 = (float2(_MainTexUspeed1 , _MianTexVspeed1));
			float2 uv_MainTex1 = i.uv_texcoord * _MainTex1_ST.xy + _MainTex1_ST.zw;
			float2 panner38 = ( 1.0 * _Time.y * appendResult32 + uv_MainTex1);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth42 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth42 = saturate( abs( ( screenDepth42 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Softedge1 ) ) );
			o.Emission = ( i.vertexColor * _MainColor1 * tex2D( _SecondTex1, panner36 ).r * tex2D( _MainTex1, panner38 ).r * tex2D( _MaskTex1, uv_MaskTex1 ).r * distanceDepth42 * i.vertexColor.a ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
7;98;1828;614;1048.583;327.9868;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;28;-718.663,-293.8559;Inherit;False;Property;_MianTexVspeed1;主贴图V速度;4;0;Create;False;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-718.663,-365.8559;Inherit;False;Property;_MainTexUspeed1;主贴图U速度;3;0;Create;False;0;0;False;0;False;0;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-730.3793,-30.39435;Inherit;False;Property;_SecTexVspeed1;纹理贴图V速度;7;0;Create;False;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-731.3793,-98.3943;Inherit;False;Property;_SecTexUspeed1;纹理贴图U速度;6;0;Create;False;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-658.663,-483.4979;Inherit;False;0;39;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;-655.0793,-222.7943;Inherit;False;0;41;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;32;-579.663,-363.8559;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-577.3793,-94.39429;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;36;-436.3792,-148.3944;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-387.6889,226.4415;Inherit;False;Property;_Softedge1;软粒子;9;0;Create;False;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;38;-438.6629,-417.856;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;44;-174.4364,-785.3474;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;42;-217.8853,206.797;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-259.6528,-179.9875;Inherit;True;Property;_SecondTex1;纹理贴图;5;0;Create;False;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-266.1821,10.6347;Inherit;True;Property;_MaskTex1;遮罩贴图;8;0;Create;False;0;0;False;0;False;-1;None;4c5ed528efef347438076da92444f1b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;39;-259.6365,-447.449;Inherit;True;Property;_MainTex1;主贴图;2;0;Create;False;0;0;False;0;False;-1;None;c5876a02acc3dc14ba891c894e84c86b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;46;-212.9366,-618.7491;Inherit;False;Property;_MainColor1;主色调;1;1;[HDR];Create;False;0;0;False;0;False;0.6792453,0.6792453,0.6792453,0;0.6792453,0.6792453,0.6792453,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;245.6783,-460.8691;Inherit;False;7;7;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;676.3999,-387.7;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;FxClass/AdditiveMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;29;0
WireConnection;32;1;28;0
WireConnection;35;0;31;0
WireConnection;35;1;30;0
WireConnection;36;0;34;0
WireConnection;36;2;35;0
WireConnection;38;0;33;0
WireConnection;38;2;32;0
WireConnection;42;0;37;0
WireConnection;40;1;36;0
WireConnection;39;1;38;0
WireConnection;48;0;44;0
WireConnection;48;1;46;0
WireConnection;48;2;40;1
WireConnection;48;3;39;1
WireConnection;48;4;41;1
WireConnection;48;5;42;0
WireConnection;48;6;44;4
WireConnection;0;2;48;0
ASEEND*/
//CHKSM=F0F20164E943712B1343B02D1343B103F4E9CF90