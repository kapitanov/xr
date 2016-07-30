using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
	internal sealed class SoftShadowEffect : Shader<ShadowMapRenderer>
	{
	   public SoftShadowEffect(ShadowMapRenderer renderer)
		   : base(renderer)
		{
			Initialize();
		}
		
		public override void Initialize()
		{
			LoadShader("shaders/screen_space_shadows/shadow_blur");

			vHorizontalBlurKernel = GetParameterBySemantic("HORIZONTAL_BLUR_KERNEL");
			vVerticalBlurKernel = GetParameterBySemantic("VERTICAL_BLUR_KERNEL");
			texSource = GetParameterBySemantic("SOURCE_TEXTURE");

			var screenTexelSize = new Vector2(
				1.0f / Renderer.GraphicsDevice.Viewport.Width, 
				1.0f / Renderer.GraphicsDevice.Viewport.Height);

			vHorizontalBlurKernel.SetValue(CreateGaussianKernel(screenTexelSize, 15, true));
			vVerticalBlurKernel.SetValue(CreateGaussianKernel(screenTexelSize, 15, false));

			UseTechnique("Blur");
		}

		public Effect Bind(Texture2D source)
		{
			texSource.SetValue(source);
			return Effect;
		}
	   
		private Vector3[] CreateGaussianKernel(Vector2 texelSize, int kernelSize, bool isHorizontal)
		{
			var gaussianKernel = new Vector3[kernelSize];

			// Get center texel offset and weight
			gaussianKernel[0] = new Vector3(0.0f, 0.0f, 1.0f * XMath.GetGaussianDistribution(0, 0, 2.0f));

			// Get other texels offsets and weights
			for (int i = 1; i < kernelSize; i += 2)
			{
				if (isHorizontal)
				{
					gaussianKernel[i] = new Vector3(i * texelSize.X, 0.0f, 2.0f * XMath.GetGaussianDistribution(i, 0, 3.0f));
					gaussianKernel[i + 1] = new Vector3(-i * texelSize.X, 0.0f, 2.0f * XMath.GetGaussianDistribution(i + 1, 0, 3.0f));
				}
				else
				{
					gaussianKernel[i] = new Vector3(0.0f, i * texelSize.Y, 2.0f * XMath.GetGaussianDistribution(0, i, 3.0f));
					gaussianKernel[i + 1] = new Vector3(0.0f, -i * texelSize.Y, 2.0f * XMath.GetGaussianDistribution(0, i + 1, 3.0f));
				}
			}

			return gaussianKernel;
		}

		private EffectParameter vHorizontalBlurKernel;
		private EffectParameter vVerticalBlurKernel;
		private EffectParameter texSource;
	}
}
