Shader "Custom/My First Shader" {

	Properties {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader {

//		Pass {
//			CGPROGRAM
//
//			#pragma vertex MyVertexProgram
//			#pragma fragment MyFragmentProgram
//
//			#include "UnityCG.cginc"
//
//			sampler2D _rt0;
//			float4 _rt0_ST;
//
//			struct VertexData {
//				float4 position : POSITION;
//				float2 uv : TEXCOORD0;
//			};
//
//			struct Interpolators {
//				float4 position : SV_POSITION;
//				float2 uv : TEXCOORD0;
//			};
//
//			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
//
////			void mainImage( out vec4 fragColor, in vec2 fragCoord ) {
//			   float3 e = float3(float2(1.,1.)/_ScreenParams.xy,0.);
////			   vec2 q = fragCoord.xy/_ScreenParams.xy;
//			   float2 q = i.uv.xy;
//
//			   float4 c = tex2D(_rt0, q);
//			   
//			   float p11 = c.x;
//			   
//			   float p10 = tex2D(_rt0, q-e.zy).x;
//			   float p01 = tex2D(_rt0, q-e.xz).x;
//			   float p21 = tex2D(_rt0, q+e.xz).x;
//			   float p12 = tex2D(_rt0, q+e.zy).x;
//			   
//			   float d = 0.;
//
//			   // Mouse interaction:
////			   d = smoothstep(4.5,.5,length(iMouse.xy - i.uv.xy));
//
//
//			   // The actual propagation:
//			   d += -(p11-.5)*2. + (p10 + p01 + p21 + p12 - 2.);
//			   d *= .99; // dampening
////			   d *= min(1.,float(iFrame)); // clear the buffer at iFrame == 0
//			   d = d*.5 + .5;
//			   
//			   return float4(d, 0, 0, 0);
//			}
//
//
//			ENDCG
//		}
		Pass {
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = mul(UNITY_MATRIX_MVP, v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				return tex2D(_MainTex, i.uv) / 2;
			}
//			    fragColor = vec4(c,1.);


			ENDCG
		}
	}
}