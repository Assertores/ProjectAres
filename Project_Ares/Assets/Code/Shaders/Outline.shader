
Shader "Custom/Outline"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "black" {}
		_SceneTex("Scene Texture", 2D) = "black" {}
		_OutlineColor("Outline Color", Color) = (1,0.5,0,1)
	}

		SubShader
		{
			//Blend SrcAlpha OneMinusSrcAlpha
			Pass
			{
				CGPROGRAM

				sampler2D _MainTex;
				half4 _OutlineColor;

				float2 _MainTex_TexelSize;
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct vertdata
				{
					float2 uv : TEXCOORD0;
					float2 uvScaled : TEXCOORD1;
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uvs : TEXCOORD0;
				};

				v2f vert(appdata_base v)
				{
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);

					o.uvs = o.pos.xy / 2 + 0.5;

					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					float TX_x = _MainTex_TexelSize.x;

					float ColorIntesityInRadius;

					for (int k = 0; k < 8; k += 1)
					{
						ColorIntesityInRadius += tex2D(_MainTex, i.uvs.xy + float2((k - 8 / 2) * TX_x, 0)).r / 8;
					}
					return ColorIntesityInRadius;
				}

				ENDCG
			}

			GrabPass{}

			Pass
			{
				CGPROGRAM

				sampler2D _MainTex;
				sampler2D _SceneTex;
				half4 _OutlineColor;

				sampler2D _GrabTexture;

				float2 _GrabTexture_TexelSize;

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct vertdata
				{
					float2 uv : TEXCOORD0;
					float2 uvScaled : TEXCOORD1;
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uvs : TEXCOORD0;
				};

				v2f vert(appdata_base v)
				{
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);

					o.uvs = o.pos.xy / 2 + 0.5;

					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					float TX_y = _GrabTexture_TexelSize.y;

					half ColorIntesityInRadius = 0;

					if (tex2D(_MainTex,float2(i.uvs.x, 1 - i.uvs.y)).r > 0.5)
					{
						return tex2D(_SceneTex,float2(i.uvs.x, 1 - i.uvs.y));
					}


					for (int j = 0; j < 8; j += 1)
					{
						ColorIntesityInRadius += tex2D(_GrabTexture, float2(i.uvs.x,i.uvs.y) + float2(0,(j - 8 / 2)*TX_y)).r / 8;
					}

					half4 outColor = ColorIntesityInRadius * _OutlineColor * 2 + (1 - ColorIntesityInRadius) * tex2D(_SceneTex,float2(i.uvs.x,1 - i.uvs.y));
					return outColor;
				}
				ENDCG
			}
		}
}