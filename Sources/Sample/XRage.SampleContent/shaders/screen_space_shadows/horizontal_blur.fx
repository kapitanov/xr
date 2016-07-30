sampler TextureSampler : register(s0);

#define SAMPLE_COUNT 7

float2 g_vSampleOffsets[SAMPLE_COUNT];
float g_fSampleWeights[SAMPLE_COUNT];



float4 GaussianBlurShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 c = 0;
	for (int i = 0; i < SAMPLE_COUNT; i++)
	{
		c += tex2D(TextureSampler, texCoord + g_vSampleOffsets[i]) * g_fSampleWeights[i];
	}
	
	return c;
}

technique GaussianHorizontalBlur
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 GaussianBlurShader();
	}
}
