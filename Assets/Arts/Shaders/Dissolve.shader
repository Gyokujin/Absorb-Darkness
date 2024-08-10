Shader "Custom/Dissolve"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0
        _EdgeColor("Edge Color", Color) = (0, 0, 0, 0)
        _Brightness("Brightness", Range(0, 2)) = 1.0

    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(3)
            };

            sampler2D _MainTex;
            float _Brightness;
            float _DissolveThreshold;
            float4 _EdgeColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldPos = i.worldPos;

                fixed3 lightColor = ambient;
                fixed3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);

                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    lightColor += Shade4PointLights (normalize(worldNormal), i.worldPos, _MainTex, viewDir);
                #endif

                col.rgb *= lightColor + _Brightness;

                fixed4 edgeCol = _EdgeColor;
                edgeCol.a = col.a;

                float noiseScale = 1.0;
                fixed noise = frac(sin(dot(i.uv * noiseScale ,float2(12.9898,78.233))) * 43758.5453);
                noise = noise * 0.5 + 0.5; // Remap the noise

                if (noise < _DissolveThreshold)
                {
                    clip(-1);
                }
                else if (noise < _DissolveThreshold + 0.1)
                {
                    return edgeCol;
                }
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            
            ENDCG
        }
    }
}