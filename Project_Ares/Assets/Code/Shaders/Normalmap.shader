﻿Shader "Sprites/Bumped Diffuse both sides with Shadows"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Color("Tint", Color) = (1,1,1,1)
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
			sampler2D _BumpMap;
			fixed4 _Color;
			float _ScaleX;

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
				// float3 normal = v.normal;
				//v.normal = float3(0,0,-1);
				//v.tangent = float4(1, 0, 0, 1);

				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.color += _Color;
			}

			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Normal *= lerp(-1, 1, step(0.5, IN.face));
			}
			ENDCG

		//	// Now render front faces first
		//	Cull Back
		//	Lighting On
		//	ZWrite Off
		//	Fog { Mode Off }

		//	CGPROGRAM
		//	#pragma surface surf Lambert alpha vertex:vert addshadow alphatest:_Cutoff 
		//	#pragma multi_compile DUMMY PIXELSNAP_ON 

		//	sampler2D _MainTex;
		//	sampler2D _BumpMap;
		//	fixed4 _Color;
		//	float _ScaleX;

		//	struct Input {
		//		float2 uv_MainTex;
		//		float2 uv_BumpMap;
		//		fixed4 color : COLOR;
		//	};

		//	void vert(inout appdata_full v, out Input o) {
		//		#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
		//		v.vertex = UnityPixelSnap(v.vertex);
		//		#endif
		//		float3 normal = v.normal;

		//		v.normal = float3(0,0,1);
		//		v.tangent = float4(1, 0, 0, 1);

		//		UNITY_INITIALIZE_OUTPUT(Input, o);
		//		o.color += _Color;
		//	}

		//	void surf(Input IN, inout SurfaceOutput o) {
		//		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
		//		o.Albedo = c.rgb;
		//		o.Alpha = c.a;
		//		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));					
		//		o.Normal.z *= -1;
		//		o.Normal.x *= -1;
		//	}
		//	ENDCG
		}

			Fallback "Transparent/Cutout/Diffuse"
}