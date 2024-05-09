Shader "Unlit/Ring"
{
    Properties
    {
        [Header(Textures)]
        _MainTex("Main texture", 2D) = "white" {}
        _NoiseRotationMaskTex("Noise rotation mask texture", 2D) = "white" {}

        [Header(Noise)]
        _NoiseMaskRotationSpeed("Noise mask rotation speed", Range(0, 1)) = 1.0
        _NoiseLuminosity("Noise luminosity", Range(0, 300)) = 5
        _NoiseIntensity("Noise intensity", Range(0, 1)) = 1.0
        _NoiseThreshold("Noise threshold", Range(0, 1)) = 0.025

        [Header(Physical Properties)]
        _OuterRadius("Outer radius", Range(0, 0.5)) = 0.5
        _InnerRadius("Inner radius", Range(0, 0.5)) = 0.5
        _RingColor("Ring color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent"
        }

        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseRotationMaskTex;
            float _NoiseMaskRotationSpeed;
            float _NoiseLuminosity;
            float _NoiseIntensity;
            float _NoiseThreshold;
            half _OuterRadius;
            half _InnerRadius;
            float4 _RingColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Function to rotate UV coordinates
            float2 RotateUV(float2 uv, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);
                float2 rotatedUV;
                rotatedUV.x = uv.x * c - uv.y * s;
                rotatedUV.y = uv.x * s + uv.y * c;
                return rotatedUV;
            }

            v2f vert(appdata v, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // first, calculate the distance from the center of the mesh
                float dist = length(i.uv - float2(0.5, 0.5));

                // next, determine the alpha value based on whether the distance is within the specified outer radius
                float alpha = saturate(step(_InnerRadius, dist) * (1.0 - step(_OuterRadius, dist)));

                // simulate variations in ring density based on distance
                alpha *= 1.0 - smoothstep(_InnerRadius, _OuterRadius, dist);

                // sample our textures
                fixed4 mainTexColor = tex2D(_MainTex, i.uv);

                // calculate rotated UV for the detail textures
                float2 centerOffset = float2(0.5, 0.5);
                float2 rotatedNoiseMaskTexUV = RotateUV(i.uv - centerOffset, _NoiseMaskRotationSpeed * _Time.y) + centerOffset;
                fixed4 rotatedNoiseMaskTexColor = tex2D(_NoiseRotationMaskTex, rotatedNoiseMaskTexUV).r;

                // generate procedural noise (you may need to adjust the parameters)
                float noise = frac(sin(dot(i.uv, float2(12.9898, 78.233))) * 43758.5453);
                noise *= _NoiseIntensity;

                // threshold the noise to create larger gaps
                alpha *= (noise < _NoiseThreshold) ? 0.0 : 1.0;

                // Ignore black pixels in detail textures
                rotatedNoiseMaskTexColor.rgb = rotatedNoiseMaskTexColor.rgb * (_NoiseLuminosity + rotatedNoiseMaskTexColor.a);

                // combine main texture, detail texture, and procedural noise
                fixed4 finalColor = _RingColor * (mainTexColor * rotatedNoiseMaskTexColor);
                return finalColor * alpha * _RingColor.a;
            }
            ENDCG
        }
    }
}
