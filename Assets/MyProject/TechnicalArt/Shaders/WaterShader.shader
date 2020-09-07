// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterShader"
{
	Properties
	{
		_Color0("Color 0", Color) = (0.2906728,0.6355275,0.8679245,0)
		_Smothness("Smothness", Float) = 0
		_DepthColor("Depth Color", Color) = (0.5562578,0.2196078,0.7450981,0)
		_ColorDepth("Color Depth", Float) = 0
		_FresnelColor("Fresnel Color", Color) = (0.360048,0.7555727,0.8207547,0)
		_FresnelPower("FresnelPower", Float) = 0
		_FoamColor("Foam Color", Color) = (1,1,1,0)
		_FoamDepth_("Foam Depth_", Float) = 0
		_FoamTexture("Foam Texture", 2D) = "white" {}
		_FoamSpeed("Foam Speed", Float) = 0
		_Foamtiling("Foam tiling", Float) = 0
		_Opacity("Opacity", Float) = 0
		_OpacityDepth("Opacity Depth", Float) = 0
		_Normal("Normal", 2D) = "white" {}
		_Normal2("Normal 2", 2D) = "white" {}
		_NormalIntensity("Normal Intensity", Float) = 0
		_NormalIntensity2("Normal Intensity 2", Float) = 0
		_NormalTiling("Normal Tiling", Float) = 0
		_NormalTiling2("Normal Tiling 2", Float) = 0
		_NormalSpeed("Normal Speed", Float) = 0
		_NormalSpeed2("Normal Speed 2", Float) = 0
		_WaveSpeed("Wave Speed", Float) = 0
		_WaveIntensity("Wave Intensity ", Float) = 0
		_WaveFrecuency("Wave Frecuency", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _WaveIntensity;
		uniform float _WaveSpeed;
		uniform float _WaveFrecuency;
		uniform float _NormalIntensity;
		uniform sampler2D _Normal;
		uniform float _NormalSpeed;
		uniform float _NormalTiling;
		uniform float _NormalIntensity2;
		uniform sampler2D _Normal2;
		uniform float _NormalSpeed2;
		uniform float _NormalTiling2;
		uniform float4 _FoamColor;
		uniform float4 _Color0;
		uniform float4 _DepthColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _ColorDepth;
		uniform float4 _FresnelColor;
		uniform float _FresnelPower;
		uniform float _FoamDepth_;
		uniform sampler2D _FoamTexture;
		uniform float _FoamSpeed;
		uniform float _Foamtiling;
		uniform float _Smothness;
		uniform float _Opacity;
		uniform float _OpacityDepth;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float mulTime57 = _Time.y * _WaveSpeed;
			v.vertex.xyz += ( _WaveIntensity * ase_vertexNormal * float3( sin( ( mulTime57 + ( v.texcoord.xy * _WaveFrecuency ) ) ) ,  0.0 ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float mulTime45 = _Time.y * _NormalSpeed;
			float2 temp_cast_0 = (_NormalTiling).xx;
			float2 uv_TexCoord40 = i.uv_texcoord * temp_cast_0;
			float2 panner41 = ( mulTime45 * float2( 1,0 ) + uv_TexCoord40);
			float mulTime50 = _Time.y * _NormalSpeed2;
			float2 temp_cast_1 = (_NormalTiling2).xx;
			float2 uv_TexCoord49 = i.uv_texcoord * temp_cast_1;
			float2 panner47 = ( mulTime50 * float2( 1,0 ) + uv_TexCoord49);
			o.Normal = BlendNormals( UnpackScaleNormal( tex2D( _Normal, panner41 ), _NormalIntensity ) , UnpackScaleNormal( tex2D( _Normal2, panner47 ), _NormalIntensity2 ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth5 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth5 = abs( ( screenDepth5 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _ColorDepth ) );
			float clampResult8 = clamp( distanceDepth5 , 0.0 , 1.0 );
			float4 lerpResult6 = lerp( _Color0 , _DepthColor , clampResult8);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV13 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode13 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV13, _FresnelPower ) );
			float clampResult12 = clamp( fresnelNode13 , 0.0 , 1.0 );
			float4 lerpResult11 = lerp( lerpResult6 , _FresnelColor , clampResult12);
			float mulTime28 = _Time.y * _FoamSpeed;
			float2 temp_cast_2 = (_Foamtiling).xx;
			float2 uv_TexCoord25 = i.uv_texcoord * temp_cast_2;
			float2 panner24 = ( mulTime28 * float2( 1,0 ) + uv_TexCoord25);
			float screenDepth20 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth20 = abs( ( screenDepth20 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( ( _FoamDepth_ * tex2D( _FoamTexture, panner24 ) ).r ) );
			float clampResult21 = clamp( distanceDepth20 , 0.0 , 1.0 );
			float4 lerpResult18 = lerp( _FoamColor , lerpResult11 , clampResult21);
			o.Albedo = lerpResult18.rgb;
			o.Smoothness = _Smothness;
			float screenDepth32 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth32 = abs( ( screenDepth32 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _OpacityDepth ) );
			float clampResult33 = clamp( distanceDepth32 , 0.0 , 1.0 );
			float lerpResult30 = lerp( _Opacity , 1.0 , clampResult33);
			float clampResult36 = clamp( ( ( 1.0 - clampResult21 ) + lerpResult30 ) , 0.0 , 1.0 );
			o.Alpha = clampResult36;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17600
830;73;580;740;-271.8779;214.9552;1.588869;True;False
Node;AmplifyShaderEditor.RangedFloatNode;26;-1091.162,341.0574;Inherit;False;Property;_Foamtiling;Foam tiling;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-900.3231,487.4654;Inherit;False;Property;_FoamSpeed;Foam Speed;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;28;-654.4684,479.1781;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-789.8272,289.9528;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;24;-470.7679,299.6212;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-226.5253,129.9956;Inherit;False;Property;_FoamDepth_;Foam Depth_;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-204.195,265.0911;Inherit;True;Property;_FoamTexture;Foam Texture;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;11.27341,120.0642;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;207.509,1247.195;Inherit;False;Property;_OpacityDepth;Opacity Depth;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-778.7264,-245.3811;Inherit;False;Property;_ColorDepth;Color Depth;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-699.4461,41.38696;Inherit;False;Property;_FresnelPower;FresnelPower;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;391.9096,86.99426;Inherit;False;Property;_NormalTiling;Normal Tiling;17;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;903.5386,729.7267;Inherit;False;Property;_WaveFrecuency;Wave Frecuency;23;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;918.2153,629.4059;Inherit;False;Property;_WaveSpeed;Wave Speed;21;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;850.7859,777.2815;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;20;145.66,36.34076;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;372.1145,211.6943;Inherit;False;Property;_NormalSpeed;Normal Speed;19;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;256.8152,432.9804;Inherit;False;Property;_NormalTiling2;Normal Tiling 2;18;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;237.0201,557.6805;Inherit;False;Property;_NormalSpeed2;Normal Speed 2;20;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;32;417.4525,1238.908;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;5;-565.9055,-238.4719;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;45;603.4453,109.7112;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;550.048,1124.268;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;57;1087.191,675.0091;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;13;-484.1565,-44.39264;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;50;468.3511,455.6973;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;8;-296.8427,-257.1203;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-558.9957,-416.7436;Inherit;False;Property;_DepthColor;Depth Color;2;0;Create;True;0;0;False;0;0.5562578,0.2196078,0.7450981,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;40;587.8568,-13.18407;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;535.199,1041.161;Inherit;False;Property;_Opacity;Opacity;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;21;364.3139,-81.39613;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-566.4884,-600.0034;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;False;0;0.2906728,0.6355275,0.8679245,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;452.7626,332.802;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;1090.786,825.2815;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;10;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;33;681.2629,1211.284;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-134.1809,-199.1322;Inherit;False;Property;_FresnelColor;Fresnel Color;4;0;Create;True;0;0;False;0;0.360048,0.7555727,0.8207547,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;37;804.1617,915.8231;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;12;-41.80251,-7.389702;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;6;-107.3969,-330.5112;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;41;882.7534,-45.98104;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;48;739.8044,433.3235;Inherit;False;Property;_NormalIntensity2;Normal Intensity 2;16;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;30;863.5822,1045.539;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;1298.786,729.2815;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;47;747.6591,300.005;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;39;874.8987,87.33733;Inherit;False;Property;_NormalIntensity;Normal Intensity;15;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;56;1489.464,572.4065;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;1467.533,403.7822;Inherit;False;Property;_WaveIntensity;Wave Intensity ;22;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;59;1490.786,713.2815;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;46;953.2294,322.1027;Inherit;True;Property;_Normal2;Normal 2;14;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;11;182.026,-242.8633;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;1010.96,980.002;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;154.986,-426.1961;Inherit;False;Property;_FoamColor;Foam Color;6;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;38;1088.324,-23.88346;Inherit;True;Property;_Normal;Normal;13;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;18;776.043,-184.3938;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;1513.697,298.8147;Inherit;False;Property;_Smothness;Smothness;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;53;1476.127,158.777;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;1708.299,491.9698;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;36;1105.446,1086.967;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1874.512,25.91032;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;27;0
WireConnection;25;0;26;0
WireConnection;24;0;25;0
WireConnection;24;1;28;0
WireConnection;22;1;24;0
WireConnection;23;0;19;0
WireConnection;23;1;22;0
WireConnection;20;0;23;0
WireConnection;32;0;34;0
WireConnection;5;0;9;0
WireConnection;45;0;43;0
WireConnection;57;0;60;0
WireConnection;13;3;14;0
WireConnection;50;0;52;0
WireConnection;8;0;5;0
WireConnection;40;0;42;0
WireConnection;21;0;20;0
WireConnection;49;0;51;0
WireConnection;62;0;61;0
WireConnection;62;1;65;0
WireConnection;33;0;32;0
WireConnection;37;0;21;0
WireConnection;12;0;13;0
WireConnection;6;0;2;0
WireConnection;6;1;4;0
WireConnection;6;2;8;0
WireConnection;41;0;40;0
WireConnection;41;1;45;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;30;2;33;0
WireConnection;64;0;57;0
WireConnection;64;1;62;0
WireConnection;47;0;49;0
WireConnection;47;1;50;0
WireConnection;59;0;64;0
WireConnection;46;1;47;0
WireConnection;46;5;48;0
WireConnection;11;0;6;0
WireConnection;11;1;10;0
WireConnection;11;2;12;0
WireConnection;35;0;37;0
WireConnection;35;1;30;0
WireConnection;38;1;41;0
WireConnection;38;5;39;0
WireConnection;18;0;17;0
WireConnection;18;1;11;0
WireConnection;18;2;21;0
WireConnection;53;0;38;0
WireConnection;53;1;46;0
WireConnection;55;0;54;0
WireConnection;55;1;56;0
WireConnection;55;2;59;0
WireConnection;36;0;35;0
WireConnection;0;0;18;0
WireConnection;0;1;53;0
WireConnection;0;4;3;0
WireConnection;0;9;36;0
WireConnection;0;11;55;0
ASEEND*/
//CHKSM=FCECCD6DA84B5354474D76FAC249CC6B21C48E87