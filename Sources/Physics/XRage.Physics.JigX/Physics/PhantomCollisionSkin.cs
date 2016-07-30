using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JigLibX.Physics;

namespace AISTek.XRage.Physics
{
    internal class PhantomCollisionSkin :XCollisionSkin
    {
        public PhantomCollisionSkin(long entityId)
            : base(entityId)
        { }

        public PhantomCollisionSkin(long entityId, Body owner)
            : base(entityId, owner)
        { }
    }
}
