using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace AISTek.XRage.Graphics
{
	internal class ShadowMapRenderer : Renderer
	{
		public ShadowMapRenderer(DeferredRenderer deferredRenderer)
			: base(deferredRenderer.Game)
		{
			DeferredRenderer = deferredRenderer;
			IsShadowingEnabled = true;
			ShadowMapSize = 1024;
			Bias = 0.0010f;
		}

		internal FullScreenQuad FullScreenQuad { get; private set; }

		internal Effect ShadowMapShader { get; set; }

		private ShadowOcclusionEffect ShadowOcclusionEffect { get; set; }

		private SoftShadowEffect SoftShadowEffect { get; set; }

		#region Renter targets

		internal RenderTarget2D ShadowMap { get; set; }

		internal RenderTarget2D ShadowOcclusionMask { get; set; }

		internal RenderTarget2D ShadowOcclusion { get; set; }

		internal RenderTarget2D ShadowOcclusionDisabled { get; set; }


		#endregion

		public Vector2[] PcfKernel { get; private set; }

		public Vector3[] GaussianKernel { get; private set; }

		public DeferredRenderer DeferredRenderer { get; private set; }

		public Texture2D ShadowOcclusionMap { get; private set; }

		public ICamera Camera { get; private set; }

		public ICamera LightCamera { get; private set; }

		public bool IsShadowingEnabled { get; set; }

		private Matrix CameraWorldViewProjection { get; set; }

		public int ShadowMapSize { get; set; }

		public float Bias { get; set; }

		public override void Initialize()
		{
			ShadowMapSize = Graphics.GraphicsSettings.GetShadowMapSize();

			FullScreenQuad = new FullScreenQuad(GraphicsDevice);

			ShadowMap = new RenderTarget2D(
				GraphicsDevice,
				ShadowMapSize,
				ShadowMapSize,
				false,
				SurfaceFormat.Single,
				DepthFormat.Depth24,
				0,
				RenderTargetUsage.PlatformContents);

			ShadowOcclusionMask = new RenderTarget2D(
				GraphicsDevice,
				GraphicsDevice.Viewport.Width,
				GraphicsDevice.Viewport.Height,
				false,
				SurfaceFormat.Color,
				DepthFormat.None);

			ShadowOcclusion = new RenderTarget2D(
				GraphicsDevice,
				GraphicsDevice.Viewport.Width,
				GraphicsDevice.Viewport.Height,
				false,
				SurfaceFormat.Color,
				DepthFormat.None);

			ShadowOcclusionDisabled = new RenderTarget2D(
				GraphicsDevice,
				1,
				1,
				false,
				SurfaceFormat.Color,
				DepthFormat.None);

			DrawDisabledShadowOcclusionMap();

			ShadowOcclusionMap = ShadowOcclusionDisabled;

			ShadowOcclusionEffect = new ShadowOcclusionEffect(this);
			SoftShadowEffect = new SoftShadowEffect(this);

			ShadowMapShader = Game.Content.Load<Effect>("shaders/screen_space_shadows/shadow_map");
		}

		public void Update()
		{
			if (Game.Input.GetAction("shadowOcclusion.incBias").IsOn)
			{
				Bias *= 1.1f;
				Debug.Print("Bias -> {0}", Bias);
			}

			if (Game.Input.GetAction("shadowOcclusion.decBias").IsOn)
			{
				Bias *= 0.9f;
				Debug.Print("Bias <- {0}", Bias);
			}

			if (Game.Input.GetAction("shadowOcclusion.enable").IsOn)
			{
				IsShadowingEnabled = true;
				Debug.Print("shadow_maps: on");
			}

			if (Game.Input.GetAction("shadowOcclusion.disable").IsOn)
			{
				IsShadowingEnabled = false;
				Debug.Print("shadow_maps: off");
			}
			///////////////////////
			if (Game.Input.GetAction("shadowOcclusion.size128").IsOn)
			{
				ShadowMapSize = 128;
				ShadowMap = new RenderTarget2D(
					GraphicsDevice,
					ShadowMapSize,
					ShadowMapSize,
					false,
					SurfaceFormat.Single,
					DepthFormat.Depth24,
					0,
					RenderTargetUsage.PlatformContents);
				Debug.Print("shadow_maps: 128x128");
			}
			if (Game.Input.GetAction("shadowOcclusion.size256").IsOn)
			{
				ShadowMapSize = 256;
				ShadowMap = new RenderTarget2D(
					GraphicsDevice,
					ShadowMapSize,
					ShadowMapSize,
					false,
					SurfaceFormat.Single,
					DepthFormat.Depth24,
					0,
					RenderTargetUsage.PlatformContents);
				Debug.Print("shadow_maps: 256x256");
			}
			if (Game.Input.GetAction("shadowOcclusion.size512").IsOn)
			{
				ShadowMapSize = 512;
				ShadowMap = new RenderTarget2D(
					GraphicsDevice,
					ShadowMapSize,
					ShadowMapSize,
					false,
					SurfaceFormat.Single,
					DepthFormat.Depth24,
					0,
					RenderTargetUsage.PlatformContents);
				Debug.Print("shadow_maps: 512x512");
			}
			if (Game.Input.GetAction("shadowOcclusion.size1024").IsOn)
			{
				ShadowMapSize = 1024;
				ShadowMap = new RenderTarget2D(
					GraphicsDevice,
					ShadowMapSize,
					ShadowMapSize,
					false,
					SurfaceFormat.Single,
					DepthFormat.Depth24,
					0,
					RenderTargetUsage.PlatformContents);
				Debug.Print("shadow_maps: 1024x1024");
			}
			if (Game.Input.GetAction("shadowOcclusion.size2048").IsOn)
			{
				ShadowMapSize = 2048;
				ShadowMap = new RenderTarget2D(
					GraphicsDevice,
					ShadowMapSize,
					ShadowMapSize,
					false,
					SurfaceFormat.Single,
					DepthFormat.Depth24,
					0,
					RenderTargetUsage.PlatformContents);
				Debug.Print("shadow_maps: 2048x2048");
			}
			if (Game.Input.GetAction("shadowOcclusion.size4096").IsOn)
			{
				ShadowMapSize = 4096;
				ShadowMap = new RenderTarget2D(
					GraphicsDevice,
					ShadowMapSize,
					ShadowMapSize,
					false,
					SurfaceFormat.Single,
					DepthFormat.Depth24,
					0,
					RenderTargetUsage.PlatformContents);
				Debug.Print("shadow_maps: 4096x4096");
			}
			///////////////////
			if (Game.Input.GetAction("shadowOcclusion.useMaterialShader").IsOn)
			{
				useMaterialShader = true;
				Debug.Print("useMaterialShader");
			}

			if (Game.Input.GetAction("shadowOcclusion.useRendererShader").IsOn)
			{
				useMaterialShader = false;
				Debug.Print("useRendererShader");
			}
		}

		private void DrawDisabledShadowOcclusionMap()
		{
			GraphicsDevice.SetRenderTarget(ShadowOcclusionDisabled);
			GraphicsDevice.Clear(Color.White);
			GraphicsDevice.SetRenderTarget(null);
		}

		public override void DrawFrame(ICamera camera, GameTime gameTime)
		{
			throw new NotImplementedException("ShadowMapRenderer.DrawFrame() method is not implemented. Use specific methods instead.");
		}

		private void ExecuteDisabledQuery(ShadowOcclusionQuery query)
		{
			query.SetLightCamera(nullCamera);
			query.SetShadowOcclusion(ShadowOcclusionDisabled);
		}

		public void RenderShadowOcclusion(ShadowOcclusionQuery query)
		{
			if (!IsShadowingEnabled)
			{
				ExecuteDisabledQuery(query);
				return;
			}

			Camera = query.RenderCamera;

			// Select appropriate shadow type
			switch (query.Light.Type)
			{
				case LightType.Spot:
					ExecuteSpotLightQuery(query);
					break;

				default:
					ExecuteSpotLightQuery(query);
					break;
			}
		}

		private void ExecuteSpotLightQuery(ShadowOcclusionQuery query)
		{
			query.SetLightCamera(new SpotLightCamera(query.Light));
			DrawShadowMapGeometry(query);

			DrawShadowOcclusion(query);

			GraphicsDevice.SetRenderTarget(null);

			if (query.UseSoftShadows || false)
			{
				var result = SoftenShadowOcclusion(query);
				query.SetShadowOcclusion(result);
			}
			else
			{
				query.SetShadowOcclusion(ShadowOcclusion);
			}
		}

		#region Render shadow map

		bool useMaterialShader = false;

		private void DrawShadowMapGeometry(ShadowOcclusionQuery query)
		{
			var lightCamera = query.LightCamera;
			GraphicsDevice.SetRenderTarget(ShadowMap);

			GraphicsDevice.BlendState = BlendState.Opaque;
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			GraphicsDevice.Clear(Color.White);

			lightCamera.AssignCamera(Graphics);

			var pass = RenderPassDescriptor.ShadowMap(lightCamera);
			Graphics.RenderChunkManager.GeometryRenderChunks.Clear();
			Graphics.RenderChunkProviders.QueryForChunks(ref pass);

			var geometryChunkList = Graphics.RenderChunkManager.GeometryRenderChunks;

			ShadowMapShader.Parameters["g_matViewProjection"].SetValue(lightCamera.View * lightCamera.Projection);
			ShadowMapShader.Parameters["g_fFarClip"].SetValue(Camera.FarClippingPlane);

			

			foreach (var geometry in geometryChunkList)
			{
				Graphics.Variables.Set(VariableMatrixId.World, geometry.WorldTransform);
				MatrixOperations.Perform(Graphics.Variables);

				GraphicsDevice.SetVertexBuffers(
					geometry.VertexStreams
					.Select(buffer => new VertexBufferBinding(buffer))
					.ToArray());
				GraphicsDevice.Indices = geometry.Indices;

				geometry.Material.CurrentTechnique = "CreateShadowMap";
				var passes = useMaterialShader
					? geometry.Material.BindMaterial(Graphics)
					: ShadowMapShader.CurrentTechnique.Passes.Count;

				ShadowMapShader.Parameters["g_matWorld"].SetValue(geometry.WorldTransform);


				for (int i = 0; i < passes; i++)
				{
					if (useMaterialShader)
						geometry.Material.BeginPass(i);
					else
						ShadowMapShader.CurrentTechnique.Passes[i].Apply();

					GraphicsDevice.DrawIndexedPrimitives(
					   geometry.Type,
					   geometry.VertexStreamOffset,
					   0,
					   geometry.NumVertices,
					   geometry.StartIndex,
					   geometry.PrimitiveCount);

					geometry.Material.EndPass();
				}

				Graphics.RenderingStatistics.IncrementPrimitivesCount(geometry.PrimitiveCount * passes);
			}

			GraphicsDevice.SetRenderTarget(null);
			Graphics.RenderingStatistics.IncrementGeometryChunksCount(geometryChunkList.Count);
		}

		#endregion

		#region Render shadow occlusion

		private void DrawShadowOcclusion(ShadowOcclusionQuery query)
		{
			GraphicsDevice.SetRenderTarget(ShadowOcclusion);
			var technique = ShadowOcclusionEffect.Bind(query);
			FullScreenQuad.Draw(GraphicsDevice, technique);
		}

		#endregion

		#region Blur shadow occlusion

		private Texture2D SoftenShadowOcclusion(ShadowOcclusionQuery query)
		{
			GraphicsDevice.SetRenderTarget(ShadowOcclusionMask);
			FullScreenQuad.Draw(GraphicsDevice, SoftShadowEffect.Bind(ShadowOcclusion));

			GraphicsDevice.SetRenderTarget(ShadowOcclusion);
			FullScreenQuad.Draw(GraphicsDevice, SoftShadowEffect.Bind(ShadowOcclusionMask));

			GraphicsDevice.SetRenderTarget(null);
			return ShadowOcclusion;
		}

		#endregion

		public override void SaveSettings()
		{
			Graphics.GraphicsSettings.SetShadowMapSize(ShadowMapSize);
		}

		private ICamera nullCamera = new NullCamera();
	}
}
