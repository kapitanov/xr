using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JigLibX.Physics;
using JigLibX.Collision;

namespace AISTek.XRage.Physics
{
    internal class XCollisionSkin : CollisionSkin
    {
        public XCollisionSkin(long entityId, Body owner)
            : base(owner)
        {
            EntityId = entityId;
        }

        public XCollisionSkin(long entityId)
            : base()
        {
            EntityId = entityId;
        }

        public long EntityId { get; set; }
    }
}
