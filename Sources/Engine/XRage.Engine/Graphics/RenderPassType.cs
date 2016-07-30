using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    public enum RenderPassType
    {
        SolidGeometry,
        SemiTransparentGeometry,
        ShadowCasters,
        ShadowTargets,
        Lighting
        //Normal = 0,
        //OpaqueOnly,
        //SemiTransparentOnly,
        //WaterReflection,
        //WaterRefraction,
        //WaterRefractionSkyOnly,
        //ShadowMapCreate,
    }
}
