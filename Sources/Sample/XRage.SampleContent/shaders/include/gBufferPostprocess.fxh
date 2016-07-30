struct GBufferVertexInput
{
	float3 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct GBufferVertexOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};


GBufferVertexOutput TransformGBufferVertex(GBufferVertexInput input)
{
	GBufferVertexOutput output;
	output.Position = float4(input.Position, 1.0f);
	output.TexCoord = input.TexCoord - g_vHalfPixel;
	return output;
}

float RestoreDepth(float3 depthMapValue)
{
	return depthMapValue.r;
}

float4 RestorePosition(float2 screenPosition, float depth)
{
	float4 position;

	position.x = 2.0f * screenPosition.x - 1.0f;
	position.y = -(2.0f * screenPosition.y - 1.0f);
	position.z = depth;
	position.w = 1.0f;
	position = mul(position, g_matInvertViewProjection);
	position /= position.w;

	return position;
}



float4 ComposeLighting(float4 diffuseColor, float4 lighting)
{
	float3 color = diffuseColor.rgb * lighting.rgb;
	return float4(color, 1);
}