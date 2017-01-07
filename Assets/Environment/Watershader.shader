/*
Unity Answer thingy:
http://answers.unity3d.com/questions/175600/gpu-generated-mesh.html
ShaderLab reference material:
https://docs.unity3d.com/Manual/SL-Reference.html
NVidia's CG stuffs:
http://http.developer.nvidia.com/GPUGems2/gpugems2_chapter26.html

*/
Shader "Custom/Watershader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.75
		_Metallic ("Metallic", Range(0,1)) = 0.25

		// Variables for waves
		_Amplitude ("Wave Amplitude", Range(0, 1)) = 0.2
		_Frequency ("Wave Frequency", Range(0, 1)) = 0.3
		_Speed ("Wave Speed", Range(0, 5)) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};
		
		// Variables
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Wavy thingies
		half _Amplitude;
		half _Frequency;
		half _Speed;

		// Do stuff with verts here
		void vert() {
			// Something needs to happen to the Y-coord of the vert here.
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a color
			o.Albedo = _Color.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			// Alpha comes from the color
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
