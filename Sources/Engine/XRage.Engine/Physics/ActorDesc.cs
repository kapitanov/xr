using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Physics
{
    public class ActorDesc : XComponent
    {
        public ActorDesc(XGame game)
            : base(game)
        {
            Shapes = new List<IShapeDesc>();
            Position = Vector3.Zero;
            Orientation = Matrix.Identity;
            Density = 1.0f;
        }

        public Vector3 Position { get; set; }

        public Matrix Orientation { get; set; }

        public float Density { get; set; }

        public bool IsDynamic { get; set; }

        public bool IsPhantom { get; set; }

        public bool IsCharacter { get; set; }

        public long EntityId { get; set; }

        public IList<IShapeDesc> Shapes { get; private set; }
    }

}
