using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.SceneManagement
{
    internal enum Stage
    {
        LoadingBrushes = 1,
        LoadingTerrain,
        LoadingEntities,
        InitializingEntities,

        StagesCount
    }
}
