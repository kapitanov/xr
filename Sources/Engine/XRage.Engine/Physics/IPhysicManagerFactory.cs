using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Physics
{
    public interface IPhysicManagerFactory
    {
        PhysicsManager CreatePhysicsManager(XGame game);
    }
}
