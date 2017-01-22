Shader "Custom/combineEffect" {

	Properties {
		_rt0 ("RenderTexture", 2D) = "white" {}
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
		
			#include "UnityCG.cginc"

			uniform sampler2D _rt0;
			uniform sampler2D _MainTex;

			#define TEXTURED 0

			float4 frag(v2f_img i) : COLOR {
			 	float2 q = i.uv.xy;//_ScreenParams.xy;
			 	return tex2D(_rt0,i.uv.xy);

				#if TEXTURED == 1

					float3 e = float3(float2(1.,1.)/_ScreenParams.xy,0.);

				    float p10 = tex2D(_rt0, q-e.zy).x;
				    float p01 = tex2D(_rt0, q-e.xz).x;
				    float p21 = tex2D(_rt0, q+e.xz).x;
				    float p12 = tex2D(_rt0, q+e.zy).x;


//			        vec3 grad = normalize(vec3(p21 - p01, p12 - p10, 1.));
//				    vec4 c = texture2D(iChannel1, fragCoord.xy*2./iChannelResolution[1].xy + grad.xy*.35);
//				    vec3 light = normalize(vec3(.2,-.5,.7));
//				    float diffuse = dot(grad,light);
//				    float spec = pow(max(0.,-reflect(light,grad).z),32.);
//				    fragColor = mix(c,vec4(.7,.8,1.,1.),.25)*max(diffuse,0.) + spec;

				    float3 grad = normalize(float3(p21 - p01, p12 - p10, 1.));
				    float4 c = tex2D(_MainTex, i.uv.xy * 2./float2(1280.,720.) + grad.xy*.35);
				    float3 light = normalize(float3(.2,-.5,.7));
				    float diffuse2 = dot(grad,light);
				    float spec = pow(max(0.,-reflect(light,grad).z),32.);

				    return lerp(c,float4(.7,.8,1.,1.),.25)*max(diffuse2,0.) + spec;	

				#else

//					float h = texture2D(iChannel0, q).x;
//				    float sh = 1.35 - h*2.;
//				    vec3 c =
//				       vec3(exp(pow(sh-.75,2.)*-10.),
//				            exp(pow(sh-.50,2.)*-20.),
//				            exp(pow(sh-.25,2.)*-10.));
//				    fragColor = vec4(c,1.);

			   		float h = tex2D(_rt0, q).x;
				    float sh = 1.35 - h*2.;
				    float3 c = 
					    float3( exp(pow(sh-.75,2.)*-10.),
					        	exp(pow(sh-.50,2.)*-20.),
					    		exp(pow(sh-.25,2.)*-10.));
				    return float4(c,1.);

		   		#endif
			}

			ENDCG
		}
	}
}
