Shader "Unlit/Transparent Colored"
{
	Properties
	{
		_MainTex("Base (RGB) Gray, Alpha (A)", 2D) = "black" {}
	}

		SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
			{
				Cull Off
				Lighting Off
				ZWrite Off
				Fog{ Mode Off }
				Offset -1, -1
				ColorMask RGB
				AlphaTest Greater .01
				Blend SrcAlpha OneMinusSrcAlpha
				ColorMaterial AmbientAndDiffuse

				CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"

				sampler2D _MainTex;
				fixed4 _Color;

				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					fixed gray : TEXCOORD1;
				};

				float4 _MainTex_ST;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.color = v.color;
					o.gray = dot(v.color, fixed4(1,1,1,0));
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
				
				fixed4 frag(v2f i) : COLOR
				{
					fixed4 col;
					if (i.gray == 0)
					{
						col = tex2D(_MainTex, i.texcoord);
						//col.rgb = dot(col.rgb, fixed3(.5, .5, .5));
						col.rgb = dot(col.rgb, fixed3(.222,.707,.071));
						//col.rgb = dot(col.rgb, fixed3(0.299, 0.587, 0.114));
					}
					else
					{
						col = tex2D(_MainTex, i.texcoord) * i.color;
					}
					return col;
				}
					ENDCG
			}
	}
}