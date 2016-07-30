using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Physics;
using AISTek.XRage.Entities;
using AISTek.XRage.Messaging;

namespace AISTek.XRage.Interfaces
{
    public class PhysicsInterface : XInterface
    {
        internal PhysicsInterface(XGame game, PhysicsManager physicsManager)
            : base(game, InterfaceType.Physics)
        {
            Actors = new Dictionary<long, IPhysicsActor>();
            PhysicsScene = physicsManager.CreateScene();
        }

        public IPhysicsScene PhysicsScene { get; private set; }

        public IDictionary<long, IPhysicsActor> Actors { get; private set; }

        public override void Shutdown()
        {
            foreach (var actor in Actors.Values)
            {
                actor.Dispose();
            }

            Actors.Clear();

            if (PhysicsScene != null)
            {
                PhysicsScene.Dispose();
                PhysicsScene = null;
            }
        }

        public void AddActor(BaseEntity entity, IPhysicsActor actor)
        {
            Actors.Add(entity.UniqueId, actor);
        }

        public void RemoveActor(BaseEntity entity)
        {
            Actors.Remove(entity.UniqueId);
        }

        protected virtual void Game_GameMessage(IMessage message)
        {
            ExecuteMessage(message);
        }

        public override bool ExecuteMessage(IMessage message)
        {
            switch (message.Type)
            {
                //case MessageType.GetPhysicsScene:
                //    {
                //        MsgGetPhysicsScene getPhysSceneMsg = message as MsgGetPhysicsScene;
                //        message.TypeCheck(getPhysSceneMsg);

                //        getPhysSceneMsg.PhysicsScene = this.physicsScene;
                //    }
                //    return true;
                //case MessageType.AddEntityToPhysicsScene:
                //    {
                //        MsgAddEntityToPhysicsScene msgAddEntity = message as MsgAddEntityToPhysicsScene;
                //        message.TypeCheck(msgAddEntity);

                //        if (msgAddEntity.EntityID != QSGame.UniqueIDEmpty
                //            && msgAddEntity.Actor != null)
                //        {
                //            this.actors.Add(msgAddEntity.EntityID, msgAddEntity.Actor);
                //        }
                //    }
                //    return true;
                //case MessageType.RemoveEntityFromPhysicsScene:
                //    {
                //        MsgRemoveEntityFromPhysicsScene remPhysMsg = message as MsgRemoveEntityFromPhysicsScene;
                //        message.TypeCheck(remPhysMsg);

                //        if (remPhysMsg.EntityID != QSGame.UniqueIDEmpty)
                //        {
                //            IPhysicsActor actor;
                //            if (this.actors.TryGetValue(remPhysMsg.EntityID, out actor))
                //            {
                //                this.actors.Remove(remPhysMsg.EntityID);
                //                this.physicsScene.ScheduleForDeletion(actor);
                //            }
                //        }
                //    }
                //    return true;
                //case MessageType.BeginPhysicsFrame:
                //    {
                //        MsgBeginPhysicsFrame msgBeginFrame = message as MsgBeginPhysicsFrame;
                //        message.TypeCheck(msgBeginFrame);

                //        // Begin next physics frame.
                //        this.physicsScene.BeginFrame(msgBeginFrame.GameTime);
                //    }
                //    return true;
                //case MessageType.EndPhysicsFrame:
                //    {
                //        // Wait for previous physics frame to finish.
                //        this.physicsScene.EndFrame();
                //    }
                //    return true;
                //case MessageType.SetGravity:
                //    {
                //        MsgSetGravity msgSetGrav = message as MsgSetGravity;
                //        message.TypeCheck(msgSetGrav);

                //        this.physicsScene.Gravity = msgSetGrav.Gravity;
                //    }
                //    return true;
                //case MessageType.GetGravity:
                //    {
                //        MsgGetGravity msgGetGrav = message as MsgGetGravity;
                //        message.TypeCheck(msgGetGrav);

                //        msgGetGrav.Gravity = this.physicsScene.Gravity;
                //    }
                //    return true;
                //case MessageType.SetPhysicsTimeStep:
                //    {
                //        MsgSetPhysicsTimeStep msgSetPhysStep = message as MsgSetPhysicsTimeStep;
                //        message.TypeCheck(msgSetPhysStep);

                //        this.physicsScene.SetPhysicsTimeStep(msgSetPhysStep.TimeStep);
                //    }
                //    return true;
                default:
                    return false;
            }
        }
    }
}
