using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Physics
{
    public class JigLibXPhysicsManager : PhysicsManager
    {
        public JigLibXPhysicsManager(XGame game)
            : base(game)
        { }

        public override IPhysicsScene CreateScene()
        {
            return new JigLibXScene();
        }
    }
}
