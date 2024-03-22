Shader "MapGrids"
{
    Properties
    {
        
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
            };

            struct InstanceData
            {
                float2 pos;
                int type;
            };

            StructuredBuffer<InstanceData> instanceData;
            float4x4 local2World;

            v2f vert (appdata v, uint id:SV_INSTANCEID)
            {
                float2 pos = v.vertex.xy + instanceData[id].pos;
                v2f o;
                o.vertex = mul(local2World, float4(pos, 0, 1));
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);

                //o.vertex = float4(pos, 0, 0);
                o.uv = v.uv;
                o.type = float4((float)instanceData[id].type, 0, 0, 0);
                return o;
            }

            fixed4 frag (v2f i) : SV_TARGET
            {
                fixed4 col;
                if(i.type.r == 0)
                    col = fixed4(1, 0, 0, 1);
                else
                    col = fixed4(0, 1, 0, 1);
                col = fixed4(i.vertex.xy, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
