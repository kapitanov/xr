using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public class LightRenderChunk : IRenderChunk
    {
        public Vector3 Position { get; set; }

        public Vector3 Direction { get; set; }

        public LightType Type { get; set; }

        public Vector3 DiffuseColor { get; set; }

        public Vector3 SpecularColor { get; set; }

        public float Intensity { get; set; }

        public float InnerConeAngle { get; set; }

        public float OuterConeAngle { get; set; }

        public FalloffType Falloff { get; set; }

        public float Radius { get; set; }

        public bool CastsShadows { get; set; }

        public bool SoftShadows { get; set; }
        
        public void Recycle()
        { }
    }
}
