Shader "Custom/combineEffect" {

	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_rt0 ("RenderTexture", 2D) = "white" {}
	}

	SubShader {
		Pass {
//			GLSLPROGRAM
//			#pragma vertex vert_img
//			#pragma fragment frag
//
//			UnityCG.glslinc
//			uniform sampler2D _MainTex;
//			uniform sampler2D _rt0;
//
//			#define TEXTURED 0
//
//			void mainImage( out vec4 fragColor, in vec2 fragCoord )
//			{
//			    vec2 q = fragCoord.xy/iResolution.xy;
//
//			#if TEXTURED == 0
//			    
//			    vec3 e = vec3(vec2(1.)/iResolution.xy,0.);
//			    float p10 = texture2D(iChannel0, q-e.zy).x;
//			    float p01 = texture2D(iChannel0, q-e.xz).x;
//			    float p21 = texture2D(iChannel0, q+e.xz).x;
//			    float p12 = texture2D(iChannel0, q+e.zy).x;
//			    
//			    // Totally fake displacement and shading:
//			    vec3 grad = normalize(vec3(p21 - p01, p12 - p10, 1.));
//			    vec4 c = texture2D(iChannel1, fragCoord.xy*2./iChannelResolution[1].xy + grad.xy*.35);
//			    vec3 light = normalize(vec3(.2,-.5,.7));
//			    float diffuse = dot(grad,light);
//			    float spec = pow(max(0.,-reflect(light,grad).z),32.);
//			    fragColor = mix(c,vec4(.7,.8,1.,1.),.25)*max(diffuse,0.) + spec;
//			    
//			#else
//			    
//			    float h = texture2D(iChannel0, q).x;
//			    float sh = 1.35 - h*2.;
//			    vec3 c =
//			       vec3(exp(pow(sh-.75,2.)*-10.),
//			            exp(pow(sh-.50,2.)*-20.),
//			            exp(pow(sh-.25,2.)*-10.));
//			    fragColor = vec4(c,1.);
//
//			#endif
//			}
//
//
//			ENDGLSL
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
		
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _rt0;

			#define TEXTURED 0

			float4 frag(v2f_img i) : COLOR {
			 	float2 q = i.uv.xy;//_ScreenParams.xy;

				#if TEXTURED == 1

				    float3 e = float3(float2(1.0,1.0)/_ScreenParams.xy,0.0);

				    float p10 = tex2D(_rt0, q-e.zy).x;
				    float p01 = tex2D(_rt0, q-e.xz).x;
				    float p21 = tex2D(_rt0, q+e.xz).x;
				    float p12 = tex2D(_rt0, q+e.zy).x;


				    float3 grad = normalize(float3(p21 - p01, p12 - p10, 1.0));
				    float4 c = tex2D(_MainTex, i.uv.xy * 2.0 / _ScreenParams.xy); //+ grad.xy*.35);
				    float3 light = normalize(float3(0.2,-0.5,0.7));
				    float diffuse2 = dot(grad,light);
				    float spec = pow(max(0.0,-reflect(light,grad).z),32.0);

				    return lerp(c,1.,1.);

//				    return lerp(c,float4(0.7,0.8,1.0,1.0),0.25)*max(diffuse2,0.0) + spec;	

				#else

			   		float h = tex2D(_rt0, q).x;
				    float sh = 1.35 - h*2.;
				    float3 c = 
					    float3(exp(pow(sh-.75,2.)*-10.),
					        exp(pow(sh-.50,2.)*-20.),
					    	exp(pow(sh-.25,2.)*-10.));
				    return float4(c,1.0);

		   		#endif
			}

			ENDCG
		}
	}
}
