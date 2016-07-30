using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Used for level-of-detail values throughout the <see cref="Terrain"/> and 
    /// <see cref="TerrainPatch"/> system.
    /// </summary>
    public enum LevelOfDetail
    {
        NumOfLods = 9,
        Minimum = 8,
        Low = 4,
        Med = 2,
        High = 1
    }
}
