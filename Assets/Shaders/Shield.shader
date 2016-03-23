Shader "Custom/Shield" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200

		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert alpha:blend
		#pragma vertex vert

		#include "UnityCG.cginc"

		struct Input {
			float2 uv_MainTex: TEXCOORD0;
			float4 vertex: SV_POSITION;
			float yPos: TEXCOORD1;
		};

		sampler2D _MainTex;
		fixed4 _Color;

		void vert (inout appdata_base v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);

			v.vertex.xyz += v.normal * .02 * (sin(_Time.y * 10 + v.vertex.x - v.vertex.z * v.vertex.y));
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.yPos = v.vertex.y;
      	}

		void surf (Input IN, inout SurfaceOutput o) {
			float2 uv = IN.uv_MainTex;
			float width = .5;

			uv.y += _Time.x * 5;

			float4 c = _Color;

			o.Albedo = c.rgb;
			o.Alpha = abs((fmod(uv.y * 5, width) * 2) - width) * _Color.a * 2;
		}
		ENDCG
	}
}
