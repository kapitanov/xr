#ifndef _SHADOWING_H
#define _SHADOWING_H

ShadowTransformedVertexData ShadowMapVertexShader(uniform float farClip, float4 position : POSITION)
{
	ShadowTransformedVertexData output;
	output.Position = mul(position, g_matWorldViewProjection);
	output.Depth = output.Position.z / farClip;
	return output;
}

float4 ShadowMapPixelShader(ShadowTransformedVertexData input) : COLOR0
{			
	return float4(input.Depth, input.Depth, input.Depth, 1); 
}

#endif