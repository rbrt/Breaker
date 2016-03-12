Shader "Unlit/VoronoiVisualizer"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TestTex1 ("Test1", 2D) = "white" {}
		_TestTex2 ("Test2", 2D) = "white" {}
		_Step ("Step", Range(0,1)) = 0
		_Param1 ("Param1", Range(0,1)) = 0
		_Param2 ("Param1", Range(0,1)) = 0
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
			sampler2D _TestTex1;
			sampler2D _TestTex2;
			float4 _MainTex_ST;
			float _Step;
			float _Param1;
			float _Param2;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half2 initialCoords = i.uv;
				/*initialCoords.x += _Param1;
				initialCoords.y += _Param2;*/
				fixed4 col = tex2D(_MainTex, initialCoords);
				float max = 100;
				float blend = 0;
				float2 testCoords = i.uv;
				testCoords.x += sin(col.r + _Time.w) * .1;
				testCoords.y += cos(col.g + _Time.w) * .1;

				float points = 30.0;

				for (int index = 0; index < points; index++){
					fixed4 sample = tex2D(_MainTex, half2(index * _SinTime.x * .05f / points, index / points));
					if (distance(testCoords, sample.xy) < max){
						max = distance(testCoords, sample.xy);
						col = sample / max * .07;
					}
				}

				return col;
			}
			ENDCG
		}
	}
}
