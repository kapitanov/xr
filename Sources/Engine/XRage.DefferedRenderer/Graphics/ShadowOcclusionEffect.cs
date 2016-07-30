using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
	internal sealed class ShadowOcclusionEffect : Shader<ShadowMapRenderer>
	{
		public ShadowOcclusionEffect(ShadowMapRenderer renderer)
			: base(renderer)
		{
			Initialize();
		}

		public override void Initialize()
		{
			LoadShader("shaders/screen_space_shadows/shadow_occlusion_mask");

			matInvertViewProjection = GetParameterBySemantic("INV_VIEW_PROJECTION");
			matLightViewProjection = GetParameterBySemantic("LIGHT_VIEW_PROJECTION");
			vHalfPixel = GetParameterBySemantic("HALF_PIXEL_OFFSET");
			fFarClip = GetParameterBySemantic("FAR_CLIP_PLANE");
			fDepthBias = GetParameterBySemantic("DEPTH_BIAS");
			vPcfKernel = GetParameterBySemantic("PCF_KERNEL");
			texDepthMap = GetParameterBySemantic("DEPTH_MAP");
			texShadowMap = GetParameterBySemantic("SHADOW_MAP");

			UseTechnique("ShadowOcclusion");

			float shadowMapTexelSize = 1.0f / Renderer.ShadowMapSize;

			var pcfKernel = new[]
			{
				new Vector2(0.0f, 0.0f),
				new Vector2(-shadowMapTexelSize, 0.0f),
				new Vector2(shadowMapTexelSize, 0.0f),
				new Vector2(0.0f, -shadowMapTexelSize),
				new Vector2(-shadowMapTexelSize, -shadowMapTexelSize),
				new Vector2(shadowMapTexelSize, -shadowMapTexelSize),
				new Vector2(0.0f, shadowMapTexelSize),
				new Vector2(-shadowMapTexelSize, shadowMapTexelSize),
				new Vector2(shadowMapTexelSize, shadowMapTexelSize)
			};

			vPcfKernel.SetValue(pcfKernel);
		}

		public Effect Bind(ShadowOcclusionQuery query)
		{
			matInvertViewProjection.SetValue(Matrix.Invert(query.RenderCamera.View * query.RenderCamera.Projection));
			matLightViewProjection.SetValue(query.LightCamera.View * query.LightCamera.Projection);
			vHalfPixel.SetValue(Renderer.DeferredRenderer.HalfPixel);
			fFarClip.SetValue(query.RenderCamera.FarClippingPlane);
			fDepthBias.SetValue(Renderer.Bias);
			texDepthMap.SetValue(Renderer.DeferredRenderer.GBuffer.DepthBuffer);
			texShadowMap.SetValue(Renderer.ShadowMap);

			return Effect;
		}

		private EffectParameter matInvertViewProjection;
		private EffectParameter matLightViewProjection;
		private EffectParameter vHalfPixel;
		private EffectParameter fFarClip;
		private EffectParameter fDepthBias;
		private EffectParameter vPcfKernel;
		private EffectParameter texDepthMap;
		private EffectParameter texShadowMap;
	}
}
