float4x4 g_matWorldViewProjection	: WORLDVIEWPROJECTION;
float4x4 g_matWorld					: WORLD;

#include "include/gBuffer.fxh"

float specularPower					: SPECULAR_POWER;
float specularIntensity				: SPECULAR_INTENSITY;

texture diffuseMap					: DIFFUSE_MAP;
texture normalMap					: NORMAL_MAP;

sampler diffuseSampler = sampler_state
{
	Texture = (diffuseMap);
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

sampler normalSampler = sampler_state
{
	Texture = (normalMap);
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

struct VertexData
{
	float4 	Position	: POSITION;
	float2 	TexCoord	: TEXCOORD0;
	float3	Normal		: NORMAL0;
	float3	Binormal	: BINORMAL0;
	float3	Tangent		: TANGENT0;
};

struct TransformedVertexData
{
	float4 Position			: POSITION0;
	float2 TexCoord			: TEXCOORD0;
	float2 Depth			: TEXCOORD1;
	float3x3 TangentToWorld	: TEXCOORD2;
};

TransformedVertexData VertexShaderFunction(VertexData input)
{
	TransformedVertexData output;
	output.Position = ComputeGBufferVertexPosition(input.Position);
	output.Depth = ComputeGBufferVertexDepth(output.Position);
	output.TexCoord = input.TexCoord;
	output.TangentToWorld[0] = mul(input.Tangent, g_matWorld);
	output.TangentToWorld[1] = mul(input.Binormal, g_matWorld);
	output.TangentToWorld[2] = mul(input.Normal, g_matWorld);
	
	return output;
}

GBufferPixelData PixelShaderFunction(TransformedVertexData input)
{
	float3 diffuseColor = tex2D(diffuseSampler, input.TexCoord);

	float3 normal = tex2D(normalSampler, input.TexCoord);
	normal = 2.0f * normal - 1.0f;
	normal = mul(normal, input.TangentToWorld);
	normal = 0.5f * (normalize(normal)+ 1.0f);

	return ComputeGBufferPixel(
		input.Depth,
		diffuseColor,
		normal,
		specularPower,
		specularIntensity);
}

technique RenderGBuffer
{
	pass Pass_1
	{
		ZWriteEnable = true;
		ZEnable = true;		
		AlphaBlendEnable = false;
		FillMode = Solid;
		CullMode = CCW; 

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
