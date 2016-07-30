using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    public sealed class GBuffer
    {
        public GBuffer(Renderer renderer)
        {
            this.renderer = renderer;
            GraphicsDevice = renderer.GraphicsDevice;
        }

        public RenderTarget2D DiffuseBuffer { get; private set; }

        public RenderTarget2D NormalBuffer { get; private set; }

        public RenderTarget2D DepthBuffer { get; private set; }

        public GBufferFormat Format { get; private set; }

        public void Initialize(GBufferFormat format)
        {
            Format = format;

            DiffuseBuffer = new RenderTarget2D(
               GraphicsDevice,
               GraphicsDevice.Viewport.Width,
               GraphicsDevice.Viewport.Height,
               false,
               format.DiffuseFormat,
               format.DepthPrecision);

            NormalBuffer = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                false,
                format.NormalFormat,
                DepthFormat.None);

            DepthBuffer = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                false,
                format.DepthFormat,
                DepthFormat.None,
                0,
                RenderTargetUsage.PlatformContents);

            bindings = new[]
            {
                new RenderTargetBinding(DiffuseBuffer),
               new RenderTargetBinding(NormalBuffer),
               new RenderTargetBinding(DepthBuffer)
            };
        }

        public void BindRenderTargets()
        {
            GraphicsDevice.SetRenderTargets(bindings);
        }

        private GraphicsDevice GraphicsDevice { get; set; }

        private Renderer renderer;
        private RenderTargetBinding[] bindings;
    }
}
