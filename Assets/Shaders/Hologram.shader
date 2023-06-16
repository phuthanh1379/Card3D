Shader "Unlit/SpecialFX/Hologram"
{
    // Define variables
    Properties
    {
        // Declare default name, type, default value
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1) // White
        _Transparency("Transparency", Range(0.0,0.5)) = 0.25
        _CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
        _Distance("Distance", Float) = 1
        _Amplitude("Amplitude", Float) = 1
        _Speed("Speed", Float) = 1
        _Amount("Amount", Range(0.0,1.0)) = 1
    }
    
    // Main code, there can be many SubShader
    SubShader
    {
        // Change RenderType must also change Queue (RenderQueue)
        // See: SL SubshaderTags
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        
        // Level of details
        LOD 100
        
        ZWrite Off // Not to render to the Depth Buffer (See: Cull and Depth)
        Blend SrcAlpha OneMinusSrcAlpha // See: SL Blend
        
        Pass
        {
            // Instructions for GPU
            CGPROGRAM
            #pragma vertex vert // Call Vertex function "vert"
            #pragma fragment frag // Call Fragment function "frag"

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // Texture co-ordinates
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION; // Screen-space position
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float _Transparency;
            float _CutoutThresh;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _Amount;

            // Vertex function:
            // Takes the shape of model, potentially modifies it
            v2f vert (appdata v)
            {
                v2f o;

                // Apply sinusoidal movement to vertices in object space along the x-axis before
                // they get translated in the Unity object at clip position function
                v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
                
                // Moving from LocalSpace -> WorldSpace -> ViewSpace -> ClipSpace -> ScreenSpace
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Fragment function:
            // Applies color to the shape output by the vert function
            // SV_Target: Render target (In this example: the frame buffer for the screen)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
                
                col.a = _Transparency;
                
                clip(col.r - _CutoutThresh); // Cutout certain vertices which have the Red channel < CutoutThresh
                // if (col.r < _CutoutThresh) discard; // Same solution, but not widely used
                return col;
            }
            ENDCG
        }
    }
}
