Shader "Custom/Watershader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.75
		_Metallic("Metallic", Range(0,1)) = 0.25

		// Variables for waves
		_Amplitude("Wave Amplitude", Range(0, 1)) = 0.2
		_RandomHeight("Random Noise Height", Range(0, 1)) = 0.1
		_Frequency("Wave Frequency", Range(0, 1)) = 0.3
		_Speed("Wave Speed", Range(0, 5)) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma vertex vert Lambert vertex:vert
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
		half _RandomHeight;
		half _Frequency;
		half _Speed;

		float rand(float3 myVector) {
			return frac(sin(dot(myVector, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
		}

		float sine(float x, float z)
		{
			float returnThis = 0;

			float random = rand(float3(x, 0, z));
			returnThis += sin(_Time[1] * _Speed + x * _Frequency * _Frequency) * _Amplitude;
			returnThis += sin(cos(random * 1.0f) * _RandomHeight * cos(_Time[1] * _Speed * sin(random * 1.0f)));
			return returnThis;
			//return _Amplitude * sin(_Frequency * (x + _Time.y * _Speed)) + _Amplitude * sin(_Frequency * (z + _Time.y * _Speed));
		}

		// Do stuff with verts here
		void vert(inout appdata_base v) {
			// Do all work in world space
			float3 v0 = mul(unity_ObjectToWorld, v.vertex).xyz;

			// Create two fake neighbor vertices.
			float3 v1 = v0 + float3(0.1, 0, 0); // +X
			float3 v2 = v0 + float3(0, 0, 0.1); // +Z

			// Do animation stuff here

			// Modify the real vertex
			v0.y = sine(v0.x, v0.z);

			// Modify the fake neighbors
			v1.y = sine(v1.x, v1.z);
			v2.y = sine(v2.x, v2.z);

			// Solve worldspace normal
			float3 vna = cross(v2 - v0, v1 - v0);

			// Put normals back in object space
			v.normal = normalize(mul(unity_WorldToObject, vna));

			// Put vertex back in object space
			v.vertex.xyz = mul(unity_WorldToObject, v0);
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
