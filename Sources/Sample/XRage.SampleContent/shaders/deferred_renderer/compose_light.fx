float4x4 g_matInvertViewProjection;
float2 g_vHalfPixel			: HALF_PIXEL_OFFSET;
texture g_texColorMap		: GBUFFER_DIFFUSE_MAP; 
texture g_texLightingMap	: LIGHTING_MAP;

#include "../include/gBufferPostprocess.fxh"
#include "../include/hdr.fxh"
#include "../include/common.fxh"

sampler colorSampler = sampler_state
{
	Texture = (g_texColorMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = POINT;
	MinFilter = POINT;
	Mipfilter = POINT;
};

sampler lightingSampler = sampler_state
{
	Texture = (g_texLightingMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = POINT;
	MinFilter = POINT;
	Mipfilter = POINT;
};


GBufferVertexOutput VertexShaderFunction(GBufferVertexInput input)
{
	return TransformGBufferVertex(input);
}

float4 ComposeLightingPixelShader(GBufferVertexOutput input) : COLOR0
{
	float4 diffuseColor = tex2D(colorSampler, input.TexCoord);
	float4 lighting = tex2D(lightingSampler, input.TexCoord);
		
	return ComposeLighting(diffuseColor, lighting);
}

technique ComposeLightingWithGBuffer
{
	pass Pass_1
	{
		VertexShader = compile vs_2_0 TransformGBufferVertex();
		PixelShader = compile ps_2_0 ComposeLightingPixelShader();
	}
}
