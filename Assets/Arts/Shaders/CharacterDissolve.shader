Shader "Custom/CharacterDissolve"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0
        _EdgeColor("Edge Color", Color) = (0, 0, 0, 0)
        _TextureBrightness("Texture Brightness", Range(0, 1)) = 1.0

        _LightIntensity("Light Intensity", Range(0, 1)) = 1.0
        _LightColor("Light Color", Color) = (1, 1, 1, 1) // ����Ʈ ���� ������Ƽ
        _LightDirection("Light Direction", Vector) = (0, -1, 0) // ����Ʈ ����
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
            #include "UnityCG.cginc"

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
            float _TextureBrightness;
            float _LightIntensity;
            float _DissolveThreshold;
            float4 _EdgeColor;
            float4 _LightColor; // ����Ʈ ���� ����
            float3 _LightDirection; // ����Ʈ ���� ����

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.rgb *= _TextureBrightness; // �ؽ�ó�� ��� ����

                // ����Ʈ ���� ���� ����ȭ
                float3 lightDir = normalize(_LightDirection);

                // ����Ʈ ���� ��: ����Ʈ ����� ���� ������ ����(dot)�� ���
                float diff = max(0, dot(i.worldNormal, lightDir));

                // ���� ����� ���⸦ ���
                float3 lightColor = _LightColor.rgb * diff * _LightIntensity;

                // �ؽ�ó ���� ���� ������ ����
                fixed4 col = texColor;
                col.rgb += lightColor;

                fixed4 edgeCol = _EdgeColor;
                edgeCol.a = col.a;

                float noiseScale = 1.0;
                fixed noise = frac(sin(dot(i.uv * noiseScale, float2(12.9898, 78.233))) * 43758.5453);
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