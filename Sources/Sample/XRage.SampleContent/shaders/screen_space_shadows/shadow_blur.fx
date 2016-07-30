#define KERNEL_SIZE	15

float3	g_vHorizontalBlurKernel [KERNEL_SIZE]	: HORIZONTAL_BLUR_KERNEL;
float3	g_vVerticalBlurKernel [KERNEL_SIZE]		: VERTICAL_BLUR_KERNEL;

texture g_texSource								: SOURCE_TEXTURE;

sampler sourceSampler = sampler_state
{
	Texture = (g_texSource);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MagFilter = POINT;
	MinFilter = POINT;
	Mipfilter = POINT;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoord	: TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoord	: TEXCOORD0;
};

VertexShaderOutput BlurVertexShader(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = input.Position;
	output.TexCoord = input.TexCoord;
	
	return output;
}

float4 PerformBlur(float2 texCoord : TEXCOORD0, uniform float3 kernel[KERNEL_SIZE]) : COLOR0
{
	float4 acc = float4(0.0f, 0.0f, 0.0f, 0.0f);
   
	for(int i = 0; i < KERNEL_SIZE; i++ )
	{
		float3 kernelValue = kernel[i];
		acc += tex2D(sourceSampler, texCoord + kernelValue.xy) * kernelValue.z;
	}

	return acc;
}

technique Blur
{
	pass HorizontalPass
	{
		VertexShader = compile vs_2_0 BlurVertexShader();
		PixelShader = compile ps_2_0 PerformBlur(g_vHorizontalBlurKernel);
	}

	pass VerticalPass
	{
		VertexShader = compile vs_2_0 BlurVertexShader();
		PixelShader = compile ps_2_0 PerformBlur(g_vVerticalBlurKernel);
	}
}