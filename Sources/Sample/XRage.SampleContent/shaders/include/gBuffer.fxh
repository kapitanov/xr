#include "include/vertexData.fxh"

// G-Buffer common definitions

struct GBufferVsOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float2 Depth : TEXCOORD2;
};

struct GBufferPixelData
{ 
	float4 Color : COLOR0;
	float4 Normal : COLOR1;
	float4 Depth : COLOR2;
};

float4 ComputeGBufferVertexPosition(float4 inputPosition)
{
	return mul(float4(inputPosition.xyz, 1), g_matWorldViewProjection);
}

float2 ComputeGBufferVertexDepth(float4 transformedPosition)
{
	return float2(transformedPosition.z, transformedPosition.w);
}

float3x3 ComputeTangentToWorld(float4x4 matWorld, float3 tangent, float3 binormal, float3 normal)
{
	float3 tangentWS = mul(tangent, g_matWorld);
	float3 binormalWS = mul(binormal, g_matWorld);
	float3 normalWS = mul(normal, g_matWorld);

	return float3x3(tangentWS, binormalWS, normalWS);
}

float3x3 ComputeWorldToTangent(float3x3 matTangentToWorld)
{
	return float3x3(normalize(matTangentToWorld[0]), normalize(matTangentToWorld[1]), normalize(matTangentToWorld[2]));
}

float2 ComputeParallaxTexCoordsA(float3 viewDirectionNormalized, float2 texCoords, float height, float2 parallaxMapScaleBias)
{
	height = height * parallaxMapScaleBias.x + parallaxMapScaleBias.y;
	return texCoords + (height * viewDirectionNormalized.xy);
}

float2 ComputeParallaxTexCoordsB(float3 viewDirectionNormalized, float2 texCoords, sampler2D heightMap, float parallaxMapScale)
{
	float3 eyeVector = -viewDirectionNormalized;

	float depth = tex2D(heightMap, texCoords).a;
	float2 halfOffset = eyeVector.xy * depth * parallaxMapScale;

	depth = (depth + tex2D(heightMap, texCoords + halfOffset ).x) * 0.5;
	halfOffset  = eyeVector.xy * depth * parallaxMapScale;

	depth = (depth + tex2D(heightMap, texCoords + halfOffset ).x) * 0.5;
	halfOffset  = eyeVector.xy * depth * parallaxMapScale;
	
	return texCoords + halfOffset;
}

GBufferPixelData ComputeGBufferPixel(
	float2 vsDepthData, 
	float3 diffuseColor, 
	float3 normal, 
	float specularPower, 
	float specularIntensity)
{
	GBufferPixelData output;

	output.Color.rgb = diffuseColor;
	output.Color.a = specularIntensity;
	output.Normal.rgb = normal;
	output.Normal.a = specularPower;
	output.Depth = vsDepthData.x / vsDepthData.y;
		
	return output;
}

float2 ParallaxOcclusionMappingOffset(
		ParallaxMappingTransformedVertex vertex,
		int g_nMinSamples,
		int g_nMaxSamples,		
		sampler2D tNormalHeightMap
	)
{	
	float3 vViewWS   = normalize(vertex.ViewDirection);
	float3 vViewTS   = normalize(ToTangentSpace(vertex, vertex.ViewDirection));
	float3 vNormalWS = normalize(GetNormalWS(vertex));
	float2 texCoord  = vertex.TexCoord;
	float2 vParallaxOffsetTS = vertex.ParallaxOffsetTS;

	//===============================================//
	// Parallax occlusion mapping offset computation //
	//===============================================//

	// Utilize dynamic flow control to change the number of samples per ray 
	// depending on the viewing angle for the surface. Oblique angles require 
	// smaller step sizes to achieve more accurate precision for computing displacement.
	// We express the sampling rate as a linear function of the angle between 
	// the geometric normal and the view direction ray:
	
	int nNumSteps = (int) lerp( g_nMaxSamples, g_nMinSamples, dot( vViewWS, vNormalWS ) );
	//int nNumSteps = (int) lerp( g_nMaxSamples, g_nMinSamples, dot( vViewTS, vNormalTS ) );

	// Intersect the view ray with the height field profile along the direction of
	// the parallax offset ray (computed in the vertex shader. Note that the code 
	// designed specifically to take advantage of the dynamic flow control constructs
	// in HLSL and is very sensitive to specific syntax. When converting to other examples,
	// if still want to use dynamic flow control in the resulting assembly shader,
	// care must be applied.
	// 
	// In the below steps we approximate the height field profile as piecewise linear
	// curve. We find the pair of endpoints between which the intersection between the 
	// height field profile and the view ray is found and then compute line segment
	// intersection for the view ray and the line segment formed by the two endpoints.
	// This intersection is the displacement offset from the original texture coordinate.
	// See the above paper for more details about the process and derivation.
	//

	float fCurrHeight = 0.0;
	float fStepSize   = 1.0 / (float) nNumSteps;
	float fPrevHeight = 1.0;
	float fNextHeight = 0.0;

	int    nStepIndex = 0;
	bool   bCondition = true;

	float2 vTexOffsetPerStep = fStepSize * vParallaxOffsetTS;
	float2 vTexCurrentOffset = texCoord;
	float  fCurrentBound     = 1.0;
	float  fParallaxAmount   = 0.0;

	float2 pt1 = 0;
	float2 pt2 = 0;
	   
	float2 texOffset2 = 0;
	float2 dx = 0, dy = 0;
	

	while ( nStepIndex < nNumSteps ) 
	{
		vTexCurrentOffset -= vTexOffsetPerStep;

		// Sample height map which in this case is stored in the alpha channel of the normal map:
		fCurrHeight = tex2Dgrad( tNormalHeightMap, vTexCurrentOffset, dx, dy ).a;

		fCurrentBound -= fStepSize;

		if ( fCurrHeight > fCurrentBound ) 
		{   
			pt1 = float2( fCurrentBound, fCurrHeight );
			pt2 = float2( fCurrentBound + fStepSize, fPrevHeight );

			texOffset2 = vTexCurrentOffset - vTexOffsetPerStep;

			nStepIndex = nNumSteps + 1;
			fPrevHeight = fCurrHeight;
		}
		else
		{
			nStepIndex++;
			fPrevHeight = fCurrHeight;
		}
	}   

	float fDelta2 = pt2.x - pt2.y;
	float fDelta1 = pt1.x - pt1.y;
	  
	float fDenominator = fDelta2 - fDelta1;
	  
	// SM 3.0 requires a check for divide by zero, since that operation will generate
	// an 'Inf' number instead of 0, as previous models (conveniently) did:
	if ( fDenominator == 0.0f )
	{
		fParallaxAmount = 0.0f;
	}
	else
	{
		fParallaxAmount = (pt1.x * fDelta2 - pt2.x * fDelta1 ) / fDenominator;
	}
	  
	float2 vParallaxOffset = vParallaxOffsetTS * (1 - fParallaxAmount );

	// The computed texture offset for the displaced point on the pseudo-extruded surface:
	float2 texSampleBase = texCoord - vParallaxOffset;
		      
	return texSampleBase;
}