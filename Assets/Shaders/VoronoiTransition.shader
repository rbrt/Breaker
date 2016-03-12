Shader "Unlit/VoronoiTransition"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TestTex1 ("Test1", 2D) = "white" {}
		_TestTex2 ("Test2", 2D) = "white" {}
		_Step ("Step", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Overlay" }
		LOD 100

		Pass
		{
			CGPROGRAM
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
			sampler2D _TestTex1;
			sampler2D _TestTex2;
			float4 _MainTex_ST;
			float _Step;

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
				float blend = 0;
				float2 testCoords = i.uv;
				testCoords.x += sin(col.r + _Time.w) * .1;
				testCoords.y += cos(col.g + _Time.w) * .1;

				float points = 50.0;

				for (int index = 0; index < points; index++){
					fixed4 sample = tex2D(_MainTex, half2(index * _SinTime.x * .05f / points, index / points));
					if (distance(testCoords, sample.xy) < max){
						max = distance(testCoords, sample.xy);
						// Get our blend value
						blend = lerp(sample, max, _Step) / max * (_Step * _Step);
					}
				}

				// Coords should deform then return to normal as zero goes to one
				float2 blendCoords;
				if (blend < .5){
					blendCoords = lerp(i.uv, testCoords, blend * 2);
				}
				else{
					blendCoords = lerp(testCoords, i.uv, (blend - .5) * 2);
				}

				blend = saturate(blend);
				//blend = floor(blend) / blend;
				//blend = fmod(blend, 1);

				col = lerp(tex2D(_TestTex1, blendCoords),
						   tex2D(_TestTex2, blendCoords),
						   blend);

				return col;
			}
			ENDCG
		}
	}
}
