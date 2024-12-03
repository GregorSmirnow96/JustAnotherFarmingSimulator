// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "COMI/WaterfallCenter"
{
	Properties
	{
		[Header()][Header(_____Main Texture_____)][Space(10)]_TextureMain("TextureMain", 2D) = "white" {}
		_TillingX("TillingX", Range( 1 , 20)) = 0
		_TillingY("TillingY", Range( 0 , 5)) = 0
		_WaterSpeed("WaterSpeed", Range( 0 , 1)) = 1
		_MainColorScaleAndOffset("MainColorScaleAndOffset", Vector) = (1,0,0,0)
		[Header()][Header(_____Main Texture_____)][Space(10)]_TextureMain2("TextureMain2", 2D) = "white" {}
		_Tilling2X("Tilling2X", Range( 1 , 20)) = 0
		_DepthFadeDistance("Depth Fade Distance", Range( 0 , 1)) = 0.5
		_Tilling2Y("Tilling2Y", Range( 0 , 5)) = 0
		_Offset2X("Offset2X", Range( 0 , 10)) = 0
		_Water2Speed("Water2Speed", Range( 0 , 1)) = 1
		_MainColor2ScaleAndOffset("MainColor2ScaleAndOffset", Vector) = (1,0,0,0)
		[Header(_____ Set Noise_____)][Space(10)]_Noise("Noise", 2D) = "white" {}
		_NoiseValue("NoiseValue", Range( 0 , 1)) = 0.4
		_NoiseUVTilling("NoiseUVTilling ", Range( 0.01 , 10)) = 0
		_NoiseSpeedY("NoiseSpeedY", Range( 0 , 5)) = 0
		[Header(_____NoiseColor Intensity_____)][Space(10)]_ColorNoiseScale("ColorNoiseScale", Range( 0 , 10)) = 1
		_ColorNoiseOffset("ColorNoiseOffset", Range( -2 , 2)) = 0
		[Header(_____Set Color_____)][Space(10)]_TopColor("TopColor", Color) = (1,1,1,1)
		_BottomColor("BottomColor", Color) = (1,1,1,1)
		_ColorLerpScale("ColorLerpScale", Range( 0 , 2)) = 1
		_ColorLerpOffset("ColorLerpOffset", Range( -1 , 1)) = 0
		[Header(_____ Opacity_____)][Space(10)]_OpacityValue("OpacityValue", Range( 0 , 1)) = 1
		[Header(_____ Edge Opacity_____)][Space(10)]_EdgeScale("EdgeScale", Range( 0 , 2)) = 1
		_EdgeOffset("EdgeOffset", Range( -1 , 1)) = 0
		_Color1Small("Color1Small", Color) = (0,0,0,0)
		_Color2Small("Color2Small", Color) = (0,0,0,0)
		_Color1Big("Color1Big", Color) = (0,0,0,0)
		_Color2BIg("Color2BIg", Color) = (0,0,0,0)
		_FresnelScale("Fresnel Scale", Range( 0 , 2)) = 1
		_FresnelEdgeColor("FresnelEdgeColor", Color) = (1,1,1,1)
		_FresnelBias("FresnelBias", Range( 0 , 1)) = 0.2
		_FresnelPower("Fresnel Power", Range( 0 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color1Small;
		uniform float4 _Color1Big;
		uniform sampler2D _TextureMain;
		uniform float _NoiseValue;
		uniform sampler2D _Noise;
		uniform float _NoiseSpeedY;
		uniform float _NoiseUVTilling;
		uniform float _WaterSpeed;
		uniform float _TillingX;
		uniform float _TillingY;
		uniform float2 _MainColorScaleAndOffset;
		uniform float4 _Color2Small;
		uniform float4 _Color2BIg;
		uniform sampler2D _TextureMain2;
		uniform float _Water2Speed;
		uniform float _Tilling2X;
		uniform float _Tilling2Y;
		uniform float _Offset2X;
		uniform float2 _MainColor2ScaleAndOffset;
		uniform float4 _BottomColor;
		uniform float4 _TopColor;
		uniform float _ColorLerpScale;
		uniform float _ColorLerpOffset;
		uniform float _ColorNoiseScale;
		uniform float _ColorNoiseOffset;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFadeDistance;
		uniform float4 _FresnelEdgeColor;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _EdgeScale;
		uniform float _EdgeOffset;
		uniform float _OpacityValue;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult41 = (float2(0.0 , _NoiseSpeedY));
			float2 appendResult31 = (float2(_NoiseUVTilling , _NoiseUVTilling));
			float2 uv_TexCoord35 = i.uv_texcoord * appendResult31;
			float2 panner36 = ( 1.0 * _Time.y * appendResult41 + uv_TexCoord35);
			float temp_output_27_0 = ( _NoiseValue * tex2D( _Noise, panner36 ).r );
			float2 appendResult15 = (float2(_TillingX , _TillingY));
			float2 uv_TexCoord12 = i.uv_texcoord * appendResult15;
			float2 panner7 = ( ( _Time.y * _WaterSpeed ) * float2( 0,1 ) + uv_TexCoord12);
			float4 lerpResult86 = lerp( _Color1Small , _Color1Big , saturate( (tex2D( _TextureMain, ( temp_output_27_0 + panner7 ) ).r*_MainColorScaleAndOffset.x + _MainColorScaleAndOffset.y) ));
			float2 appendResult99 = (float2(_Tilling2X , _Tilling2Y));
			float2 appendResult102 = (float2(_Offset2X , 0.0));
			float2 uv_TexCoord100 = i.uv_texcoord * appendResult99 + appendResult102;
			float2 panner93 = ( ( _Time.y * _Water2Speed ) * float2( 0,1 ) + uv_TexCoord100);
			float4 lerpResult107 = lerp( _Color2Small , _Color2BIg , saturate( (tex2D( _TextureMain2, ( temp_output_27_0 + panner93 ) ).r*_MainColor2ScaleAndOffset.x + _MainColor2ScaleAndOffset.y) ));
			float4 lerpResult52 = lerp( _BottomColor , _TopColor , (i.uv_texcoord.y*_ColorLerpScale + _ColorLerpOffset));
			float3 appendResult63 = (float3(( lerpResult86 * lerpResult107 * lerpResult52 ).rgb));
			float temp_output_58_0 = (temp_output_27_0*_ColorNoiseScale + _ColorNoiseOffset);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth114 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth114 = saturate( abs( ( screenDepth114 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeDistance ) ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV123 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode123 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV123, _FresnelPower ) );
			float4 appendResult64 = (float4(( saturate( ( float4( ( appendResult63 * temp_output_58_0 ) , 0.0 ) + ( saturate( ( 1.0 - distanceDepth114 ) ) * _FresnelEdgeColor * ( fresnelNode123 + -0.03 ) ) ) ) + float4( 0,0,0,0 ) )));
			o.Emission = appendResult64.xyz;
			o.Alpha = saturate( ( (( 1.0 - abs( ( i.uv_texcoord.x + -0.5 ) ) )*_EdgeScale + _EdgeOffset) * _OpacityValue * temp_output_58_0 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma exclude_renderers xboxseries playstation switch nomrt 
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	// CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18934
7;30;1906;989;1593.532;405.159;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;91;-4360.135,653.9586;Inherit;False;1800.815;486.5311;Noise;9;30;31;40;41;35;36;28;26;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-4310.135,737.686;Float;False;Property;_NoiseUVTilling;NoiseUVTilling ;14;0;Create;True;0;0;0;False;0;False;0;3.95;0.01;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-3850.921,967.7753;Inherit;False;Property;_NoiseSpeedY;NoiseSpeedY;15;0;Create;True;0;0;0;False;0;False;0;1.02;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-3893.234,727.8861;Inherit;False;FLOAT2;4;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;41;-3524.921,928.7753;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-3691.633,735.286;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-3262.724,-170.8728;Float;False;Property;_TillingY;TillingY;2;0;Create;True;0;0;0;False;0;False;0;0.84;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-3978.042,339.9533;Float;False;Property;_Offset2X;Offset2X;9;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-4032.004,80.17844;Float;False;Property;_Tilling2X;Tilling2X;6;0;Create;True;0;0;0;False;0;False;0;1.36;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-4044.953,180.3102;Float;False;Property;_Tilling2Y;Tilling2Y;8;0;Create;True;0;0;0;False;0;False;0;0.2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-3249.775,-271.0045;Float;False;Property;_TillingX;TillingX;1;0;Create;True;0;0;0;False;0;False;0;2.4;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;36;-3360.634,959.2859;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-2906.513,-205.9608;Inherit;False;FLOAT2;4;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-3209.557,507.9273;Float;False;Property;_Water2Speed;Water2Speed;10;0;Create;True;0;0;0;False;0;False;1;0.014;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;4;-3060.334,-46.48996;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;102;-3634.78,403.397;Inherit;False;FLOAT2;4;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;99;-3688.742,145.2221;Inherit;False;FLOAT2;4;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-3082.569,139.0796;Float;False;Property;_WaterSpeed;WaterSpeed;3;0;Create;True;0;0;0;False;0;False;1;0.354;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;-3038.54,910.4898;Inherit;True;Property;_Noise;Noise;12;0;Create;True;0;0;0;False;2;Header(_____ Set Noise_____);Space(10);False;-1;None;1d0d70fca0b4b544191a98442f4c6d14;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-2665.837,108.8513;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-3374.863,703.9586;Inherit;False;Property;_NoiseValue;NoiseValue;13;0;Create;True;0;0;0;False;0;False;0.4;0.084;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-2717.556,-188.4456;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;100;-3353.785,232.7373;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-2673.587,463.8935;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-2372.806,46.06025;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-2721.32,732.5372;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;93;-2369.246,311.2048;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;-1867.026,397.9231;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-1791.845,51.85332;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;-1475.098,55.8656;Inherit;True;Property;_TextureMain;TextureMain;0;1;[Header];Create;True;1;;0;0;False;2;Header(_____Main Texture_____);Space(10);False;-1;None;942f2e0492acc4c1fb831e862e63d990;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;96;-1532.6,436.26;Inherit;True;Property;_TextureMain2;TextureMain2;5;1;[Header];Create;True;1;;0;0;False;2;Header(_____Main Texture_____);Space(10);False;-1;None;1270e24b17e09b14ba7f6226324f9d6e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;48;-1308.952,-90.70367;Inherit;False;Property;_MainColorScaleAndOffset;MainColorScaleAndOffset;4;0;Create;True;0;0;0;False;0;False;1,0;2.76,-0.45;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;106;-1268.114,307.7509;Inherit;False;Property;_MainColor2ScaleAndOffset;MainColor2ScaleAndOffset;11;0;Create;True;0;0;0;False;0;False;1,0;10,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;54;-1379.343,-728.0444;Inherit;False;Property;_ColorLerpScale;ColorLerpScale;20;0;Create;True;0;0;0;False;0;False;1;0.854;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-1328.743,-1091.632;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;105;-963.6294,400.713;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;47;-1010.12,143.6605;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-1387.636,-565.4277;Inherit;False;Property;_ColorLerpOffset;ColorLerpOffset;21;0;Create;True;0;0;0;False;0;False;0;-0.106;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;112;-726.9996,187.0591;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;87;-900.4006,-289.3215;Inherit;False;Property;_Color1Small;Color1Small;25;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.7571645,0.7830188,0.7810798,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;53;-978.4224,-598.2291;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;88;-897.4088,-72.17755;Inherit;False;Property;_Color1Big;Color1Big;27;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;113;-830.5043,550.5016;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;110;-799.4644,252.6045;Inherit;False;Property;_Color2Small;Color2Small;26;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.4716981,0.4716981,0.4716981,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;115;-219.1112,434.2234;Inherit;False;Property;_DepthFadeDistance;Depth Fade Distance;7;0;Create;True;0;0;0;False;0;False;0.5;0.120588;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;109;-780.9899,407.6486;Inherit;False;Property;_Color2BIg;Color2BIg;28;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.7988608,0.8791674,0.9056604,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;51;-884.2529,-996.5638;Inherit;False;Property;_BottomColor;BottomColor;19;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.608624,0.9150943,0.8438221,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;45;-881.1765,-806.363;Inherit;False;Property;_TopColor;TopColor;18;0;Create;True;0;0;0;False;2;Header(_____Set Color_____);Space(10);False;1,1,1,1;0.3643643,0.6925554,0.8679245,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;114;85.29501,402.2906;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;107;-514.9866,353.5584;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;122;254.1332,1352.611;Inherit;False;Property;_FresnelPower;Fresnel Power;32;0;Create;True;0;0;0;False;0;False;1;0.21;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;176.1871,1248.912;Inherit;False;Property;_FresnelScale;Fresnel Scale;29;0;Create;True;0;0;0;False;0;False;1;0.26;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;86;-515.8755,147.6829;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;52;-412.2529,-632.5638;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;127;131.6167,1157.381;Inherit;False;Property;_FresnelBias;FresnelBias;31;0;Create;True;0;0;0;False;0;False;0.2;0.202;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;123;440.6782,1086.444;Inherit;True;Standard;WorldNormal;ViewDir;True;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1017.495,644.3699;Inherit;False;Property;_ColorNoiseScale;ColorNoiseScale;16;0;Create;True;0;0;0;False;2;Header(_____NoiseColor Intensity_____);Space(10);False;1;5.87;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;116;364.3234,428.1695;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-157.262,-309.0708;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1016.704,760.938;Inherit;False;Property;_ColorNoiseOffset;ColorNoiseOffset;17;0;Create;True;0;0;0;False;0;False;0;2;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;558.7613,1299.651;Inherit;False;Constant;_Float5;Float 5;6;0;Create;True;0;0;0;False;0;False;-0.03;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;125;748.561,1186.552;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;126;543.3159,407.156;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;118;135.6319,566.9238;Inherit;False;Property;_FresnelEdgeColor;FresnelEdgeColor;30;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.6468049,0.8050813,0.8962264,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-1373.945,1494.595;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;58;-520.1345,587.2099;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;63;-42.80177,-11.2183;Inherit;False;FLOAT3;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;858.3971,469.6624;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;157.3711,-12.39271;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;71;-1094.533,1535.245;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;119;764.5692,147.2612;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-701.6827,1861.729;Inherit;False;Property;_EdgeOffset;EdgeOffset;24;0;Create;True;0;0;0;False;0;False;0;-0.336;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;74;-766.448,1460.928;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-736.7828,1749.93;Inherit;False;Property;_EdgeScale;EdgeScale;23;0;Create;True;0;0;0;False;2;Header(_____ Edge Opacity_____);Space(10);False;1;1.849;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-309.1034,1150.109;Inherit;False;Property;_OpacityValue;OpacityValue;22;0;Create;True;0;0;0;False;2;Header(_____ Opacity_____);Space(10);False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;75;-271.4481,1477.828;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;-0.51;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;56;913.089,51.54538;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;515.2452,730.6367;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;1061.625,48.24418;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;1179.879,41.49343;Inherit;False;FLOAT4;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;79;2612.225,253.4568;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;2469.811,472.9857;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;78;2328.973,206.3864;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;83;1003.137,990.9946;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1655.309,72.7631;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Xuqi/WaterfallCenter;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;ps4;psp2;n3ds;wiiu;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;31;0;30;0
WireConnection;31;1;30;0
WireConnection;41;1;40;0
WireConnection;35;0;31;0
WireConnection;36;0;35;0
WireConnection;36;2;41;0
WireConnection;15;0;46;0
WireConnection;15;1;16;0
WireConnection;102;0;103;0
WireConnection;99;0;97;0
WireConnection;99;1;98;0
WireConnection;26;1;36;0
WireConnection;5;0;4;2
WireConnection;5;1;6;0
WireConnection;12;0;15;0
WireConnection;100;0;99;0
WireConnection;100;1;102;0
WireConnection;94;0;4;2
WireConnection;94;1;92;0
WireConnection;7;0;12;0
WireConnection;7;1;5;0
WireConnection;27;0;28;0
WireConnection;27;1;26;1
WireConnection;93;0;100;0
WireConnection;93;1;94;0
WireConnection;95;0;27;0
WireConnection;95;1;93;0
WireConnection;29;0;27;0
WireConnection;29;1;7;0
WireConnection;3;1;29;0
WireConnection;96;1;95;0
WireConnection;105;0;96;1
WireConnection;105;1;106;1
WireConnection;105;2;106;2
WireConnection;47;0;3;1
WireConnection;47;1;48;1
WireConnection;47;2;48;2
WireConnection;112;0;47;0
WireConnection;53;0;50;2
WireConnection;53;1;54;0
WireConnection;53;2;55;0
WireConnection;113;0;105;0
WireConnection;114;0;115;0
WireConnection;107;0;110;0
WireConnection;107;1;109;0
WireConnection;107;2;113;0
WireConnection;86;0;87;0
WireConnection;86;1;88;0
WireConnection;86;2;112;0
WireConnection;52;0;51;0
WireConnection;52;1;45;0
WireConnection;52;2;53;0
WireConnection;123;1;127;0
WireConnection;123;2;121;0
WireConnection;123;3;122;0
WireConnection;116;0;114;0
WireConnection;111;0;86;0
WireConnection;111;1;107;0
WireConnection;111;2;52;0
WireConnection;125;0;123;0
WireConnection;125;1;124;0
WireConnection;126;0;116;0
WireConnection;72;0;50;1
WireConnection;58;0;27;0
WireConnection;58;1;59;0
WireConnection;58;2;60;0
WireConnection;63;0;111;0
WireConnection;117;0;126;0
WireConnection;117;1;118;0
WireConnection;117;2;125;0
WireConnection;57;0;63;0
WireConnection;57;1;58;0
WireConnection;71;0;72;0
WireConnection;119;0;57;0
WireConnection;119;1;117;0
WireConnection;74;0;71;0
WireConnection;75;0;74;0
WireConnection;75;1;76;0
WireConnection;75;2;77;0
WireConnection;56;0;119;0
WireConnection;62;0;75;0
WireConnection;62;1;49;0
WireConnection;62;2;58;0
WireConnection;65;0;56;0
WireConnection;64;0;65;0
WireConnection;79;0;82;0
WireConnection;79;1;78;2
WireConnection;82;0;78;1
WireConnection;83;0;62;0
WireConnection;0;2;64;0
WireConnection;0;9;83;0
ASEEND*/
//CHKSM=8E3ADD19825D2FD2A9D239765BE4BDC09EFB7BB5