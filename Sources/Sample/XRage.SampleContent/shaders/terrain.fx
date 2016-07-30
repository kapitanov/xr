float4x4 g_matWorldViewProjection	: WORLDVIEWPROJECTION;
float4x4 g_matWorld					: WORLD;
float g_fSpecularPower				: SPECULAR_POWER;
float g_fSpecularIntensity			: SPECULAR_INTENSITY;
float g_fTerrainScale				: TERRAIN_SCALE;

#include "include/gBuffer.fxh"

texture g_texTextureMap				: TEXTURE_MAP;

texture g_texSandDiffuse			: SAND_TEXTURE;
texture g_texSandNormal				: SAND_NORMAL;

texture g_texGrassDiffuse			: GRASS_TEXTURE;
texture g_texGrassNormal			: GRASS_NORMAL;

texture g_texRockDiffuse			: ROCK_TEXTURE;
texture g_texRockNormal				: ROCK_NORMAL;

// Texture map sampler
sampler textureMapSampler = sampler_state
{
	Texture = (g_texTextureMap);
	ADDRESSU = wrap;
	ADDRESSV = wrap;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

// Sand texture samplers
sampler sandDiffuseSampler = sampler_state
{
	Texture = (g_texSandDiffuse);
	ADDRESSU = wrap;
	ADDRESSV = wrap;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

sampler sandNormalSampler = sampler_state
{
	Texture = (g_texSandNormal);
	ADDRESSU = wrap;
	ADDRESSV = wrap;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

// Grass texture samplers
sampler grassDiffuseSampler = sampler_state
{
	Texture = (g_texGrassDiffuse);
	ADDRESSU = wrap;
	ADDRESSV = wrap;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

sampler grassNormalSampler = sampler_state
{
	Texture = (g_texGrassNormal);
	ADDRESSU = wrap;
	ADDRESSV = wrap;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};


// Rock texture samplers
sampler rockDiffuseSampler = sampler_state
{
	Texture = (g_texRockDiffuse);
	ADDRESSU = wrap;
	ADDRESSV = wrap;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

sampler rockNormalSampler = sampler_state
{
	Texture = (g_texRockNormal);
	ADDRESSU = wrap;
	ADDRESSV = wrap;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

struct VertexData
{
	float4 	Position		: POSITION;
	float3	Normal			: NORMAL0;
	float2 TexCoord			: TEXCOORD0;
};

struct TransformedVertexData
{
	float4 Position			: POSITION0;
	float2 TexCoord			: TEXCOORD0;
	float2 Depth			: TEXCOORD1;
	float  Height			: TEXCOORD2;
	float3 Normal			: TEXCOORD3;
};

TransformedVertexData VertexShaderFunction(VertexData input)
{
	TransformedVertexData output;
	output.Position = ComputeGBufferVertexPosition(input.Position);
	output.Depth = ComputeGBufferVertexDepth(output.Position);
	// output.TexCoord.x = input.Position.x * 0.03f / g_fTerrainScale;
	// output.TexCoord.y = input.Position.z * 0.03f / g_fTerrainScale;

	output.TexCoord = input.TexCoord;
	output.Height = input.Position.y;

	//output.TangentToWorld[0] = mul(input.Tangent, g_matWorld);
	//output.TangentToWorld[1] = mul(input.Binormal, g_matWorld);
	//output.TangentToWorld[2] = mul(input.Normal, g_matWorld);
	
	output.Normal = input.Normal;

	return output;
}

GBufferPixelData PixelShaderFunction(TransformedVertexData input)
{
	float3 textureMapValue = tex2D(textureMapSampler, input.TexCoord);
	
	// Read diffuse maps' values
	float3 sandDiffuseColor  = tex2D(sandDiffuseSampler, input.TexCoord);
	float3 grassDiffuseColor  = tex2D(grassDiffuseSampler, input.TexCoord);
	float3 rockDiffuseColor  = tex2D(rockDiffuseSampler, input.TexCoord);
	
	// Read normal maps' values
	float3 sandNormal = tex2D(sandNormalSampler, input.TexCoord);
	float3 grassNormal = tex2D(grassNormalSampler, input.TexCoord);
	float3 rockNormal = tex2D(rockNormalSampler, input.TexCoord);

	// Combine textures and normals

	float3 diffuseColor = sandDiffuseColor * textureMapValue.b +
						  grassDiffuseColor * textureMapValue.g +
						  rockDiffuseColor * textureMapValue.r;

	//float3 normal = (2.0f * sandNormal - 1.0f) * textureMapValue.b +
	//				(2.0f * grassNormal - 1.0f)* textureMapValue.g +
	//				(2.0f * rockNormal - 1.0f)* textureMapValue.r;
	
	float3 normal = 2.0f * (sandNormal * textureMapValue.b +
							grassNormal * textureMapValue.g +
							rockNormal * textureMapValue.r) - 1.0f;

//	diffuseColor = float3(input.Height, input.Height, input.Height);
	//diffuseColor = float3(input.TexCoord.x, input.TexCoord.y, 0);
	//diffuseColor = textureMapValue;

	normal = input.Normal;
	normal = 0.5f * (normalize(normal) + 1.0f);
	
//	normal = textureMapValue;

	return ComputeGBufferPixel(
		input.Depth,
		diffuseColor,
		normal,
		g_fSpecularPower,
		g_fSpecularIntensity);
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
