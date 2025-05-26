Shader"Toon/Grass"
{
    Properties
    {
		[Header(Shading)]
		[Toggle(_UseGrass)]_UseGrass("Grass Only", Float) = 0
		_BaseTexture("Texture",2D) = "white" {}
        _TopColor("Top Color", Color) = (1,1,1,1)
		_BottomColor("Bottom Color", Color) = (1,1,1,1)
		_TranslucentGain("Translucent Gain", Range(0,1)) = 0.5
		_BendRotationRandom("Bend Rotation Random", Range(0, 1)) = 0.2
		_GrassWidth("Grass Width", Float) = 0.05
		_GrassWidthRandom("Grass Width Random", Float) = 0.02
		_GrassHeight("Grass Height", Float) = 0.5
		_GrassHeightRandom("Grass Height Random", Float) = 0.3
		_TessellationUniform("Tessellation Uniform",Range(1,64)) = 1
		_WindDistortionMap("Wind Distortion Map", 2D) = "white" {}
		_WindFrequency("Wind Frequency", Vector) = (0.05, 0.05, 0, 0)
		_WindStrength("Wind Strength", Float) = 1
		_BladeForward("Blade Forward Amount", Float) = 0.38
		_BladeCurve("Blade Curvature Amount", Range(1, 4)) = 2
		_CullDistance("Cull Distance", Float) = 2
	
    }

	HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "Assets/shader/other/CustomTessellation.cginc"
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#define BLADE_SEGMENTS 3
CBUFFER_START(UnityPerMaterial)

float _UseGrass;
float _BladeForward;
float _BladeCurve;
float _BendRotationRandom;
float _GrassWidth;
float _GrassWidthRandom;
float _GrassHeight;
float _GrassHeightRandom;
float _CullDistance;
sampler2D _WindDistortionMap;
sampler2D _BaseTexture;
sampler2D _NormalMap;
float4 _BaseTexture_ST;
bool GrassOnly;
float4 _WindDistortionMap_ST;
float _WindStrength;
float2 _WindFrequency;
CBUFFER_END
struct geometryOutput
{
    float4 pos : SV_Position;
    float2 uv : TEXCOORD0;
    float4 _ShadowCoord : TEXCOORD1;
    float3 normal : NORMAL;
	float isBase : TEXCOORD2;

};
geometryOutput VertexOutput(float3 pos, float2 uv, float3 normal)

{
    geometryOutput o;

    o.pos = TransformObjectToHClip(pos);
	o._ShadowCoord = GetShadowCoord(GetVertexPositionInputs(pos.xyz));
	o.isBase = 0;
    o.uv = uv;
    o.normal = TransformObjectToWorldNormal(normal);
	#if UNITY_PASS_SHADOWCASTER
	// Applying the bias prevents artifacts from appearing on the surface.
	#endif
    return o;
}



	// Simple noise function, sourced from http://answers.unity.com/answers/624136/view.html
	// Extended discussion on this function can be found at the following link:
	// https://forum.unity.com/threads/am-i-over-complicating-this-random-function.454887/#post-2949326
	// Returns a number in the 0...1 range.
	float rand(float3 co)
	{
		return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
	}

	// Construct a rotation matrix that rotates around the provided axis, sourced from:
	// https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33
	float3x3 AngleAxis3x3(float angle, float3 axis)
	{
		float c, s;
		sincos(angle, s, c);

		float t = 1 - c;
		float x = axis.x;
		float y = axis.y;
		float z = axis.z;

		return float3x3(
			t * x * x + c, t * x * y - s * z, t * x * z + s * y,
			t * x * y + s * z, t * y * y + c, t * y * z - s * x,
			t * x * z - s * y, t * y * z + s * x, t * z * z + c
			);
	}
geometryOutput GenerateGrassVertex(float3 vertexPosition, float width, float height, float forward,float2 uv, float3x3 transformMatrix)
{
    float3 tangentPoint = float3(width, forward, height);
    float3 tangentNormal = normalize(float3(0, -1, forward));
    float3 localNormal = mul(transformMatrix, tangentNormal);
    float3 localPosition = vertexPosition + mul(transformMatrix, tangentPoint);
    return VertexOutput(localPosition, uv, localNormal);
}

[maxvertexcount(BLADE_SEGMENTS * 2 + 1 + 3)]

void calgeo(triangle vertexOutput IN[3] : SV_Position, inout TriangleStream<geometryOutput> triStream)
{
    //Origin
	geometryOutput o;
    if (_UseGrass < 0.5)
    {
		
    
        for (int v = 0; v < 3; v++)
        {
            o.pos = TransformObjectToHClip(IN[v].vertex);
            o._ShadowCoord = GetShadowCoord(GetVertexPositionInputs(IN[v].vertex.xyz));

            o.uv = TRANSFORM_TEX(IN[v].uv, _BaseTexture);
            o.normal = TransformObjectToWorldNormal(float3(0, 1, 0));
            o.isBase = 1;
            triStream.Append(o);
        }
    }
	//Blade
    float3 pos = IN[0].vertex;
    float dist = distance(GetCameraPositionWS(), TransformObjectToWorld(pos));
    if (dist > _CullDistance)
    {
        return;
    }
    float3 vNormal = IN[0].normal;
    float4 vTangent = IN[0].tangent;
    float3 vBinormal = cross(vNormal, vTangent) * vTangent.w;
    float3x3 tangentToLocal = float3x3(
	vTangent.x, vBinormal.x, vNormal.x,
	vTangent.y, vBinormal.y, vNormal.y,
	vTangent.z, vBinormal.z, vNormal.z
	);
    float3x3 facingRotationMatrix = AngleAxis3x3(rand(pos) * PI*2, float3(0, 0,1));
    float3x3 bendRotationMatrix = AngleAxis3x3(rand(pos.zzx) * _BendRotationRandom * PI * 0.5, float3(-1, 0, 0));
    float2 uv = pos.xz * _WindDistortionMap_ST.xy + _WindDistortionMap_ST.zw + _WindFrequency * _Time.y;
    float2 windSample = (tex2Dlod(_WindDistortionMap, float4(uv, 0, 0)).xy * 2 - 1) * _WindStrength;
    float3 wind = normalize(float3(windSample.x, windSample.y, 0));
    float3x3 windRotation = AngleAxis3x3(PI * windSample, wind);
    float3x3 transformationMatrix = mul(mul(mul(tangentToLocal, windRotation), facingRotationMatrix), bendRotationMatrix);
    float3x3 transformationMatrixFacing = mul(tangentToLocal, facingRotationMatrix);

    float height = (rand(pos.zyx) * 2 - 1) * _GrassHeightRandom + _GrassHeight;
    float width = (rand(pos.xzy) * 2 - 1) * _GrassWidthRandom + _GrassWidth;
    float forward = rand(pos.yyz) * _BladeForward;

    for (int i = 0; i < BLADE_SEGMENTS; i++)
    {
		
        float t = i / (float) BLADE_SEGMENTS;
        float segmentHeight = height * t;
        float segmentWidth = width * (1 - t);
        float segmentForward = pow(t, _BladeCurve) * forward;
        float3x3 transformMatrix = i == 0 ? transformationMatrixFacing : transformationMatrix;
        triStream.Append(GenerateGrassVertex(pos, segmentWidth, segmentHeight, segmentForward,float2(0, t), transformMatrix));
        triStream.Append(GenerateGrassVertex(pos, -segmentWidth, segmentHeight, segmentForward,float2(1, t), transformMatrix));

    }
    triStream.Append(GenerateGrassVertex(pos, 0, height, forward,float2(0.5, 1), transformationMatrix));
	
}


	ENDHLSL

    SubShader
    {

        Pass
        {

			Tags
			{
				"RenderType" = "Opaque"

				"RenderPipeline" = "UniversalPipeline"


			}
			
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_fwdbase

            #pragma geometry calgeo
			#pragma hull hull
			#pragma domain domain
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

			float4 _TopColor;
			float4 _BottomColor;
			float _TranslucentGain;

			half4 frag (geometryOutput i, half facing : VFACE) : SV_Target
            {	
				float3 normalTS = UnpackNormal(tex2D(_NormalMap, i.uv));
				float3 normal = facing > 0 ? i.normal * normalTS : -i.normal * normalTS;
				float shadow = MainLightRealtimeShadow(i._ShadowCoord) + 0.5;
				float NdotL = saturate(saturate(dot(normal, _MainLightPosition.xyz)) + _TranslucentGain);

				float3 ambient = SampleSHVertex(float4(normal, 1));
				float4 lightIntensity = NdotL * _MainLightColor + float4(ambient, 1);
				if (i.isBase > 0.5) 
				{
                    // This is the base triangle, use the base texture
                    float4 baseColor = tex2D(_BaseTexture, i.uv);
                    return baseColor * lightIntensity * shadow;
                } else {
                    // This is a grass blade, use the gradient coloring
                    float4 col = (_TopColor * i.uv.y + lightIntensity * _BottomColor) * shadow;
                    return col;
                }
				 //return float4(normal * 0.5 + 0.5, 1);


            }
            ENDHLSL
        }
Pass
{
	Tags
	{
		"LightMode" = "ShadowCaster"
		
	}

	HLSLPROGRAM
	#pragma vertex vert
	#pragma geometry calgeo
	#pragma fragment frag
	#pragma hull hull
	#pragma domain domain
	#pragma target 4.6
	#pragma multi_compile_shadowcaster

float4 frag(geometryOutput i) : SV_Target
{
    return i.pos;

}

	ENDHLSL
}
    }
}