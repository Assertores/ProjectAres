Shader "Sprites/CellShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		_CSLight("Cell shading Light", Range(-1,1)) = 1
		_CSShadow("Cell shading Shadow", Range(-1,1)) = 0
		_CSEdge("Cell shading Edge", Range(0,1)) = 0.5
		_CSEdgeSmooth("Cell shading Edge Smoothness", Range(0,0.2)) = 0.1

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

			}
			LOD 300

			// Render back faces first
			Cull Front
			Lighting On
			ZWrite Off
			Fog { Mode Off }

			CGPROGRAM
			#pragma surface surf SimpleLambert alpha vertex:vert addshadow alphatest:_Cutoff 
			#pragma multi_compile DUMMY PIXELSNAP_ON 

			sampler2D _MainTex;
			sampler2D _BumpMap;
			fixed4 _Color;
			float _ScaleX;
			float _CSLight;
			float _CSShadow;
			float _CSEdge;
			float _CSEdgeSmooth;

			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				fixed4 color : COLOR;
			};

			void vert(inout appdata_full v, out Input o) {
				#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
				v.vertex = UnityPixelSnap(v.vertex);
				#endif
				float3 normal = v.normal;

				v.normal = float3(0,0,-1);
				v.tangent = float4(1, 0, 0, 1);

				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.color += _Color;
			}
			
			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Normal.x *= -1;
			}

			half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
				half NdotL = dot(s.Normal, lightDir);
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * lerp(_CSShadow, _CSLight, smoothstep(_CSEdge - _CSEdgeSmooth, _CSEdge + _CSEdgeSmooth, NdotL * atten));
				c.a = s.Alpha;
				return c;
			}

			ENDCG

			// Now render front faces first
			Cull Back
			Lighting On
			ZWrite Off
			Fog { Mode Off }

			CGPROGRAM
			#pragma surface surf SimpleLambert alpha vertex:vert addshadow alphatest:_Cutoff 
			#pragma multi_compile DUMMY PIXELSNAP_ON 

			sampler2D _MainTex;
			sampler2D _BumpMap;
			fixed4 _Color;
			float _ScaleX;
			float _CSLight;
			float _CSShadow;
			float _CSEdge;
			float _CSEdgeSmooth;

			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				fixed4 color : COLOR;
			};

			void vert(inout appdata_full v, out Input o) {
				#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
				v.vertex = UnityPixelSnap(v.vertex);
				#endif
				float3 normal = v.normal;

				v.normal = float3(0,0,1);
				v.tangent = float4(1, 0, 0, 1);

				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.color += _Color;
			}
			
			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Normal.z *= -1;
				o.Normal.x *= -1;
			}

			half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
				half NdotL = dot(s.Normal, lightDir);
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * lerp(_CSShadow, _CSLight, smoothstep(_CSEdge - _CSEdgeSmooth, _CSEdge + _CSEdgeSmooth, NdotL * atten));
				c.a = s.Alpha;
				return c;
			}

			ENDCG
		}

			Fallback "Transparent/Cutout/Diffuse"
}