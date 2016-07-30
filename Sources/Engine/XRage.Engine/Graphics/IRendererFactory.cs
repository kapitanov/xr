using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Configuration;

namespace AISTek.XRage.Graphics
{
    public interface IRendererFactory
    {
        Renderer CreateRenderer(XGame game, GraphicsSettings graphicsSettings);
    }
}
