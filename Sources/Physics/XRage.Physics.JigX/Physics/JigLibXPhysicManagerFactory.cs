using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Physics
{
    public class JigLibXPhysicManagerFactory : IPhysicManagerFactory
    {
        public PhysicsManager CreatePhysicsManager(XGame game)
        {
            return new JigLibXPhysicsManager(game);
        }
    }
}
