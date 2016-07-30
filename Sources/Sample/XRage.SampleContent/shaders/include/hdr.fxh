float g_fMiddleGrey = 0.6f; 
float g_fMaxLuminance = 16.0f; 

static const float3 LUM_CONVERT = float3(0.299f, 0.587f, 0.114f); 

float3 HdrToneMap(float3 color, float3 luminance) 
{ 
	// Calculate the luminance of the current pixel 
	float fLumPixel = dot(color, LUM_CONVERT);     

	// Apply the modified operator (Eq. 4) 
	float fLumScaled = (fLumPixel * g_fMiddleGrey) / luminance;     
	float fLumCompressed = (fLumScaled * (1 + (fLumScaled / (g_fMaxLuminance * g_fMaxLuminance)))) / (1 + fLumScaled); 
	return fLumCompressed * color; 
} 
