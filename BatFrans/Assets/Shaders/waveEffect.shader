Shader "Custom/wave" {

	Properties {
		_FrameCount ("FrameCount", int) = 0 
	    _SourcePosition ("SourcePosition", Vector) = (0,0,0,1)
		_rt0 ("Texture", 2D) = "white" {}
		_rt1 ("RenderTexture", 2D) = "white" {}
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
		
			#include "UnityCG.cginc"

			int _FrameCount;
			float4 _SourcePosition;
			uniform sampler2D _rt0;
			uniform sampler2D _rt1;

			float4 frag(v2f_img i) : COLOR {
//			   vec3 e = vec3(vec2(1.)/iResolution.xy,0.);
//			   vec2 q = fragCoord.xy/iResolution.xy;
//			   
//			   vec4 c = texture2D(iChannel0, q);
//			   
//			   float p11 = c.x;
//			   
//			   float p10 = texture2D(iChannel1, q-e.zy).x;
//			   float p01 = texture2D(iChannel1, q-e.xz).x;
//			   float p21 = texture2D(iChannel1, q+e.xz).x;
//			   float p12 = texture2D(iChannel1, q+e.zy).x;
//			   
//			   float d = 0.;
//			    
//			   if (iMouse.z > 0.) 
//			   {
//			      // Mouse interaction:
//			      d = smoothstep(4.5,.5,length(iMouse.xy - fragCoord.xy));
//			   }
//			   else
//			   {
//			      // Simulate rain drops
//			      float t = iGlobalTime*2.;
//			      vec2 pos = fract(floor(t)*vec2(0.456665,0.708618))*iResolution.xy;
//			      float amp = 1.-step(.05,fract(t));
//			      d = -amp*smoothstep(2.5,.5,length(pos - fragCoord.xy));
//			   }
//
//			   // The actual propagation:
//			   d += -(p11-.5)*2. + (p10 + p01 + p21 + p12 - 2.);
//			   d *= .99; // dampening
//			   d *= min(1.,float(iFrame)); // clear the buffer at iFrame == 0
//			   d = d*.5 + .5;
//			   
//			   fragColor = vec4(d, 0, 0, 0);

				float3 e = float3(float2(1.,1.)/_ScreenParams.xy,0.);
				float2 q = i.uv.xy/_ScreenParams.xy;

				float4 c = tex2D(_rt0, q);

				float p11 = c.x;

				float p10 = tex2D(_rt1, q-e.zy).x;
				float p01 = tex2D(_rt1, q-e.xz).x;
				float p21 = tex2D(_rt1, q+e.xz).x;
				float p12 = tex2D(_rt1, q+e.zy).x;

				float d = 0.;

				//
			    d = smoothstep(4.5,.5,length(_SourcePosition.xy - i.uv.xy));

//				float t = _Time.z;
//				float2 pos = frac(floor(t)*float2(0.456665,0.708618))*_ScreenParams.xy;
//				float amp = 1.-step(.05,frac(t));
//	  			d = -amp*smoothstep(2.5,0.5,length(pos - i.uv.xy));

	  			//
				d += -(p11-.5)*2. + (p10 + p01 + p21 + p12 - 2.);
				d *= .99; // dampening
				d *= min(1.,float(_FrameCount));
				d = d*.5 + .5;

//				return float4(d,0., 0., 0.);
				return float4(float(_FrameCount)/10,0., 0., 0.);
			}
			ENDCG
		}
	}
}