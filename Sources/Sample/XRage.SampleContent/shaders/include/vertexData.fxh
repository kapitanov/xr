#ifndef _VERTEX_DATA_H
#define _VERTEX_DATA_H

//===============================================//
// Input vertex formats                          //
//===============================================//

struct NormalMappingVertex
{
	float4 Position		: POSITION;
	float2 TexCoord		: TEXCOORD0;
	float3 Normal		: NORMAL0;
	float3 Binormal		: BINORMAL0;
	float3 Tangent		: TANGENT0;
};

struct ParallaxMappingVertex
{
	float4 Position		: POSITION;
	float2 TexCoord		: TEXCOORD0;
	float3 Normal		: NORMAL0;
	float3 Binormal		: BINORMAL0;
	float3 Tangent		: TANGENT0;
};

struct ShadowVertexData
{
	float4 Position		: POSITION;
};

//===============================================//
// Transformed vertex formats                    //
//===============================================//

struct NormalMappedTransformedVertex
{
	float4 Position			: POSITION0;
	float2 TexCoord			: TEXCOORD0;
	float2 Depth			: TEXCOORD1;
	float3x3 TangentToWorld	: TEXCOORD2;
};


struct ParallaxMappingTransformedVertex
{
	float4 Position			: POSITION0;
	float2 TexCoord			: TEXCOORD0;
	float2 Depth			: TEXCOORD1;
	float3x3 TangentToWorld	: TEXCOORD2;
	float3 ViewDirection	: COLOR0;	
	float2 ParallaxOffsetTS	: COLOR1;
};

float3 GetNormalWS(ParallaxMappingTransformedVertex vertex)
{
	return vertex.TangentToWorld[2];
}

float3 ToTangentSpace(ParallaxMappingTransformedVertex vertex, float3 v)
{
	return mul(v, vertex.TangentToWorld);
}

struct ShadowTransformedVertexData
{
	float4 Position			: POSITION;
	float  Depth			: TEXCOORD1;
};

#endif