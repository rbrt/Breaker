Shader "Unlit/VoronoiTransition"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, Xbox360, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 xbox360 gles
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float max = 100;
				float2 testCoords = i.uv;
				testCoords.x += sin(col.r + _Time.w) * .1;
				testCoords.y += cos(col.g + _Time.w) * .1;

				if (fmod(cos(testCoords.y) + sin(testCoords.x), .3) > .15){
					testCoords.x += (sin(testCoords.y) + 1) / 2 * (_SinTime.w) * (fmod(testCoords.y, .3) / .3);
				}

				for (int index = 0; index < 50; index++){
					fixed4 sample = tex2D(_MainTex, half2(index * _SinTime.x * .05f / 50.0, index / 50.0));
					if (distance(testCoords, sample.xy) < max){
						max = distance(testCoords, sample.xy);
						col = sample / max * .075;
						//col = sample;
					}
				}
				return col;
			}
			ENDCG
		}
	}
}
