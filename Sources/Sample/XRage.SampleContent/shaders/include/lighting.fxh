#ifndef _LIGHTING_H
#define _LIGHTING_H

#define FALLOFF_LINEAR			0
#define FALLOFF_INVERSE_LINEAR	1

#define SHADOW_MAP_ID_0			0
#define SHADOW_MAP_ID_1			1
#define SHADOW_MAP_ID_2			2
#define SHADOW_MAP_ID_3			3

float AttenuationLinear(float r, float R)
{
	return (1.0f - r/R);
}

float AttenuationInverseLinear(float r, float R)
{
	return 1.0f + 1.0f / R + 1.0f / (r - R);
}

float ComputePointAttenuation(
	float3 lightVector,
	float lightRadius,
	uint falloffType)
{
	float r = length(lightVector);
	if(r > lightRadius)
	{
		return 0.0f;
	}
	else
	{
		switch(falloffType)
		{
		case FALLOFF_INVERSE_LINEAR:
			return AttenuationInverseLinear(r, lightRadius);
		
		case FALLOFF_LINEAR:
		default:
			return AttenuationLinear(r, lightRadius);
		}
	}
}

float ToRadians(float x)
{
	return 3.1416f * x / 180.0f;
}

float ComputeSpotAttenuation(
	float3 lightVector,
	float lightRadius,
	float3 lightDirection,
	float outerAngle,
	float innerAngle,
	uint falloffType)
{
	float pointAttenuation = ComputePointAttenuation(lightVector, lightRadius, falloffType);	
	float spotDotLight = saturate(dot(normalize(-lightVector), normalize(lightDirection)));
	return pointAttenuation * smoothstep(cos(outerAngle), cos(innerAngle), spotDotLight);
}

float3 PhongDiffuseLighting(
	float3 normal,
	float3 lightVector,
	float3 diffuseColor)
{
	return diffuseColor * saturate(dot(normal, lightVector));
}

float3 PhongSpecularLighting(
	float3 normal,
	float3 viewDirection,
	float3 lightVector,
	float3 specularColor,
	float specularIntensity,
	float specularPower)
{
	float3 reflectionVector = normalize(reflect(-lightVector, normal));
	float dotProduct = dot(reflectionVector, viewDirection);
	float specularLight = specularIntensity * 
		pow(saturate(dotProduct), specularPower);
	
	return specularColor * specularLight;
}

float3 PhongCombineLight(float3 diffuseLight, float3 specularLight, float attenuation)
{
	return attenuation * (diffuseLight + specularLight);
}

float GetShadowOcclusion(sampler2D shadowOcclusionSampler, float2 texCoord, uint shadowMapId)
{
	switch(shadowMapId)
	{
		case SHADOW_MAP_ID_0:
			return tex2D(shadowOcclusionSampler, texCoord).r;

		case SHADOW_MAP_ID_1:
			return tex2D(shadowOcclusionSampler, texCoord).g;

		case SHADOW_MAP_ID_2:
			return tex2D(shadowOcclusionSampler, texCoord).b;

		case SHADOW_MAP_ID_3:
			return tex2D(shadowOcclusionSampler, texCoord).a;

		default:
			return 1.0f;
	}
}

#endif