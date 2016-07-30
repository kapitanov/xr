using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Sample
{
    public class SampleRendererFactory : IRendererFactory
    {
        public Renderer CreateRenderer(XGame game, Configuration.GraphicsSettings graphicsSettings)
        {
            return new SampleRenderer(game);
        }
    }
}
