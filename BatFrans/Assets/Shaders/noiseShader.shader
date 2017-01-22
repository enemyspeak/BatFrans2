Shader "Custom/noise" {

	Properties {
		_rt0 ("Texture", 2D) = "white" {}
	}

	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
		
			#include "UnityCG.cginc"

			uniform int _FrameCount;
			uniform float4 _SourcePosition;
			uniform sampler2D _rt0;
			uniform sampler2D _rt1;

			float4 frag(v2f_img i) : COLOR {
				
			}
			ENDCG
		}
	}
}
