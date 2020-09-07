Shader "Unlit/BlendingTexture_US"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SecondaryTex ("Second Texture", 2D) = "white" {}
		_LerpValue("Transition float", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM				// Allows talk between two languages: shader lab and nvidia C for graphics.
            #pragma vertex vert		// Define for the building function. --> How to build it (shape to build it in)
            #pragma fragment frag	// Define for coloring function.	 --> What it's gonna look like
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"	// Built in shader functions. Bunch of different functions we can use.

			// How the vertex function is gonna get its information (data)
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

			// How the fragment function gets its data
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			// Reinport property from shader lab to nvidia cg
            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _SecondaryTex;
			float4 _SecondaryTex_ST;	// its actually the tiling and the offset value

			float _LerpValue;

			// With this method we can modify the position of vertex, for wxample. 
			// What goes into the fragment function
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);	// It takes the object from the object space ad put it into the camera clip space. Basically a macro to take the object coordinates we have in object space and match it to my sceen pixel.
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			// Just to go inside of the fragment shader or the pixel shader.
			// Color it in
			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture													We change our value for a _SinTime, so we dont control it through other script
				fixed4 col = lerp(tex2D(_MainTex, i.uv), tex2D(_SecondaryTex, i.uv), _SinTime.w); // _SinTime is a float4, and we only need his 4th value
				    

                return col;
            }
            ENDCG
        }
    }
}
