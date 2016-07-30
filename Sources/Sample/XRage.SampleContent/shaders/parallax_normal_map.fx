float4x4 g_matWorldViewProjection		: WORLDVIEWPROJECTION;
float4x4 g_matWorld						: WORLD;
float g_fFarClip						: FARCLIPPLANE;
float g_fSpecularPower					: SPECULAR_POWER;
float g_fSpecularIntensity				: SPECULAR_INTENSITY;
float g_fNormalMapCoefficient			: NORMAL_MAP_COEFFICIENT;
float g_fParallaxMapScaleBias			: PARALLAX_MAP_SCALE;
float3 g_vCameraPosition				: CAMERA_POSITION;

#include "include/gBuffer.fxh"
#include "include/vertexData.fxh"
#include "include/shadowing.fxh"

texture g_texDiffuseMap					: DIFFUSE_MAP;
texture g_texNormalMap					: NORMAL_MAP;
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


ParallaxMappingTransformedVertex GBuffer_VertexShader(ParallaxMappingVertex input)
{
	ParallaxMappingTransformedVertex output;

	output.Position = ComputeGBufferVertexPosition(input.Position);
	output.Depth = ComputeGBufferVertexDepth(output.Position);
	output.TexCoord = input.TexCoord;

	output.TangentToWorld = ComputeTangentToWorld(g_matWorld, input.Tangent, input.Binormal, input.Normal);
	
	float3 viewDirection = g_vCameraPosition - output.Position;
	output.ViewDirection = viewDirection;
	
	output.ParallaxOffsetTS = float2(0,0);
	return output;
}

float2 CalcParallax(ParallaxMappingTransformedVertex input)
{
	float depth = tex2D(heightSampler, input.TexCoord).a * g_fParallaxMapScaleBias; 
	float2 offset = depth * mul(input.TangentToWorld, normalize(input.ViewDirection));
	
	float depthAt1 = tex2D(heightSampler, input.TexCoord + offset).w;

	if (depthAt1  < 0.96f)
	{
		offset *= (depthAt1 + (tex2D(heightSampler, input.TexCoord).w)) * 0.5f;
	}

	return input.TexCoord + offset;

}


GBufferPixelData GBuffer_PixelShader(ParallaxMappingTransformedVertex input)
{
	float2 texCoords = CalcParallax(input);
	
	float3 diffuseColor = tex2D(diffuseSampler, texCoords);
	float3 normal = tex2D(normalSampler, texCoords);
		
	normal = mul(2.0f * normal - 1.0f, input.TangentToWorld);
	normal = normal * g_fNormalMapCoefficient + (1.0f - g_fNormalMapCoefficient) * input.TangentToWorld[2];
	normal = 0.5f * (normalize(normal) + 1.0f);
		
	return ComputeGBufferPixel(
		input.Depth,
		diffuseColor,
		normal,
		g_fSpecularPower,
		g_fSpecularIntensity);
}

float4 DepthBuffer_PixelShader(ParallaxMappingTransformedVertex input) : COLOR0
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

		VertexShader = compile vs_3_0 GBuffer_VertexShader();
		PixelShader = compile ps_3_0 GBuffer_PixelShader();
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