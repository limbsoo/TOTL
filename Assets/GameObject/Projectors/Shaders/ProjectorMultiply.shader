// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/Multiply" {
	Properties {
		_ShadowTex ("Cookie", 2D) = "gray" {}
		_FalloffTex ("FallOff", 2D) = "white" {}
	}
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
			};
			
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.uvShadow = mul (unity_Projector, vertex);
				o.uvFalloff = mul (unity_ProjectorClip, vertex);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			sampler2D _ShadowTex;
			sampler2D _FalloffTex;
			
fixed4 frag(v2f i) : SV_Target
{
                // 텍스처의 범위 클리핑
    float4 shadowProj = UNITY_PROJ_COORD(i.uvShadow);
    float4 falloffProj = UNITY_PROJ_COORD(i.uvFalloff);

                // 클리핑 범위를 설정합니다 (예: x, y, z가 0~1 범위에 있도록)
    if (shadowProj.x < 0.0 || shadowProj.x > 1.0 ||
                    shadowProj.y < 0.0 || shadowProj.y > 1.0 ||
                    shadowProj.z < 0.0 || shadowProj.z > 1.0)
    {
        discard; // 투영 범위를 넘어가면 픽셀을 버립니다.
    }

    fixed4 texS = tex2Dproj(_ShadowTex, shadowProj);
    texS.a = 1.0 - texS.a;

    fixed4 texF = tex2Dproj(_FalloffTex, falloffProj);
    fixed4 res = lerp(fixed4(1, 1, 1, 0), texS, texF.a);

    UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(1, 1, 1, 1));
    return res;
}

			//fixed4 frag (v2f i) : SV_Target
			//{
			//	fixed4 texS = tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
			//	texS.a = 1.0-texS.a;

			//	fixed4 texF = tex2Dproj (_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
			//	fixed4 res = lerp(fixed4(1,1,1,0), texS, texF.a);

			//	UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(1,1,1,1));
			//	return res;
			//}
			ENDCG
		}
	}
}
