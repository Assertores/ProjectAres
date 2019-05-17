Shader "Sprites/Outline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Color("Tint", Color) = (1,1,1,1)
		_OutlineThickness("Outline Thickness", Int) = 2
		_OutlineColor("Outline Color", Color) = (1,0,0,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
				_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5

	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "TransparentCutOut"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
				"DisableBatching" = "True"

			}
			LOD 300

			// Render back faces first
			Cull Off//Front
			Lighting On
			//ZWrite Off
			Fog { Mode Off }

			CGPROGRAM
			#pragma surface surf Lambert alpha vertex:vert addshadow alphatest:_Cutoff 
			#pragma multi_compile DUMMY PIXELSNAP_ON 
			#pragma target 3.0
			

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			sampler2D _BumpMap;
			fixed4 _Color;
			float _ScaleX;
			float _OutlineThickness;
			fixed4 _OutlineColor;

			float Outline(float2 uv) {

				if (tex2D(_MainTex, float2(uv.x + _OutlineThickness * _MainTex_TexelSize.x, uv.y)).a < 0.5) return 1.0;
				if (tex2D(_MainTex, float2(uv.x - _OutlineThickness * _MainTex_TexelSize.x, uv.y)).a < 0.5) return 1.0;
				if (tex2D(_MainTex, float2(uv.x, uv.y + _OutlineThickness * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
				if (tex2D(_MainTex, float2(uv.x, uv.y - _OutlineThickness * _MainTex_TexelSize.y)).a < 0.5) return 1.0;

				[loop]
				for (int i = 1; i < _OutlineThickness; i++) {
					if (tex2D(_MainTex, float2(uv.x + _OutlineThickness * _MainTex_TexelSize.x, uv.y + i * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
					if (tex2D(_MainTex, float2(uv.x - _OutlineThickness * _MainTex_TexelSize.x, uv.y + i * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
					if (tex2D(_MainTex, float2(uv.x + _OutlineThickness * _MainTex_TexelSize.x, uv.y - i * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
					if (tex2D(_MainTex, float2(uv.x - _OutlineThickness * _MainTex_TexelSize.x, uv.y - i * _MainTex_TexelSize.y)).a < 0.5) return 1.0;

					if (tex2D(_MainTex, float2(uv.x + i * _MainTex_TexelSize.x, uv.y + _OutlineThickness * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
					if (tex2D(_MainTex, float2(uv.x + i * _MainTex_TexelSize.x, uv.y - _OutlineThickness * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
					if (tex2D(_MainTex, float2(uv.x - i * _MainTex_TexelSize.x, uv.y + _OutlineThickness * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
					if (tex2D(_MainTex, float2(uv.x - i * _MainTex_TexelSize.x, uv.y - _OutlineThickness * _MainTex_TexelSize.y)).a < 0.5) return 1.0;
				}
				return 0.0;
			}

			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				fixed4 color : COLOR;
				float face : VFACE;
			};

			void vert(inout appdata_full v, out Input o) {
				#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
				v.vertex = UnityPixelSnap(v.vertex);
				#endif

				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.color += _Color;
			}

			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) /** IN.color*/;
				o.Alpha = c.a;
				o.Albedo = lerp(c.rgb, _OutlineColor.rgb, Outline(IN.uv_MainTex));
				
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Normal *= lerp(-1, 1, step(0.5, IN.face));
			}
			ENDCG
		}

			Fallback "Transparent/Cutout/Diffuse"
}