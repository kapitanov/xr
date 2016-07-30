using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using AISTek.XRage.Graphics.Profiling;

namespace AISTek.XRage.Graphics
{
    public class DeferredRenderer : Renderer
    {
        public DeferredRenderer(XGame game)
            : base(game)
        {
            Visualizer = new GBufferVisualizer(game, this);
            ShadowMapRenderer = new ShadowMapRenderer(this);
        }

        #region Properties

        #region General properties

        public ICamera Camera { get; private set; }

        internal FullScreenQuad FullScreenQuad { get; private set; }

        internal GBufferVisualizer Visualizer { get; private set; }

        internal ShadowMapRenderer ShadowMapRenderer { get; private set; }

        internal Vector2 HalfPixel { get; set; }

        internal Matrix CameraInViewProjection { get; private set; }

        private BlendState DrawLightingBlendState { get; set; } 

        #endregion

        #region Render targets
        
        public GBuffer GBuffer { get; private set; }

        internal RenderTarget2D LightBuffer { get; private set; }

        internal RenderTarget2D BackBuffer { get; private set; }

        #endregion

        #region Shaders

        private ClearGeomertyBufferShader ClearGeomertyBufferShader { get; set; }

        private DirectionalLightShader DirectionalLightShader { get; set; }

        private PointLightShader PointLightShader { get; set; }

        private SpotLightShader SpotLightShader { get; set; }

        private ComposeLightingShader ComposeLightingShader { get; set; }

        #endregion

        #endregion

        #region Initialization

        public override void Initialize()
        {
            FullScreenQuad = new FullScreenQuad(Graphics.GraphicsDevice);
            HalfPixel = new Vector2(
                0.5f / Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                0.5f / Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight);

            DrawLightingBlendState = new BlendState
            {
                AlphaBlendFunction = BlendFunction.Add,
                AlphaDestinationBlend = Blend.One,
                AlphaSourceBlend = Blend.One,
                ColorBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.One,
                ColorDestinationBlend = Blend.One
            };

            CreateRenderTargets();
            LoadShaders();

            ShadowMapRenderer.Initialize();
        }

        private void LoadShaders()
        {
            ClearGeomertyBufferShader = new ClearGeomertyBufferShader(this);
            SpotLightShader = new SpotLightShader(this);
            PointLightShader = new PointLightShader(this);
            DirectionalLightShader = new DirectionalLightShader(this);
            ComposeLightingShader = new ComposeLightingShader(this);
        }

        private void CreateRenderTargets()
        {
            GBuffer = new GBuffer(this);
            GBuffer.Initialize(Graphics.GraphicsSettings.GetGBufferFormat());

            LightBuffer = new RenderTarget2D(
                Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.Viewport.Width,
                Graphics.GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.HdrBlendable,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);

            BackBuffer = new RenderTarget2D(
                Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.Viewport.Width,
                Graphics.GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
        }

        #endregion

        #region Save settings

        public override void SaveSettings()
        {
            Graphics.GraphicsSettings.SetGBufferFormat(GBuffer.Format);
        }

        #endregion

        #region Frame drawing

        public override void DrawFrame(ICamera camera, GameTime gameTime)
        {
            Pix.BeginEvent("DeferredRenderer::DrawFrame");
            ShadowMapRenderer.Update();
            Camera = camera;

            CameraInViewProjection = Matrix.Invert(Camera.View * Camera.Projection);

            GBuffer.BindRenderTargets();
            FullScreenQuad.Draw(Graphics.GraphicsDevice, ClearGeomertyBufferShader.Bind());
            Pix.BeginEvent("DeferredRenderer::DrawSolidGeometry");
            DrawSolidGeometry();
            Pix.EndEvent();

            GraphicsDevice.SetRenderTarget(LightBuffer);
            GraphicsDevice.Clear(Color.Black);

            Pix.BeginEvent("DeferredRenderer::DrawLighting");
            DrawLighting();
            Pix.EndEvent();

            GraphicsDevice.SetRenderTarget(BackBuffer);
            Pix.BeginEvent("DeferredRenderer::ComposeFrame");
            ComposeFrame();
            Pix.EndEvent();
            GraphicsDevice.SetRenderTarget(null);

            Pix.EndEvent();

            Visualizer.Draw();
        }

        #region Solid geometry rendering

        #region G-Buffer rendering

        private void DrawSolidGeometry()
        {
            var pass = RenderPassDescriptor.SolidGeometry(Camera);
            Graphics.RenderChunkManager.ClearChunks();
            Graphics.RenderChunkProviders.QueryForChunks(ref pass);

            foreach (var chunk in Graphics.RenderChunkManager.GeometryRenderChunks)
            {
                Graphics.Variables.Set(VariableMatrixId.World, chunk.WorldTransform);
                MatrixOperations.Perform(Graphics.Variables);

                Graphics.GraphicsDevice.SetVertexBuffers(
                    chunk.VertexStreams
                    .Select(buffer => new VertexBufferBinding(buffer))
                    .ToArray());
                Graphics.GraphicsDevice.Indices = chunk.Indices;

                chunk.Material.CurrentTechnique = "RenderGBuffer";
                var passes = chunk.Material.BindMaterial(Graphics);

                for (int i = 0; i < passes; i++)
                {
                    chunk.Material.BeginPass(i);

                    Graphics.GraphicsDevice.DrawIndexedPrimitives(
                       chunk.Type,
                       chunk.VertexStreamOffset,
                       0,
                       chunk.NumVertices,
                       chunk.StartIndex,
                       chunk.PrimitiveCount);

                    chunk.Material.EndPass();
                }

                Graphics.RenderingStatistics.IncrementPrimitivesCount(chunk.PrimitiveCount * passes);
            }

            Graphics.RenderingStatistics.IncrementGeometryChunksCount(Graphics.RenderChunkManager.GeometryRenderChunks.Count);
        }

        #endregion

        #region Draw lighting
        
        private void DrawLighting()
        {
            // Query for light chunks
            var pass = RenderPassDescriptor.Lighting(Camera);
            Graphics.RenderChunkManager.ClearChunks();
            Graphics.RenderChunkProviders.QueryForChunks(ref pass);

            var lightRenderChunks = Graphics.RenderChunkManager.LightRenderChunks;
            var directionalLights = lightRenderChunks.Where(chunk => chunk.Type == LightType.Directional).ToList();
            var pointLights = lightRenderChunks.Where(chunk => chunk.Type == LightType.Point).ToList();
            var spotLights = lightRenderChunks.Where(chunk => chunk.Type == LightType.Spot).ToList();

            GraphicsDevice.SetRenderTarget(LightBuffer);

            Camera.AssignCamera(Graphics);
            Graphics.Variables.Set(VariableMatrixId.World, Matrix.Identity);
            MatrixOperations.Perform(Graphics.Variables);

            Graphics.GraphicsDevice.BlendState = DrawLightingBlendState;
            Graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            Graphics.GraphicsDevice.Clear(new Color(0, 0, 0, 0));

            foreach (var chunk in directionalLights)
            {
                DrawDirectionalLight(chunk);
            }

            foreach (var chunk in pointLights)
            {
                DrawPointLight(chunk);
            }

            foreach (var chunk in spotLights.Where(light => light.CastsShadows))
            {
                DrawShadedSpotLight(chunk);
            }

            foreach (var chunk in spotLights.Where(light => !light.CastsShadows))
            {
                DrawSpotLight(chunk);
            }

            GraphicsDevice.SetRenderTarget(null);
            Graphics.RenderingStatistics.IncrementLightChunksCount(lightRenderChunks.Count);
        }

        #region Draw global directional lights
                
        private void DrawDirectionalLight(LightRenderChunk chunk)
        {
            Pix.BeginEvent("DeferredRenderer::DrawDirectionalLight");

            chunk.Direction.Normalize();            
            FullScreenQuad.Draw(Graphics.GraphicsDevice, DirectionalLightShader.Bind(chunk));

            Pix.EndEvent();
        }

        #endregion

        #region Draw omnidirectional point lights
                
        private void DrawPointLight(LightRenderChunk chunk)
        {
            Pix.BeginEvent("DeferredRenderer::DrawPointLight");

            FullScreenQuad.Draw(Graphics.GraphicsDevice, PointLightShader.Bind(chunk));
                       
            Pix.EndEvent();
        }

        #endregion

        #region Draw directional spotlights

        private void DrawShadedSpotLight(LightRenderChunk chunk)
        {
            Pix.BeginEvent("DeferredRenderer::DrawShadedSpotLight");
            using (var query = ShadowOcclusionQuery.Create(Camera, chunk, chunk.SoftShadows))
            {
                Pix.BeginEvent("DeferredRenderer::DrawShadedSpotLight::ShadowMap");

                ShadowMapRenderer.RenderShadowOcclusion(query);
                var shadowOcclusion = query.ShadowOcclusion;

                Pix.EndEvent();

                Pix.BeginEvent("DeferredRenderer::DrawShadedSpotLight::Lighting");

                GraphicsDevice.SetRenderTarget(LightBuffer);
                GraphicsDevice.BlendState = BlendState.Additive;

                FullScreenQuad.Draw(GraphicsDevice, SpotLightShader.Bind(chunk, shadowOcclusion));

                Pix.EndEvent();
            }

            Pix.EndEvent();
        }

        private void DrawSpotLight(LightRenderChunk chunk)
        {
            Pix.BeginEvent("DeferredRenderer::DrawSpotLight");

            GraphicsDevice.SetRenderTarget(LightBuffer);
            GraphicsDevice.BlendState = BlendState.Additive;

            FullScreenQuad.Draw(GraphicsDevice, SpotLightShader.Bind(chunk));

            Pix.EndEvent();
        }

        #endregion

        #endregion

        #endregion

        #region Final frame composing
        
        private void ComposeFrame()
        {
            Graphics.GraphicsDevice.BlendState = BlendState.Additive;
            Graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            Graphics.GraphicsDevice.Clear(new Color(0, 0, 0, 0));

            FullScreenQuad.Draw(Graphics.GraphicsDevice, ComposeLightingShader.Bind());
        }

        #endregion

        #endregion
    }
}
