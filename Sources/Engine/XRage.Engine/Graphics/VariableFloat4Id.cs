using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Enumeration of all queriable variable float4 types.
    /// </summary>
    public enum VariableFloat4Id
    {
        ViewPos = 0,
        ViewForward,
        LightDir,
        ModelColor,
        AmbientColor,
        DiffuseColor,
        SpecularColor,
        MinColor,
        MaxColor,
        FogColor,
        WaterColor,

        Unity,
    }
}
