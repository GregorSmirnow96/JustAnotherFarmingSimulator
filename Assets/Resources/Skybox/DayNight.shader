Shader "Custom/SkyboxBlender"
{
    Properties
    {
        _Blend ("Blend", Range(0,1)) = 0.0
        _Tint ("Tint Color", Color) = (1,1,1,1)
        _Skybox1 ("Skybox 1", CUBE) = "" {}
        _Skybox2 ("Skybox 2", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            samplerCUBE _Skybox1;
            samplerCUBE _Skybox2;
            half _Blend;
            fixed4 _Tint;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert (float4 pos : POSITION)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(pos);
                o.texcoord = pos.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color1 = texCUBE(_Skybox1, i.texcoord);
                fixed4 color2 = texCUBE(_Skybox2, i.texcoord);
                return lerp(color1, color2, _Blend) * _Tint;
            }
            ENDCG
        }
    }
}
