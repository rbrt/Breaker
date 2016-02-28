Shader "Custom/CharacterShader" {
	Properties {
		_Color1 ("Color1", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_Color3 ("Color3", Color) = (1,1,1,1)
		_Color4 ("Color4", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;
		fixed4 _Color4;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			if (dot(c.rgb, 1) > 2.5){
				o.Albedo = 0;
			}
			else if (c.r > .4){
				if (c.b > .4 && c.g < .4){
					// pink
					o.Albedo = _Color4.rgb;
				}
				else{
					// red
					o.Albedo = _Color1.rgb;
				}
			}
			else if (c.g > .4){
				// green
				o.Albedo = _Color2.rgb;
			}
			else if (c.b > .4){
				// blue
				o.Albedo = _Color3.rgb;
			}


			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
