float4x4	g_matWorld;
float4x4	g_matViewProjection;
float		g_fFarClip;

struct VertexShaderOutput
{
	float4 position : POSITION;
	float depth		: TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(float4 position : POSITION)
{
	VertexShaderOutput OUT = (VertexShaderOutput)0;

	OUT.position = mul(mul(position, g_matWorld), g_matViewProjection);
	OUT.depth = OUT.position.z / g_fFarClip;
	
	return  OUT;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return float4(input.depth, input.depth, input.depth, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
