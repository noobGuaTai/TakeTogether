Shader "MapGrids"
{
    Properties
    {
        _noiseTexture ("Noise Texture for rendering grids", 2D) = "" {}
        _border ("test", Vector) = (-100,-100,100,100)
        _noiseDiff ("", Float) = 0.3
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        Cull Off
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest [unity_GUIZTestMode]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 type : COLOR0;
                float2 uvGlobal : TEXCOORD1;
            };

            struct InstanceData
            {
                float2 pos;
                int type;
            };

            StructuredBuffer<InstanceData> instanceData;
            float4x4 local2World;
            float4 _border;
            sampler2D _noiseTexture;
            float _noiseDiff;

            v2f vert (appdata v, uint id:SV_INSTANCEID)
            {
                float2 pos = v.vertex.xy + instanceData[id].pos;
                v2f o;
                o.vertex = mul(local2World, float4(pos, 0, 1));
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);

                //o.vertex = float4(pos, 0, 0);
                o.uv = v.uv;
                o.type = float4((float)instanceData[id].type, 0, 0, 0);
                float2 blPos = _border.xy;
                float2 trPos = _border.zw;

                o.uvGlobal = (instanceData[id].pos - blPos) / (trPos - blPos);
                return o;;
            }

            fixed4 frag (v2f i) : SV_TARGET
            {
                fixed4 col;
                fixed4 floor = float4(85, 81, 69, 255) / 255.0;
                fixed4 wall = float4(42, 34, 31,255) / 255.0;
                fixed4 noise = tex2D(_noiseTexture, i.uvGlobal);
                fixed4 noiseCol = noise * _noiseDiff - 0.5 * _noiseDiff;

                if(i.type.r == 0)
                    col = floor;
                else
                    col = wall;
                fixed value = (col.x + col.y + col.z) / 3.0;
                col += value * noiseCol;
                //col = noise;
                //col = float4(i.uvGlobal, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
