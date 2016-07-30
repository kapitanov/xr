using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Interfaces;

namespace AISTek.XRage.Physics
{
    public abstract class PhysicsManager : XComponent, IDisposable
    {
        protected PhysicsManager(XGame game)
            : base(game)
        {
            Game.Interfaces.AddInterface(new PhysicsInterface(Game, this));
        }

        public abstract IPhysicsScene CreateScene();

        public virtual void Dispose()
        { }
    }
}
