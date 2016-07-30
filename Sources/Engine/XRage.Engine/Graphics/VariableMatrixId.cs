using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Enumeration of all queriable variable matrix types.
    /// </summary>
    public enum VariableMatrixId
    {
        World = 0,
        View,
        LightView,
        LightProjection,
        Projection,
        WorldView,
        ReflectionView,
        ViewProjection,
        WorldViewProjection,
        LightViewProjection,
        WindDirection,

        Identity,
    }
}
