Shader "Unlit/Outline(ObjBig)"	//The way we're doing this is creating the same object but bigger as an outline and making sure we haver the original object always in front of the ouline one
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}

		_OutlineTex("Outline Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(1.0, 2.0)) = 1.1
    }

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : POSITION;
		float3 normal : NORMAL;
	};

	float4 _OutlineColor;
	float _OutlineWidth;

	v2f vert(appdata v)
	{
		v.vertex.xyz *= _OutlineWidth;	//Multip for the normal

		v2f o;
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
	}

	ENDCG

	SubShader
	{
		Tags{"Queue" = "Transparent"}
		Pass	// For render the Outline
		{
			Name "OUTLINE"

			ZWrite Off //Now we are rendering this one in the depth buffer

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// half is a data type. If float is high precision, half is medium: only 3 decimal digit precision.
			half4 frag(v2f i) : COLOR
			{
				v2f o;
				return _OutlineColor;	
			}

			ENDCG
		}

        Pass	// For normal render
        {
			Name "OBJECT"

            ZWrite On	//On the visible buffer
						//More info on ShaderLab sintax documentation
			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}

			Lighting On	// Cus we are on an Unlit Shader

			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}
        }
    }
}
