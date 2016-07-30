float4x4	g_matInvertViewProjection	: INV_VIEW_PROJECTION;
float4x4	g_matLightViewProjection	: LIGHT_VIEW_PROJECTION;
float2		g_vHalfPixel				: HALF_PIXEL_OFFSET;
float		g_fFarClip					: FAR_CLIP_PLANE;
float		g_fDepthBias				: DEPTH_BIAS;
float2		g_vPcfSamples [9]			: PCF_KERNEL;

texture g_texDepthMap					: DEPTH_MAP;
texture g_texShadowMap					: SHADOW_MAP;

#include "../include/gBufferPostprocess.fxh"


sampler depthSampler = sampler_state
{
	Texture = (g_texDepthMap);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = POINT;
	MinFilter = POINT;
	Mipfilter = POINT;
};

sampler shadowSampler = sampler_state
{
	Texture = (g_texShadowMap);
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

float GetShadowOcclusion(float2 projectedTexCoords, float realDistance)
{
	float result = 1;
	if ((saturate(projectedTexCoords.x) == projectedTexCoords.x) && 
		(saturate(projectedTexCoords.y) == projectedTexCoords.y))
	{
		result = 0;
		float shadowTerm = 0.0f;
		for( int i = 0; i < 9; i++ )
		{
			float storedDepthInShadowMap = tex2D(shadowSampler, projectedTexCoords + g_vPcfSamples[i]).x;
			if ((realDistance - g_fDepthBias) <= storedDepthInShadowMap)
			{
				shadowTerm++;				
			}
		}

		shadowTerm /= 9.0f;

		result = shadowTerm;	
	}

	return result;
}

float4 PixelShaderFunction(GBufferVertexOutput input) : COLOR0
{
	float depth = RestoreDepth(tex2D(depthSampler, input.TexCoord));
	float4 position = RestorePosition(input.TexCoord.xy, depth);

	//////////////////////
			
	float4 shadowMapPos = mul(position, g_matLightViewProjection);
	float realDistance = shadowMapPos.z / g_fFarClip;
	float2 projectedTexCoords;
	
	projectedTexCoords[0] = shadowMapPos.x / shadowMapPos.w / 2.0f + 0.5f;
	projectedTexCoords[1] = -shadowMapPos.y / shadowMapPos.w / 2.0f + 0.5f;

	float shadowOcclusion = GetShadowOcclusion(projectedTexCoords, realDistance);

	return float4(shadowOcclusion, shadowOcclusion, shadowOcclusion, 1.0f);
}

technique ShadowOcclusion
{
	pass Pass_1
	{
		ZWriteEnable = false;
		ZEnable = false;		
		AlphaBlendEnable = true;
		FillMode = Solid;

		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
