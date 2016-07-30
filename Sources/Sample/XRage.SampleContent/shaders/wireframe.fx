float4x4 g_matWorldViewProjection		: WORLDVIEWPROJECTION;
float4x4 g_matWorld						: WORLD;
float3 g_vColor							: WIREFRAME_COLOR;

#include "include/gBuffer.fxh"

struct VertexData
{
	float4 	Position		: POSITION;
	float3	Color			: COLOR0;
};

struct TransformedVertexData
{
	float4 Position			: POSITION0;
	float2 Depth			: TEXCOORD1;
};

TransformedVertexData VertexShaderFunction(VertexData input)
{
	TransformedVertexData output;
	output.Position = ComputeGBufferVertexPosition(input.Position);
	output.Depth = ComputeGBufferVertexDepth(output.Position);
	
	return output;
}

GBufferPixelData PixelShaderFunction(TransformedVertexData input)
{
	float3 normal = float3(0, 0, 1);
	normal = 0.5f * (normalize(normal)+ 1.0f);

	return ComputeGBufferPixel(
		input.Depth,
		g_vColor,
		normal,
		0,
		0);
}

technique RenderGBuffer
{
	pass Pass_1
	{
		ZWriteEnable = true;
		ZEnable = true;		
		AlphaBlendEnable = false;
		FillMode = Wireframe;
		CullMode = None; 

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
