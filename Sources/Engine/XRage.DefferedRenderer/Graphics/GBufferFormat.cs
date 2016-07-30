using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    public class GBufferFormat
    {
        public SurfaceFormat DiffuseFormat { get; set; }

        public SurfaceFormat NormalFormat { get; set; }

        public SurfaceFormat DepthFormat { get; set; }

        // Precision 
        public DepthFormat DepthPrecision { get; set; }

        public static GBufferFormat Format32Bit = new GBufferFormat 
        { 
            DepthFormat = SurfaceFormat.Single,
            DepthPrecision = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth24, 
            DiffuseFormat = SurfaceFormat.Color,
            NormalFormat = SurfaceFormat.Color 
        };
    }
}
