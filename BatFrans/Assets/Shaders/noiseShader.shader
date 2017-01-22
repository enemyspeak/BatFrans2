Shader "Custom/noise" {
	Properties {
		_rt0 ("Texture", 2D) = "white" {}
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		
			#include "UnityCG.cginc"

			uniform int firstOctave = 3;
			uniform int octaves = 8;
			uniform float persistence = 0.6;

			uniform sampler2D _rt0; // mask
			uniform sampler2D _MainTex;

			struct v2f{
                float4 position : SV_POSITION;
            };
            
            v2f vert(float4 v:POSITION) : SV_POSITION {
                v2f o;
                o.position = mul (UNITY_MATRIX_MVP, v);
                return o;
            }

			float doNoise(int x,int y) {   
			    float fx = float(x);
			    float fy = float(y);
			    
			    return 2.0 * frac(sin(dot(float2(fx, fy) ,float2(12.9898,78.233))) * 43758.5453) - 1.0;
			}

			float smoothNoise(int x,int y) {
			    return doNoise(x,y)/4.0+(doNoise(x+1,y)+doNoise(x-1,y)+doNoise(x,y+1)+doNoise(x,y-1))/8.0+(doNoise(x+1,y+1)+doNoise(x+1,y-1)+doNoise(x-1,y+1)+doNoise(x-1,y-1))/16.0;
			}

			float COSInterpolation(float x,float y,float n) {
			    float r = n*3.1415926;
			    float f = (1.0-cos(r))*0.5;
			    return x*(1.0-f)+y*f;			    
			}

			float InterpolationNoise(float x, float y) {
			    int ix = int(x);
			    int iy = int(y);
			    float fracx = x-float(int(x));
			    float fracy = y-float(int(y));
			    
			    float v1 = smoothNoise(ix,iy);
			    float v2 = smoothNoise(ix+1,iy);
			    float v3 = smoothNoise(ix,iy+1);
			    float v4 = smoothNoise(ix+1,iy+1);
			    
			   	float i1 = COSInterpolation(v1,v2,fracx);
			    float i2 = COSInterpolation(v3,v4,fracx);
			    
			    return COSInterpolation(i1,i2,fracy);
			}

			float PerlinNoise2D(float x,float y)
			{
			    float sum = 0.0;
			    float frequency = 0.0;
			    float amplitude = 0.0;
			    for(int i=firstOctave;i<octaves + firstOctave;i++)
			    {
			        frequency = pow(2.0,float(i));
			        amplitude = pow(persistence,float(i));
			        sum = sum + 0.4;// InterpolationNoise(x*frequency,y*frequency)*amplitude;
			    }
			    
			    return sum;
			}

//			float4 frag(v2f_img i) : COLOR {
			fixed4 frag(v2f i) : SV_Target {
				float2 q = i.position.xy/_ScreenParams.xy;
			    float noise = .4+111.*PerlinNoise2D(q.x,q.y);

//				return float4(q,1.,1.);
			    return float4(noise,noise,noise,1.0);
			}

			ENDCG
		}
	}
}

//
//
//const int firstOctave = 3;
//const int octaves = 8;
//const float persistence = 0.6;
//
////Not able to use bit operator like <<, so use alternative noise function from YoYo
////
////https://www.shadertoy.com/view/Mls3RS
////
////And it is a better realization I think
//float noise(int x,int y)
//{   
//    float fx = float(x);
//    float fy = float(y);
//    
//    return 2.0 * fract(sin(dot(vec2(fx, fy) ,vec2(12.9898,78.233))) * 43758.5453) - 1.0;
//}
//
//float smoothNoise(int x,int y)
//{
//    return noise(x,y)/4.0+(noise(x+1,y)+noise(x-1,y)+noise(x,y+1)+noise(x,y-1))/8.0+(noise(x+1,y+1)+noise(x+1,y-1)+noise(x-1,y+1)+noise(x-1,y-1))/16.0;
//}
//
//float COSInterpolation(float x,float y,float n)
//{
//    float r = n*3.1415926;
//    float f = (1.0-cos(r))*0.5;
//    return x*(1.0-f)+y*f;
//    
//}
//
//float InterpolationNoise(float x, float y)
//{
//    int ix = int(x);
//    int iy = int(y);
//    float fracx = x-float(int(x));
//    float fracy = y-float(int(y));
//    
//    float v1 = smoothNoise(ix,iy);
//    float v2 = smoothNoise(ix+1,iy);
//    float v3 = smoothNoise(ix,iy+1);
//    float v4 = smoothNoise(ix+1,iy+1);
//    
//   	float i1 = COSInterpolation(v1,v2,fracx);
//    float i2 = COSInterpolation(v3,v4,fracx);
//    
//    return COSInterpolation(i1,i2,fracy);
//    
//}
//
//float PerlinNoise2D(float x,float y)
//{
//    float sum = 0.0;
//    float frequency =0.0;
//    float amplitude = 0.0;
//    for(int i=firstOctave;i<octaves + firstOctave;i++)
//    {
//        frequency = pow(2.0,float(i));
//        amplitude = pow(persistence,float(i));
//        sum = sum + InterpolationNoise(x*frequency,y*frequency)*amplitude;
//    }
//    
//    return sum;
//}
//
//void mainImage( out vec4 fragColor, in vec2 fragCoord )
//{
//   
//	vec2 uv = fragCoord.xy / iResolution.xy;
//    float x = uv.x;
//    float y = uv.y;
//	//fragColor = vec4(noise(x,y),noise(x,y),noise(x,y),1);
//    float noise = 0.3+0.7*PerlinNoise2D(x,y);
//    fragColor = vec4(noise,noise,noise,1.0);
//}