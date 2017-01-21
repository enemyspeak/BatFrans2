Shader "Custom/wave" {

	Properties {
	    _SourcePosition ("SourcePosition", Vector) = (0,0,0,1)
		_rt0 ("Texture", 2D) = "white" {}
		_rt1 ("RenderTexture", 2D) = "white" {}
	}

	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
		
			#include "UnityCG.cginc"

			uniform float4 _SourcePosition;
			uniform sampler2D _rt0;
			uniform sampler2D _rt1;

			float4 frag(v2f_img i) : COLOR {
				float3 e = float3(1.,1.,0.);
				float2 q = i.uv.xy/_ScreenParams.xy;
				float2 g = _ScreenParams.xy;
				float4 c = tex2D(_rt0, q);

				float p11 = c.x;

				float p10 = tex2D(_rt1, q-e.zy).x;
				float p01 = tex2D(_rt1, q-e.xz).x;
				float p21 = tex2D(_rt1, q+e.xz).x;
				float p12 = tex2D(_rt1, q+e.zy).x;

				float d = 0.;

//				if (iMouse.z > 0.) 
//				{
//					d = smoothstep(4.5,.5,length(float2(0.0,0.0) - i.uv.xy));
//				}
//				else
//				{
					float t = _Time.y*2.;
					float2 pos = frac(floor(t)*float2(0.456665,0.708618))*_ScreenParams.xy;
					float amp = 1.-step(.05,frac(t));
					d = -amp*smoothstep(2.5,.5,length(pos - i.uv.xy));
//					d = smoothstep(2.5,.5,length(pos - i.uv.xy));
//				}

				d += -(p11-.5)*2. + (p10 + p01 + p21 + p12 - 2.);
				d *= .99; // dampening
				d *= min(1.,float(_Time.y/60)); // clear the buffer at iFrame == 0
				d = d*.5 + .5;

				return float4(d,0., 0., 0.);
			}
			ENDCG
		}
	}
}



//			   float3 e = float3(float2(1.0,1.0)/_ScreenParams.xy,0.0);
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
//			   d = smoothstep(4.5,0.5,length(float2(2.0,2.0) - i.uv.xy));
//
//
//			   // The actual propagation:
//			   d += -(p11-.5)*2. + (p10 + p01 + p21 + p12 - 2.);
//			   d *= .99; // dampening
////			   d *= min(1.,float(iFrame)); // clear the buffer at iFrame == 0
//			   d = d*.5 + .5;
//			   
//			   return float4(e, 1.0);
////			    float2 q = i.uv.xy/_ScreenParams.xy;
//				float2 q = i.uv.xy;
////			    return float4(i.uv.xy,1.,1.0);
//
//
//			    float h = tex2D(_rt0, q).x;
//
////			    return float4(c,1.0,1.0,1.0);
//
//
//			    float sh = 1.35 - h*2.;
//			    float3 c =
//			       float3(exp(pow(sh-.75,2.)*-10.),
//			            exp(pow(sh-.50,2.)*-20.),
//			            exp(pow(sh-.25,2.)*-10.));
//			    return float4(c,1.0);
//			    float3 e = float3(float2(1.0,1.0)/_ScreenParams.xy,0.0);
////
//			    float p10 = tex2D(_rt0, q-e.zy).x;
//			    float p01 = tex2D(_rt0, q-e.xz).x;
//			    float p21 = tex2D(_rt0, q+e.xz).x;
//			    float p12 = tex2D(_rt0, q+e.zy).x;
//
//			    return float4(p10,p01,p21,p12);
//
////			    // Totally fake displacement and shading:
//			    float3 grad = normalize(float3(p21 - p01, p12 - p10, 1.0));
//			    float4 c = tex2D(_rt0, i.uv.xy * 2.0 / _ScreenParams.xy); //+ grad.xy*.35);
//			    float3 light = normalize(float3(0.2,-0.5,0.7));
//			    float diffuse2 = dot(grad,light);
//			    float spec = pow(max(0.0,-reflect(light,grad).z),32.0);
//			    return float4(e,1.0);// lerp(c,float4(0.7,0.8,1.0,1.0),0.25)*max(diffuse2,0.0) + spec;			   
//			}
//
//			ENDCG
//		}
//	}
//}
//
//	Properties {
//		_Color ("Color", Color) = (1,1,1,1)
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
//	}
//	SubShader {
////		Tags { "RenderType"="Opaque" }
////		LOD 200
////		
////		CGPROGRAM
////		// Physically based Standard lighting model, and enable shadows on all light types
////		#pragma surface surf Standard fullforwardshadows
////
////		// Use shader model 3.0 target, to get nicer looking lighting
////		#pragma target 3.0
////
////		sampler2D _MainTex;
////
////		struct Input {
////			float2 uv_MainTex;
////		};
////
////		half _Glossiness;
////		half _Metallic;
////		fixed4 _Color;
////
////		void surf (Input IN, inout SurfaceOutputStandard o) {
////			// Albedo comes from a texture tinted by color
////			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
////			o.Albedo = c.rgb;
////			// Metallic and smoothness come from slider variables
////			o.Metallic = _Metallic;
////			o.Smoothness = _Glossiness;
////			o.Alpha = c.a;
////		}
////		ENDCG
//
//         Tags {"Queue" = "Background"}
//         Blend SrcAlpha OneMinusSrcAlpha
//         Lighting Off
//         ZWrite On
//         ZTest Always
//         Pass
//        {
//             Color(0,0,0,0)
//		}
//	}
//	FallBack "Diffuse"
//}



//
// A simple water effect by Tom@2016
//
// based on PolyCube version:
//    http://polycu.be/edit/?h=W2L7zN
//
// As people give me too much credit for this one,
// it's based on: http://freespace.virgin.net/hugo.elias/graphics/x_water.htm
// A very old Hugo Elias water tutorial :)
//
// Note:
//   I could use one buffer only as in https://www.shadertoy.com/view/4sd3WB
//   with a clever trick to utilize two channels
//   and keep buffer A in x/r and buffer B in y/g.
//   However, now I render every second simulation step,
//   so the animation is more dynamic.
//
// Here is 1-buffer version for comparison:
//   https://www.shadertoy.com/view/4dK3Ww
//
//
//#define TEXTURED 1
//
//void mainImage( out vec4 fragColor, in vec2 fragCoord )
//{
//    vec2 q = fragCoord.xy/iResolution.xy;
//
//#if TEXTURED == 1
//    
//    vec3 e = vec3(vec2(1.)/iResolution.xy,0.);
//    float p10 = texture2D(iChannel0, q-e.zy).x;
//    float p01 = texture2D(iChannel0, q-e.xz).x;
//    float p21 = texture2D(iChannel0, q+e.xz).x;
//    float p12 = texture2D(iChannel0, q+e.zy).x;
//    
//    // Totally fake displacement and shading:
//    vec3 grad = normalize(vec3(p21 - p01, p12 - p10, 1.));
//    vec4 c = texture2D(iChannel1, fragCoord.xy*2./iChannelResolution[1].xy + grad.xy*.35);
//    vec3 light = normalize(vec3(.2,-.5,.7));
//    float diffuse = dot(grad,light);
//    float spec = pow(max(0.,-reflect(light,grad).z),32.);
//    fragColor = mix(c,vec4(.7,.8,1.,1.),.25)*max(diffuse,0.) + spec;
//    
//#else
//    
//    float h = texture2D(iChannel0, q).x;
//    float sh = 1.35 - h*2.;
//    vec3 c =
//       vec3(exp(pow(sh-.75,2.)*-10.),
//            exp(pow(sh-.50,2.)*-20.),
//            exp(pow(sh-.25,2.)*-10.));
//    fragColor = vec4(c,1.);
//
//#endif
//}
