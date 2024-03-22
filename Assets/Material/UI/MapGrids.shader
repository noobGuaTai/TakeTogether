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
            "IgnoreProjector"="True"
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
                int type : COLOR0;
            };

            struct InstanceData
            {
                float2 pos;
                int type;
            };

            StructuredBuffer<InstanceData> instanceData;

            v2f vert (appdata v, uint id:SV_INSTANCEID)
            {
                float2 pos = v.vertex.xy + instanceData[id].pos;
                v2f o;
                o.vertex = UnityObjectToClipPos(float4(pos, 0, 1));

                //o.vertex = float4(pos, 0, 0);
                o.uv = v.uv;
                o.type = instanceData[id].type;
                return o;
            }

            fixed4 frag (v2f i) : SV_TARGET
            {
                fixed4 col;
                if(i.type == 0)
                    col = fixed4(1, 0, 0, 1);
                else
                    col = fixed4(0, 1, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
