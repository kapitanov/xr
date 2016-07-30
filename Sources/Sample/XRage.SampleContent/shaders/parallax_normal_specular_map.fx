float4x4 g_matWorldViewProjection		: WORLDVIEWPROJECTION;
float4x4 g_matWorld						: WORLD;
float g_fFarClip						: FARCLIPPLANE;
float g_fSpecularPowerCoefficient		: SPECULAR_POWER_COEFFICIENT;
float g_fSpecularIntensityCoefficient	: SPECULAR_INTENSITY_COEFFICIENT;
float g_fNormalMapCoefficient			: NORMAL_MAP_COEFFICIENT;
float2 g_vParallaxMapScaleBias			: PARALLAX_MAP_SCALE_BIAS;
float3 g_vCameraPosition				: CAMERA_POSITION;

#include "include/gBuffer.fxh"
#include "include/shadowing.fxh"

texture g_texDiffuseMap					: DIFFUSE_MAP;
texture g_texNormalMap					: NORMAL_MAP;
texture g_texSpecularMap				: SPECULAR_MAP;
texture g_texHeightMap					: HEIGHT_MAP;

sampler diffuseSampler = sampler_state
{
	Texture = (g_texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = ANISOTROPIC;
	MINFILTER = ANISOTROPIC;
	MIPFILTER = LINEAR;
	MaxAnisotropy = 16;
};

sampler normalSampler = sampler_state
{
	Texture = (g_texNormalMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = ANISOTROPIC;
	MINFILTER = ANISOTROPIC;
	MIPFILTER = LINEAR;
	MaxAnisotropy = 16;
};

sampler specularSampler = sampler_state
{
	Texture = (g_texSpecularMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = ANISOTROPIC;
	MINFILTER = ANISOTROPIC;
	MIPFILTER = LINEAR;
	MaxAnisotropy = 16;
};

sampler heightSampler = sampler_state
{
	Texture = (g_texHeightMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = ANISOTROPIC;
	MINFILTER = ANISOTROPIC;
	MIPFILTER = LINEAR;
	MaxAnisotropy = 16;
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
	float3 ViewDirection	: COLOR0;
};

TransformedVertexData GBuffer_VertexShader(VertexData input)
{
	TransformedVertexData output;
	output.Position = ComputeGBufferVertexPosition(input.Position);
	output.Depth = ComputeGBufferVertexDepth(output.Position);
	output.TexCoord = input.TexCoord;
	output.TangentToWorld[0] = mul(input.Tangent, g_matWorld);
	output.TangentToWorld[1] = mul(input.Binormal, g_matWorld);
	output.TangentToWorld[2] = mul(input.Normal, g_matWorld);
	output.ViewDirection = normalize(output.Position.xyz - g_vCameraPosition);
	
	return output;
}

GBufferPixelData GBuffer_PixelShader(TransformedVertexData input)
{
	float height = tex2D(heightSampler, input.TexCoord).r;
	float2 texCoords = ComputeParallaxTexCoordsA(input.ViewDirection, input.TexCoord, height, g_vParallaxMapScaleBias);

	float3 diffuseColor = tex2D(diffuseSampler, texCoords);
	float3 normal = tex2D(normalSampler, texCoords);
	float4 specularAttributes = tex2D(specularSampler, texCoords);

	float specularPower = g_fSpecularPowerCoefficient *  specularAttributes.g;
	float specularIntensity = g_fSpecularIntensityCoefficient * specularAttributes.r;
		
	normal = mul(2.0f * normal - 1.0f, input.TangentToWorld);
	normal = normal * g_fNormalMapCoefficient + (1.0f - g_fNormalMapCoefficient) * input.TangentToWorld[2];
	normal = 0.5f * (normalize(normal) + 1.0f);
	
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

		VertexShader = compile vs_2_0 GBuffer_VertexShader();
		PixelShader = compile ps_2_0 GBuffer_PixelShader();
	}
}

technique CreateShadowMap
{
	pass Pass_1
	{
		ZWriteEnable = true;
		ZEnable = true;		
		AlphaBlendEnable = false;
		FillMode = Solid;
		CullMode = None; 

		VertexShader = compile vs_2_0 ShadowMapVertexShader(g_fFarClip);
		PixelShader = compile ps_2_0 ShadowMapPixelShader();
	}
}