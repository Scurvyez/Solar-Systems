Shader "Unlit/FlowMap"
{
    Properties
    {
        _MainTex ("Sprite texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FlowMap ("Flow map", 2D) = "white" {}
        _FlowSpeed ("Flow speed", float) = 0.05
        _FlowMapScale ("Flow map scale", float) = 1.0
        _EdgeTransparency ("Edge transparency", Range(0, 1)) = 1.0
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

        // disable backface culling (let's us see the textures on all sides of the mesh')
        // disable lighting, depth writing, and use alpha blending
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
            
            struct appdata_t
            {
                float4 vertex   : POSITION; // vertex position
                float4 color    : COLOR; // vertex color
                float2 texcoord : TEXCOORD0; // texture coordinates
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION; // screen space vertex position
                fixed4 color    : COLOR; // interpolated color
                half2 texcoord  : TEXCOORD0; // interpolated texture coordinates
            };

            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _FlowMap;
            float _FlowSpeed;
            float _FlowMapScale;
            float _EdgeTransparency;

            v2f vert(appdata_t IN)
            {
                v2f OUT;

                OUT.vertex = UnityObjectToClipPos(IN.vertex); // transform vertex to clip space
                
                // calculate texture coordinates relative to the center of the object
                OUT.texcoord = IN.texcoord - 0.5; // center the coordinates (so the texture scaling happens based on the midpoint)
                OUT.texcoord /= _FlowMapScale; // scale the coordinates (bigger value means texture "zooms" in)
                OUT.texcoord += 0.5; // clamp back to [0, 1] range
                
                OUT.color = IN.color * _Color; // apply color tint

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // sample the flow map so we can get the flow direction (update later to account for direction from all angles)
                float3 flowDir = tex2D(_FlowMap, IN.texcoord) * 2.0f - 1.0f; 
                flowDir *= _FlowSpeed; // scale the flow direction based on speed

                // calculate our two phases for the flow animation
                float phase0 = frac(_Time.y * 0.5f + 0.5f);
                float phase1 = frac(_Time.y * 0.5f + 1.0f);

                // sample the main texture at two different phases for the animation
                half3 tex0 = tex2D(_MainTex, IN.texcoord + flowDir.xy * phase0);
                half3 tex1 = tex2D(_MainTex, IN.texcoord + flowDir.xy * phase1);

                // interpolate between the two textures based on the phase, we moving now!
                float flowLerp = abs((0.5f - phase0) / 0.5f);
                half3 finalColor = lerp(tex0, tex1, flowLerp);

                // Use the uv's to control transparency near the edges
                half alpha = saturate(1.0 - _EdgeTransparency * length(IN.texcoord - 0.5) * 2.0);

                // combine the final color with our tint color and handle alpha
                fixed4 c = float4(finalColor, alpha) * IN.color;
                c.rgb *= c.a; // apply alpha to RGB channels
                return c; // return the final color :)
            }
            ENDCG
        }
    }
}
