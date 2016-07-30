using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    internal class ParticleRenderer : Renderer
    {
        public ParticleRenderer (XGame game)
            :base(game)
        {

        }

        internal RenderTarget2D DiffuseBuffer { get; private set; }

        internal RenderTarget2D NormalBuffer { get; private set; }

        internal RenderTarget2D DepthBuffer { get; private set; }

        internal RenderTarget2D LightBuffer { get; private set; }

        public override void Initialize()
        {
            CreateRenderTargets();
        }

        private void CreateRenderTargets()
        {
            DiffuseBuffer = new RenderTarget2D(
                Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.Viewport.Width,
                Graphics.GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24);

            NormalBuffer = new RenderTarget2D(
                Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.Viewport.Width,
                Graphics.GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);

            DepthBuffer = new RenderTarget2D(
                Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.Viewport.Width,
                Graphics.GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Single,
                DepthFormat.None);

            LightBuffer = new RenderTarget2D(
                Graphics.GraphicsDevice,
                Graphics.GraphicsDevice.Viewport.Width,
                Graphics.GraphicsDevice.Viewport.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
        }

        public override void DrawFrame(ICamera camera, GameTime gameTime)
        {
        
        }


    }
}
