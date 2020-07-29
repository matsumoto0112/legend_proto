Shader "toon"
{
	Properties
	{
		_MainTex("MainTexture", 2D) = "white" {}
		_RampTex("RampTexture", 2D) = "white" {}
		_Color("Color",COLOR) = (1,1,1,1)
		_LightColor("_LightColor",COLOR) = (0,1,1,1)
	}

		SubShader
	{
		Tags {"RenderType" = "Opaque" }
		LOD 100

		// Pass to render object without lighting and shading
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 uv : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float3 normal : NORMAL;
			float3 worldNormal : TEXCOORD1;
			float2 uv : TEXCOORD0;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			o.normal = v.normal;
			o.uv = v.uv;
			return o;
		}

		sampler2D _MainTex;
		float4 _MainTex_ST;

		sampler2D _RampTex;
		float4 _RampTex_ST;

		float4 _Color, _LightColor;

		fixed4 frag(v2f i) : SV_Target
		{
			float4 inLightDir = mul(UNITY_MATRIX_M, _WorldSpaceLightPos0);
			float intensity = (dot(normalize(i.normal), inLightDir.xyz));
			float3 normal = normalize(i.worldNormal);

			fixed4 texCol = (tex2D(_MainTex, i.uv) * _Color);
			float4 color = float4(texCol.rgb * tex2D(_RampTex,float2(intensity,0.5)).rgb, 1);
			//float4 colDay = saturate(lerp(color, (color - (1.0 - _LightColor)), 1));

			return color;
		}
		ENDCG
	}

		// Pass to render object as a shadow caster
		Pass
		{
			Name "CastShadow"
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}