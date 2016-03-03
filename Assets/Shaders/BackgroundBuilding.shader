﻿Shader "Custom/BackgroundBuilding" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_WindowColor ("WindowColor", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma vertex vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#include "UnityCG.cginc"

		struct Input {
			float2 uv_MainTex: TEXCOORD0;
			float4 vertex: SV_POSITION;
			float window;
		};

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _WindowColor;

		void vert (inout appdata_base v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);

			o.window = 0;

			if (v.vertex.x < .82){
				if (v.vertex.x > -.82){
					if (v.vertex.y < .82){
						if (v.vertex.y > -.82){
							o.window = 1;
						}
					}
				}
			}

			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      	}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			if (IN.window > 0){
				c = _WindowColor;
			}

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
