using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Enumeration of all queriable variable texture2D types.
    /// </summary>
    public enum VariableTexture2d
    {
        ShadowMap = 0,
        ReflectionMap,
        RefractionMap,
        CubeMap,
        HeightMap,
        NormalMap,

        Unity,
    }
}
