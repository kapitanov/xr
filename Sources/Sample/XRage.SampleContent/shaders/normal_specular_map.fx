float4x4 g_matWorldViewProjection		: WORLDVIEWPROJECTION;
float4x4 g_matWorld						: WORLD;
float g_fFarClip						: FARCLIPPLANE;
float g_fSpecularPowerCoefficient		: SPECULAR_POWER_COEFFICIENT;
float g_fSpecularIntensityCoefficient	: SPECULAR_INTENSITY_COEFFICIENT;
float g_fNormalMapCoefficient			: NORMAL_MAP_COEFFICIENT;

#include "include/common.fxh"
#include "include/gBuffer.fxh"
#include "include/vertexData.fxh"
#include "include/shadowing.fxh"

texture diffuseMap						: DIFFUSE_MAP;
texture normalMap						: NORMAL_MAP;
texture specularMap						: SPECULAR_MAP;

sampler diffuseSampler = sampler_state
{
	Texture = (diffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = ANISOTROPIC;
	MINFILTER = ANISOTROPIC;
	MIPFILTER = LINEAR;
	MaxAnisotropy = 16;
};

sampler normalSampler = sampler_state
{
	Texture = (normalMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = ANISOTROPIC;
	MINFILTER = ANISOTROPIC;
	MIPFILTER = LINEAR;
	MaxAnisotropy = 16;
};

sampler specularSampler = sampler_state
{
	Texture = (specularMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = ANISOTROPIC;
	MINFILTER = ANISOTROPIC;
	MIPFILTER = LINEAR;
	MaxAnisotropy = 16;
};

NormalMappedTransformedVertex GBuffer_VertexShader(NormalMappingVertex input)
{
	NormalMappedTransformedVertex output;
	output.Position = ComputeGBufferVertexPosition(input.Position);
	output.Depth = ComputeGBufferVertexDepth(output.Position);
	output.TexCoord = input.TexCoord;
	output.TangentToWorld[0] = mul(input.Tangent, g_matWorld);
	output.TangentToWorld[1] = mul(input.Binormal, g_matWorld);
	output.TangentToWorld[2] = mul(input.Normal, g_matWorld);
	
	return output;
}

GBufferPixelData GBuffer_PixelShader(NormalMappedTransformedVertex input)
{
	float3 diffuseColor = tex2D(diffuseSampler, input.TexCoord);
	float4 specularAttributes = tex2D(specularSampler, input.TexCoord);
	float specularPower = g_fSpecularPowerCoefficient *  specularAttributes.g;
	float specularIntensity = g_fSpecularIntensityCoefficient * specularAttributes.r;

	float3 normal = tex2D(normalSampler, input.TexCoord);
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

float4 DepthBuffer_PixelShader(NormalMappedTransformedVertex input) : COLOR0
{
	return input.Depth.x / input.Depth.y;
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
		VertexShader = compile vs_2_0 ShadowMapVertexShader(g_fFarClip);
		PixelShader = compile ps_2_0 ShadowMapPixelShader();
	}
}