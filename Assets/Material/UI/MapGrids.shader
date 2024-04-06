Shader "MapGrids"
{
    Properties
    {
        _noiseTexture ("Noise Texture for rendering grids", 2D) = "" {}
        _border ("test _border", Vector) = (-100,-100,100,100)
        _noiseDiff ("", Float) = 0.3
        _playerMapUV ("test player uv", Vector) = (-100,-100,100,100)
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
                float2 uvGlobalDis : TEXCOORD1;
                float2 uvGlobalCon : TEXCOORD2;
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
            float4 _playerMapUV;

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

                o.uvGlobalDis = (instanceData[id].pos - blPos) / (trPos - blPos);
                o.uvGlobalCon = (pos - blPos) / (trPos - blPos);
                return o;;
            }

            fixed4 frag (v2f i) : SV_TARGET
            {
                fixed4 col;
                fixed4 floor = float4(85, 81, 69, 255) / 255.0;
                fixed4 wall = float4(42, 34, 31,255) / 255.0;
                fixed4 noise = tex2D(_noiseTexture, i.uvGlobalDis);
                fixed4 noiseCol = noise * _noiseDiff - 0.5 * _noiseDiff;

                if(i.type.r == 0)
                    col = floor;
                else
                    col = wall;
                fixed value = (col.x + col.y + col.z) / 3.0;
                col += value * noiseCol;
                //col = noise;
                //col = float4(i.uvGlobalDis, 0, 1);

                // render player's position
                float dis = distance(i.uvGlobalCon * 100, _playerMapUV.xy * 100);
                float maxDis = 2;
                float ratio = lerp(0, 1, (maxDis - clamp(dis, 0, maxDis))/maxDis);
                ratio = ratio * ratio * ratio;
                col += float4(ratio, 0,0,0);
                //col = float4(1 / dis, 0, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
