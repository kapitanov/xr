////////////////////////////////////////////////////////////////////
// GBuffer postprocessing shader for spot light	with shadow map   //
////////////////////////////////////////////////////////////////////

float4x4 g_matInvertViewProjection	: INV_VIEW_PROJECTION;
float3 g_vLightPosition				: LIGHT_POSITION;
float3 g_vLightDirection			: LIGHT_DIRECTION;
float g_fLightOuterConeAngle		: LIGHT_OUTER_CONE_ANGLE;
float g_fLightInnerConeAngle		: LIGHT_INNER_CONE_ANGLE;
float3 g_vCameraPosition			: CAMERA_POSITION;
float g_fLightRadius				: LIGHT_RADIUS;
uint g_iFalloffType					: LIGHT_FALLOFF;
float g_fLightIntensity				: LIGHT_INTENSITY;
float2 g_vHalfPixel					: HALF_PIXEL_OFFSET;
float3 g_cDiffuseColor				: LIGHT_DIFFUSE_COLOR;
float3 g_cSpecularColor				: LIGHT_SPECULAR_COLOR;
texture g_texColorMap				: GBUFFER_DIFFUSE_MAP; 
texture g_texNormalMap				: GBUFFER_NORMAL_MAP;
texture g_texDepthMap				: GBUFFER_DEPTH_MAP;
texture g_texShadowMap				: SHADOW_OCCLUSION_MAP;

#include "../include/gBufferPostprocess.fxh"
#include "../include/lighting.fxh"
#include "../include/common.fxh"

sampler colorSampler = TEXTURE_SAMPLER_POINT(g_texColorMap, CLAMP);
sampler normalSampler = TEXTURE_SAMPLER_POINT(g_texNormalMap, CLAMP);
sampler depthSampler = TEXTURE_SAMPLER_POINT(g_texDepthMap, CLAMP);
sampler shadowSampler = TEXTURE_SAMPLER_POINT(g_texShadowMap, CLAMP);

float4 ShadedLightPixelShader(GBufferVertexOutput input) : COLOR0
{
	float4 normalData = tex2D(normalSampler, input.TexCoord);
	float3 normal = 2.0f * normalData.xyz - 1.0f;
 
	float specularPower = normalData.a * 256.0f;
	float specularIntensity = tex2D(colorSampler, input.TexCoord).a;
	
	float depth = RestoreDepth(tex2D(depthSampler, input.TexCoord));
	float4 position = RestorePosition(input.TexCoord.xy, depth);
	float3 lightVector = g_vLightPosition - position;
	float3 viewDirection = normalize(g_vCameraPosition - position);	

	float shadowOcclusion = tex2D(shadowSampler, input.TexCoord);
	
	if(shadowOcclusion > 0.0f)
	{
		float attenuation = ComputeSpotAttenuation(
			lightVector, 
			g_fLightRadius, 
			g_vLightDirection,
			g_fLightOuterConeAngle,
			g_fLightInnerConeAngle,
			g_iFalloffType);

		attenuation *= shadowOcclusion;

		if(attenuation > 0.0f)
		{
			lightVector = normalize(lightVector);
			float3 diffuseLight = PhongDiffuseLighting(normal, lightVector, g_cDiffuseColor);
			float3 specularLight = PhongSpecularLighting(normal, viewDirection, 
													 lightVector, g_cSpecularColor, specularIntensity, specularPower);
			float3 combinedLight = PhongCombineLight(diffuseLight, specularLight, attenuation * g_fLightIntensity);
			return float4(combinedLight.xyz, 1.0f);
		} 
		else
		{
			return float4(0.0f, 0.0f, 0.0f, 1.0f);
		}
	}
	else
	{
		return float4(0.0f, 0.0f, 0.0f, 1.0f);
	}
}

float4 NonshadedLightPixelShader(GBufferVertexOutput input) : COLOR0
{
	float4 normalData = tex2D(normalSampler, input.TexCoord);
	float3 normal = 2.0f * normalData.xyz - 1.0f;
 
	float specularPower = normalData.a * 256.0f;
	float specularIntensity = tex2D(colorSampler, input.TexCoord).a;
	
	float depth = RestoreDepth(tex2D(depthSampler, input.TexCoord));
	float4 position = RestorePosition(input.TexCoord.xy, depth);
	float3 lightVector = g_vLightPosition - position;
	float3 viewDirection = normalize(g_vCameraPosition - position);	

	float attenuation = ComputeSpotAttenuation(
		lightVector, 
		g_fLightRadius, 
		g_vLightDirection,
		g_fLightOuterConeAngle,
		g_fLightInnerConeAngle,
		g_iFalloffType);

	if(attenuation > 0.0f)
	{
		lightVector = normalize(lightVector);
		float3 diffuseLight = PhongDiffuseLighting(normal, lightVector, g_cDiffuseColor);
		float3 specularLight = PhongSpecularLighting(normal, viewDirection, 
												 lightVector, g_cSpecularColor, specularIntensity, specularPower);
		float3 combinedLight = PhongCombineLight(diffuseLight, specularLight, attenuation * g_fLightIntensity);
		return float4(combinedLight.xyz, 1.0f);
	} 
	else
	{
		return float4(0.0f, 0.0f, 0.0f, 1.0f);
	}
}

technique RenderSpotLightWithShadowOcclusion
{
	pass Pass_1
	{
		ZWriteEnable = false;
		ZEnable = false;		
		AlphaBlendEnable = true;
		FillMode = Solid;

		VertexShader = compile vs_2_0 TransformGBufferVertex();
		PixelShader = compile ps_2_0 ShadedLightPixelShader();
	}
}

technique RenderSpotLight
{
	pass Pass_1
	{
		ZWriteEnable = false;
		ZEnable = false;		
		AlphaBlendEnable = true;
		FillMode = Solid;

		VertexShader = compile vs_2_0 TransformGBufferVertex();
		PixelShader = compile ps_2_0 NonshadedLightPixelShader();
	}
}
