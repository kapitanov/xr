using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Physics
{
    public class TriangleMeshShapeDesc : IShapeDesc
    {
        public IList<Vector3> Vertices { get; set; }

        public Vector3[] Normals { get; set; }

        public IList<int> Indices { get; set; }
    }
}
