using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Physics
{
    public interface IPhysicsScene : IDisposable
    {
        Vector3 Gravity { get; set; }

        IPhysicsActor CreateActor(ActorDesc desc);

        //void SetPhysicsTimeStep(int stepsPerSecond);

        //void ScheduleForDeletion(IPhysicsActor actor);

        void BeginFrame(GameTime gameTime);

        void EndFrame();

        //void KillProcessingThread();
    }

}
