// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Post/Post_MoHu+SeSan"
{
	Properties
	{
		_Float8("UV2的U控制模糊强度。V控制色散混合度", Float) = 0
		[KeywordEnum(X8,X16,X21,X26)] _Numberofsamples("偏移次数", Float) = 0
		[Enum(ON,0,OFF,1)]_Float5("是否加上曲线控制（UV2_U）", Float) = 1
		_U("U方向中心偏移", Range( 0 , 1)) = 0.5
		_V("V方向中心偏移", Range( 0 , 1)) = 0.5
		_blur("blur", Float) = 0
		_mask("mask", 2D) = "white" {}
		_mask_Scale("mask_Scale", Range( 0 , 1)) = 0
		_Float6("色散占比", Range( 0 , 1)) = 0.5
		[Enum(ON,0,OFF,1)]_Float7("色散占比控制是否加上曲线控制（UV2_V）", Float) = 1

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest Always
		Offset 0 , 0
		
		
		GrabPass{ "_GrabScreen_1" }

		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#pragma shader_feature _NUMBEROFSAMPLES_X8 _NUMBEROFSAMPLES_X16 _NUMBEROFSAMPLES_X21 _NUMBEROFSAMPLES_X26


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform float _Float8;
			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabScreen_1 )
			uniform half _U;
			uniform half _V;
			uniform half _blur;
			uniform float _Float5;
			uniform float _Float6;
			uniform float _Float7;
			uniform sampler2D _mask;
			uniform half _mask_Scale;
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord = screenPos;
				
				o.ase_texcoord1 = v.ase_texcoord1;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float4 screenPos = i.ase_texcoord;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 appendResult104 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
				float4 screenColor181 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,appendResult104);
				float4 screenColor192 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,appendResult104);
				float2 appendResult35 = (float2(_U , _V));
				float4 uv1212 = i.ase_texcoord1;
				uv1212.xy = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult214 = lerp( uv1212.x , 0.0 , _Float5);
				float2 temp_output_38_0 = ( ( appendResult104 - appendResult35 ) * float2( 0.01,0.01 ) * ( _blur + lerpResult214 ) );
				float2 temp_output_39_0 = ( appendResult104 - temp_output_38_0 );
				float4 screenColor193 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_39_0);
				float2 temp_output_40_0 = ( temp_output_39_0 - temp_output_38_0 );
				float4 screenColor194 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_40_0);
				float3 appendResult195 = (float3(screenColor192.r , screenColor193.g , screenColor194.b));
				float4 screenColor26 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,appendResult104);
				float4 screenColor109 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_39_0);
				float4 screenColor110 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_40_0);
				float2 temp_output_41_0 = ( temp_output_40_0 - temp_output_38_0 );
				float4 screenColor111 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_41_0);
				float2 temp_output_42_0 = ( temp_output_41_0 - temp_output_38_0 );
				float4 screenColor112 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_42_0);
				float2 temp_output_43_0 = ( temp_output_42_0 - temp_output_38_0 );
				float4 screenColor113 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_43_0);
				float2 temp_output_45_0 = ( temp_output_43_0 - temp_output_38_0 );
				float4 screenColor114 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_45_0);
				float2 temp_output_47_0 = ( temp_output_45_0 - temp_output_38_0 );
				float4 screenColor115 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_47_0);
				float4 temp_output_58_0 = ( screenColor26 + screenColor109 + screenColor110 + screenColor111 + screenColor112 + screenColor113 + screenColor114 + screenColor115 );
				float2 temp_output_44_0 = ( temp_output_47_0 - temp_output_38_0 );
				float2 myVarName123 = temp_output_38_0;
				float2 temp_output_46_0 = ( temp_output_44_0 - myVarName123 );
				float2 temp_output_129_0 = ( temp_output_46_0 - myVarName123 );
				float2 temp_output_130_0 = ( temp_output_129_0 - myVarName123 );
				float2 temp_output_131_0 = ( temp_output_130_0 - myVarName123 );
				float2 temp_output_132_0 = ( temp_output_131_0 - myVarName123 );
				float2 temp_output_144_0 = ( temp_output_132_0 - myVarName123 );
				float2 temp_output_145_0 = ( temp_output_144_0 - myVarName123 );
				float4 screenColor143 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_145_0);
				float4 screenColor116 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_44_0);
				float4 screenColor117 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_46_0);
				float4 screenColor125 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_129_0);
				float4 screenColor126 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_130_0);
				float4 screenColor127 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_131_0);
				float4 screenColor128 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_132_0);
				float4 screenColor142 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_144_0);
				float4 temp_output_134_0 = ( temp_output_58_0 + ( screenColor143 + screenColor116 + screenColor117 + screenColor125 + screenColor126 + screenColor127 + screenColor128 + screenColor142 ) );
				float2 temp_output_152_0 = ( temp_output_145_0 - myVarName123 );
				float4 screenColor147 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_152_0);
				float2 temp_output_153_0 = ( temp_output_152_0 - myVarName123 );
				float4 screenColor148 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_153_0);
				float2 temp_output_154_0 = ( temp_output_153_0 - myVarName123 );
				float4 screenColor149 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_154_0);
				float2 temp_output_155_0 = ( temp_output_154_0 - myVarName123 );
				float4 screenColor151 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_155_0);
				float2 temp_output_156_0 = ( temp_output_155_0 - myVarName123 );
				float4 screenColor150 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_156_0);
				float4 temp_output_157_0 = ( temp_output_134_0 + ( screenColor147 + screenColor148 + screenColor149 + screenColor151 + screenColor150 ) );
				float2 temp_output_173_0 = ( temp_output_156_0 - myVarName123 );
				float4 screenColor168 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_173_0);
				float2 temp_output_172_0 = ( temp_output_173_0 - myVarName123 );
				float4 screenColor167 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_172_0);
				float2 temp_output_171_0 = ( temp_output_172_0 - myVarName123 );
				float4 screenColor166 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_171_0);
				float2 temp_output_170_0 = ( temp_output_171_0 - myVarName123 );
				float4 screenColor165 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,temp_output_170_0);
				float4 screenColor164 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabScreen_1,( temp_output_170_0 - myVarName123 ));
				#if defined(_NUMBEROFSAMPLES_X8)
				float4 staticSwitch174 = ( temp_output_58_0 / 8.0 );
				#elif defined(_NUMBEROFSAMPLES_X16)
				float4 staticSwitch174 = ( temp_output_134_0 / 16.0 );
				#elif defined(_NUMBEROFSAMPLES_X21)
				float4 staticSwitch174 = ( temp_output_157_0 / 21.0 );
				#elif defined(_NUMBEROFSAMPLES_X26)
				float4 staticSwitch174 = ( ( temp_output_157_0 + ( screenColor168 + screenColor167 + screenColor166 + screenColor165 + screenColor164 ) ) / 26.0 );
				#else
				float4 staticSwitch174 = ( temp_output_58_0 / 8.0 );
				#endif
				float4 uv1220 = i.ase_texcoord1;
				uv1220.xy = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float lerpResult221 = lerp( uv1220.y , 0.0 , _Float7);
				float4 lerpResult180 = lerp( float4( appendResult195 , 0.0 ) , staticSwitch174 , ( _Float6 + lerpResult221 ));
				float4 lerpResult224 = lerp( screenColor181 , lerpResult180 , ( tex2D( _mask, appendResult104 ).r * _mask_Scale ));
				
				
				finalColor = lerpResult224;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17500
286;263;2035;1102;-1504.89;-7.949196;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;210;-1828.454,760.3204;Inherit;False;533.45;303;;2;104;199;屏幕UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;200;-1891.947,1182.823;Inherit;False;661.871;245;;4;34;33;35;37;改变UV中心点到屏幕中央;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;201;-2058.358,1527.078;Inherit;False;777.51;487;;7;215;216;214;213;38;212;36;乘0.01让模糊值更好控制;1,1,1,1;0;0
Node;AmplifyShaderEditor.GrabScreenPosition;199;-1778.454,815.5663;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;215;-1955.768,1818.291;Inherit;False;Constant;_Float4;Float 4;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1841.947,1232.823;Half;False;Property;_U;U方向中心偏移;3;0;Create;False;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1841.947,1312.823;Half;False;Property;_V;V方向中心偏移;4;0;Create;False;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-2047.768,1894.291;Inherit;False;Property;_Float5;是否加上曲线控制（UV2_U）;2;1;[Enum];Create;False;2;ON;0;OFF;1;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;212;-2039.753,1657.261;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;35;-1554.313,1237.446;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-2034.207,1576.561;Half;False;Property;_blur;blur;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;214;-1780.768,1690.291;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;104;-1530.004,810.3203;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;213;-1600.768,1603.291;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-1404.076,1236.255;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;175;-809.059,1373.226;Inherit;False;2483.407;3491.252;;55;40;167;168;165;164;166;151;147;169;150;149;148;142;143;170;171;172;173;156;155;154;153;152;145;144;112;125;116;111;127;126;114;113;117;115;128;132;131;130;129;46;44;47;45;43;42;41;109;26;110;39;204;205;206;207;每次缩放后的UV再减缩放强度给到下一张抓屏当UV，如此规律向下循环;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1423.848,1571.078;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0.01,0.01;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;39;-756.7949,1446.506;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;-743.0226,1599.628;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-755.7862,1724.573;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;42;-739.7862,1884.573;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;43;-721.1606,2054.961;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;202;-1549.318,2086.016;Inherit;False;293;165;;1;123;模糊强度;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;45;-691.6698,2182.174;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;-1499.318,2136.016;Float;False;myVarName;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;203;-1542.536,3284.261;Inherit;False;282;165;;1;124;节点太远了，get模糊强度;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;47;-691.7542,2350.127;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-1492.536,3334.261;Inherit;False;123;myVarName;1;0;OBJECT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;44;-702.476,2543.392;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-705.1016,2765.185;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;129;-670.1779,2950.989;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;130;-678.8344,3141.676;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;131;-680.984,3297.795;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;132;-683.6097,3519.588;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;144;-326.2541,4290.831;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;145;-325.0105,4473.196;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;152;262.9043,3131.08;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;153;274.2885,3322.714;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;154;281.8779,3506.758;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;155;279.9807,3675.623;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;156;278.0833,3817.926;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;173;309.3149,3985.769;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;172;320.699,4175.404;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;171;328.2885,4359.446;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenColorNode;113;-387.8733,2453.311;Float;False;Global;_GrabScreen2;Grab Screen 2;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;117;-374.3377,3260.091;Float;False;Global;_GrabScreen14;Grab Screen 14;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;111;-387.0003,2037.119;Float;False;Global;_GrabScreen6;Grab Screen 6;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;109;-384.8761,1624.826;Float;False;Global;_GrabScreen1;Grab Screen 1;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;26;-385.0881,1439.43;Float;False;Global;_GrabScreen0;Grab Screen 0;9;0;Fetch;True;0;0;True;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;204;27.81002,1457.556;Inherit;False;365.3561;441.59;;3;62;138;58;偏移8次的模糊结果;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenColorNode;114;-383.0269,2639.831;Float;False;Global;_GrabScreen7;Grab Screen 7;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;205;588.4613,2217.229;Inherit;False;581.5006;481.4781;;4;136;137;134;133;偏移16次的模糊结果;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenColorNode;128;-372.5376,4062.7;Float;False;Global;_GrabScreen10;Grab Screen 10;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;125;-370.9344,3458.891;Float;False;Global;_GrabScreen4;Grab Screen 4;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;115;-380.4007,2873.136;Float;False;Global;_GrabScreen12;Grab Screen 12;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;116;-379.9487,3076.398;Float;False;Global;_GrabScreen13;Grab Screen 13;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;112;-382.2464,2249.709;Float;False;Global;_GrabScreen5;Grab Screen 5;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;126;-373.3136,3710.229;Float;False;Global;_GrabScreen8;Grab Screen 8;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;142;305.8416,2348.286;Float;False;Global;_GrabScreen11;Grab Screen 11;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;170;326.3911,4528.311;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenColorNode;143;299.6511,2515.908;Float;False;Global;_GrabScreen15;Grab Screen 15;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;110;-365.2586,1809.557;Float;False;Global;_GrabScreen3;Grab Screen 3;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;127;-377.1536,3887.302;Float;False;Global;_GrabScreen9;Grab Screen 9;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;148;604.7083,3188.215;Float;False;Global;_GrabScreen17;Grab Screen 17;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;206;920.2414,3381.103;Inherit;False;572.4196;259.085;;4;158;159;157;146;偏移21次的模糊结果;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenColorNode;151;619.5452,3542.558;Float;False;Global;_GrabScreen20;Grab Screen 20;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;147;601.3522,2990.2;Float;False;Global;_GrabScreen16;Grab Screen 16;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;169;324.4937,4670.614;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenColorNode;149;621.4891,3364.68;Float;False;Global;_GrabScreen18;Grab Screen 18;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;58;89.80165,1629.857;Inherit;False;8;8;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;150;618.1329,3718.493;Float;False;Global;_GrabScreen19;Grab Screen 19;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;133;646.4506,2268.274;Inherit;False;8;8;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;164;647.1216,4638.765;Float;False;Global;_GrabScreen21;Grab Screen 21;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;168;632.2848,3914.419;Float;False;Global;_GrabScreen25;Grab Screen 25;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;165;652.4218,4462.83;Float;False;Global;_GrabScreen22;Grab Screen 22;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;207;997.9741,4221.446;Inherit;False;657.3184;349.3452;;4;162;160;163;161;偏移26次的模糊结果;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;134;826.0374,2267.229;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;146;970.2414,3438.188;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;167;635.6407,4110.432;Float;False;Global;_GrabScreen24;Grab Screen 24;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;166;652.4218,4284.952;Float;False;Global;_GrabScreen23;Grab Screen 23;9;0;Fetch;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;157;1159.023,3431.103;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;161;1074.847,4309.021;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;220;1629.296,1205.944;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;138;76.09878,1514.503;Half;False;Constant;_Float1;Float 1;9;0;Create;True;0;0;False;0;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;1688.818,1375.197;Inherit;False;Property;_Float7;色散占比控制是否加上曲线控制（UV2_V）;9;1;[Enum];Create;False;2;ON;0;OFF;1;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;1300.497,4268.758;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;137;659.5059,2520.566;Float;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;16;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;1129.66,3521.941;Float;False;Constant;_Float2;Float 2;10;0;Create;True;0;0;False;0;21;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;196;-1.759308,414.1848;Inherit;False;785.3717;623.2595;Comment;4;192;195;194;193;色散;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;163;1310.577,4453.104;Float;False;Constant;_Float3;Float 3;10;0;Create;True;0;0;False;0;26;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;194;58.5645,806.3453;Float;False;Global;_GrabScreen28;_GrabScreen28;9;0;Create;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;192;48.2408,464.1848;Float;False;Global;_GrabScreen26;_GrabScreen26;9;0;Create;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;209;1281.345,999.7952;Inherit;False;334;234;;1;174;静态开关，选择放射次数;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;136;1012.962,2273.582;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;158;1335.661,3439.941;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;208;1601.966,520.8856;Inherit;False;928.1311;380.2601;;5;180;178;176;177;224;masklerp原色彩和处理后的结果 ;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;62;234.4549,1509.267;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;218;1632.874,1118.501;Inherit;False;Property;_Float6;色散占比;8;0;Create;False;0;0;False;0;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;221;2017.296,1254.944;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;162;1525.165,4391.176;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;193;49.7157,633.7903;Float;False;Global;_GrabScreen27;_GrabScreen27;9;0;Create;True;0;0;False;0;Instance;181;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;219;2275.874,1122.501;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;176;1754.966,558.8854;Inherit;True;Property;_mask;mask;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;174;1331.345,1049.795;Float;False;Property;_Numberofsamples;偏移次数;1;0;Create;False;0;0;False;0;0;0;0;True;;KeywordEnum;4;X8;X16;X21;X26;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;178;1862.11,784.6469;Half;False;Property;_mask_Scale;mask_Scale;7;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;195;538.373,612.6006;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;211;-1074.551,779.0472;Inherit;False;261;257;;1;181;抓取屏幕颜色;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;180;1653.097,751.388;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;181;-1024.551,829.0472;Float;False;Global;_GrabScreen_1;Grab Screen_1;9;0;Fetch;True;0;0;True;0;Object;-1;True;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;2128.803,576.6856;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;224;2314.786,581.9337;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;225;2563.89,436.9492;Inherit;False;Property;_Float8;UV2的U控制模糊强度。V控制色散混合度;0;0;Create;False;0;0;True;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;198;2612.028,584.8977;Float;False;True;-1;2;ASEMaterialInspector;100;1;Post/Post_MoHu+SeSan;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;7;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;214;0;212;1
WireConnection;214;1;215;0
WireConnection;214;2;216;0
WireConnection;104;0;199;1
WireConnection;104;1;199;2
WireConnection;213;0;36;0
WireConnection;213;1;214;0
WireConnection;37;0;104;0
WireConnection;37;1;35;0
WireConnection;38;0;37;0
WireConnection;38;2;213;0
WireConnection;39;0;104;0
WireConnection;39;1;38;0
WireConnection;40;0;39;0
WireConnection;40;1;38;0
WireConnection;41;0;40;0
WireConnection;41;1;38;0
WireConnection;42;0;41;0
WireConnection;42;1;38;0
WireConnection;43;0;42;0
WireConnection;43;1;38;0
WireConnection;45;0;43;0
WireConnection;45;1;38;0
WireConnection;123;0;38;0
WireConnection;47;0;45;0
WireConnection;47;1;38;0
WireConnection;44;0;47;0
WireConnection;44;1;38;0
WireConnection;46;0;44;0
WireConnection;46;1;124;0
WireConnection;129;0;46;0
WireConnection;129;1;124;0
WireConnection;130;0;129;0
WireConnection;130;1;124;0
WireConnection;131;0;130;0
WireConnection;131;1;124;0
WireConnection;132;0;131;0
WireConnection;132;1;124;0
WireConnection;144;0;132;0
WireConnection;144;1;124;0
WireConnection;145;0;144;0
WireConnection;145;1;124;0
WireConnection;152;0;145;0
WireConnection;152;1;124;0
WireConnection;153;0;152;0
WireConnection;153;1;124;0
WireConnection;154;0;153;0
WireConnection;154;1;124;0
WireConnection;155;0;154;0
WireConnection;155;1;124;0
WireConnection;156;0;155;0
WireConnection;156;1;124;0
WireConnection;173;0;156;0
WireConnection;173;1;124;0
WireConnection;172;0;173;0
WireConnection;172;1;124;0
WireConnection;171;0;172;0
WireConnection;171;1;124;0
WireConnection;113;0;43;0
WireConnection;117;0;46;0
WireConnection;111;0;41;0
WireConnection;109;0;39;0
WireConnection;26;0;104;0
WireConnection;114;0;45;0
WireConnection;128;0;132;0
WireConnection;125;0;129;0
WireConnection;115;0;47;0
WireConnection;116;0;44;0
WireConnection;112;0;42;0
WireConnection;126;0;130;0
WireConnection;142;0;144;0
WireConnection;170;0;171;0
WireConnection;170;1;124;0
WireConnection;143;0;145;0
WireConnection;110;0;40;0
WireConnection;127;0;131;0
WireConnection;148;0;153;0
WireConnection;151;0;155;0
WireConnection;147;0;152;0
WireConnection;169;0;170;0
WireConnection;169;1;124;0
WireConnection;149;0;154;0
WireConnection;58;0;26;0
WireConnection;58;1;109;0
WireConnection;58;2;110;0
WireConnection;58;3;111;0
WireConnection;58;4;112;0
WireConnection;58;5;113;0
WireConnection;58;6;114;0
WireConnection;58;7;115;0
WireConnection;150;0;156;0
WireConnection;133;0;143;0
WireConnection;133;1;116;0
WireConnection;133;2;117;0
WireConnection;133;3;125;0
WireConnection;133;4;126;0
WireConnection;133;5;127;0
WireConnection;133;6;128;0
WireConnection;133;7;142;0
WireConnection;164;0;169;0
WireConnection;168;0;173;0
WireConnection;165;0;170;0
WireConnection;134;0;58;0
WireConnection;134;1;133;0
WireConnection;146;0;147;0
WireConnection;146;1;148;0
WireConnection;146;2;149;0
WireConnection;146;3;151;0
WireConnection;146;4;150;0
WireConnection;167;0;172;0
WireConnection;166;0;171;0
WireConnection;157;0;134;0
WireConnection;157;1;146;0
WireConnection;161;0;168;0
WireConnection;161;1;167;0
WireConnection;161;2;166;0
WireConnection;161;3;165;0
WireConnection;161;4;164;0
WireConnection;160;0;157;0
WireConnection;160;1;161;0
WireConnection;194;0;40;0
WireConnection;192;0;104;0
WireConnection;136;0;134;0
WireConnection;136;1;137;0
WireConnection;158;0;157;0
WireConnection;158;1;159;0
WireConnection;62;0;58;0
WireConnection;62;1;138;0
WireConnection;221;0;220;2
WireConnection;221;2;223;0
WireConnection;162;0;160;0
WireConnection;162;1;163;0
WireConnection;193;0;39;0
WireConnection;219;0;218;0
WireConnection;219;1;221;0
WireConnection;176;1;104;0
WireConnection;174;1;62;0
WireConnection;174;0;136;0
WireConnection;174;2;158;0
WireConnection;174;3;162;0
WireConnection;195;0;192;1
WireConnection;195;1;193;2
WireConnection;195;2;194;3
WireConnection;180;0;195;0
WireConnection;180;1;174;0
WireConnection;180;2;219;0
WireConnection;181;0;104;0
WireConnection;177;0;176;1
WireConnection;177;1;178;0
WireConnection;224;0;181;0
WireConnection;224;1;180;0
WireConnection;224;2;177;0
WireConnection;198;0;224;0
ASEEND*/
//CHKSM=DF9602CCF88BCBC6550216FA966DECA9B656B55D