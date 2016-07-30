using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using AISTek.XRage.Configuration;

namespace AISTek.XRage.Graphics
{
    public class DeferredRendererFactory : IRendererFactory
    {
        public Renderer CreateRenderer(XGame game)
        {
            return new DeferredRenderer(game);
        }

        public Renderer CreateRenderer(XGame game, GraphicsSettings graphicsSettings)
        {
            if (graphicsSettings.RendererSettings.GetProperty("type") != "deferred")
                throw new InvalidOperationException("Renderer type is not supported.");
            var renderer = new DeferredRenderer(game);
            return renderer;
        }
    }
}
