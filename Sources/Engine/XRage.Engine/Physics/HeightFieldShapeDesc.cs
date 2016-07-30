using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Physics
{
    public class HeightFieldShapeDesc : IShapeDesc
    {
        public float[,] HeightField { get; set; }

        public float SizeX { get; set; }

        public float SizeZ { get; set; }
    }
}
