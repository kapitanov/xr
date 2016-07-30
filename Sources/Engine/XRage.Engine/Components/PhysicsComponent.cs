using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Physics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework.Graphics;
using AISTek.XRage.Messaging;

namespace AISTek.XRage.Components
{
    public class PhysicsComponent : BaseComponent
    {
        public PhysicsComponent(BaseEntity parent, ShapeType type, float density, bool isDynamic)
            : base(parent)
        {
            ActivateComponent();

            ShapeType = type;
            Density = density;
            IsDynamic = isDynamic;

            InitializeActor();
        }

        public PhysicsComponent(BaseEntity parent, string physicsModelPath, float density, bool isDynamic)
            : base(parent)
        {
            ShapeType = ShapeType.TriangleMesh;
            Density = density;
            IsDynamic = isDynamic;

            PhysMesh = Game.Content.Load<Model>(physicsModelPath);

            InitializeActor();
        }

        public PhysicsComponent(BaseEntity parent, float[,] heightData, int scaleFactor)
            : base(parent)
        {
            //this.shapeType = ShapeType.Heightfield;

            //MsgGetPhysicsScene getPhysSceneMsg = ObjectPool.Aquire<MsgGetPhysicsScene>();
            //getPhysSceneMsg.UniqueTarget = this.parentEntity.UniqueID;
            //this.parentEntity.Game.SendInterfaceMessage(getPhysSceneMsg, InterfaceType.Physics);

            //var physScene = getPhysSceneMsg.PhysicsScene;
            //if (physScene != null)
            //{
            //    CreateHeightfieldActor(physScene, heightData, scaleFactor);

            //    if (this.actor != null)
            //    {
            //        MsgAddEntityToPhysicsScene addToSceneMsg = ObjectPool.Aquire<MsgAddEntityToPhysicsScene>();
            //        addToSceneMsg.Actor = this.actor;
            //        addToSceneMsg.EntityID = this.parentEntity.UniqueID;
            //        this.parentEntity.Game.SendInterfaceMessage(addToSceneMsg, InterfaceType.Physics);
            //    }
            //}
        }


        public IPhysicsActor Actor { get; private set; }

        public ShapeType ShapeType { get; private set; }

        public bool IsDynamic { get; private set; }

        public float Density { get; private set; }

        //@TODO: StaticModel has a lot of extra info made for rendering, we need PhysicsModel which would
        //       require less memory and load faster.
        public Model PhysMesh { get; private set; }

        public void SetPosition(Vector3 position)
        {
            Actor.Position = position;
        }

        public void SetRotation(Matrix rotation)
        {
            Actor.Orientation = rotation;
        }

        public override void Shutdown()
        {
            base.Shutdown();

            //// We need to tell the physics interface to remove this entity's physics from the world
            //MsgRemoveEntityFromPhysicsScene remFromSceneMsg = ObjectPool.Aquire<MsgRemoveEntityFromPhysicsScene>();
            //remFromSceneMsg.EntityID = this.parentEntity.UniqueID;
            //this.parentEntity.Game.SendInterfaceMessage(remFromSceneMsg, InterfaceType.Physics);
        }


        protected virtual void InitializeActor()
        {
            //MsgGetPhysicsScene getPhysSceneMsg = ObjectPool.Aquire<MsgGetPhysicsScene>();
            //getPhysSceneMsg.UniqueTarget = this.parentEntity.UniqueID;
            //this.parentEntity.Game.SendInterfaceMessage(getPhysSceneMsg, InterfaceType.Physics);

            //IPhysicsScene physScene = getPhysSceneMsg.PhysicsScene;
            //if (physScene != null)
            //{
            //    CreateActor(physScene);

            //    if (this.actor != null)
            //    {
            //        MsgAddEntityToPhysicsScene addToSceneMsg = ObjectPool.Aquire<MsgAddEntityToPhysicsScene>();
            //        addToSceneMsg.Actor = this.actor;
            //        addToSceneMsg.EntityID = this.parentEntity.UniqueID;
            //        this.parentEntity.Game.SendInterfaceMessage(addToSceneMsg, InterfaceType.Physics);
            //    }
            //}
        }

        public override void Update(GameTime gameTime)
        {
            if (Actor != null)
            {
  //              ParentEntity.UpdateFromPhysics(Actor.Position, Actor.Orientation);
            }

          //  Actor.UpdateCollisions();
        }

        protected virtual void CreateActor(IPhysicsScene physicsScene)
        {
            var newShape = CreateShapeFromType(ShapeType);

            if (newShape == null)
            {
                throw new Exception("Shape did not load properly");
            }

            var desc = new ActorDesc(Game)
            {
                Orientation = ParentEntity.Rotation,
                Density = Density,
                IsDynamic = IsDynamic,
                Position = ParentEntity.Position,
                EntityId = ParentEntity.UniqueId,
                IsPhantom = false,
                IsCharacter = false
            };

            desc.Shapes.Add(newShape);

            Actor = physicsScene.CreateActor(desc);
        }

        /// <summary>
        /// Creates an actor using shape info and information from the parent BaseEntity.
        /// </summary>
        /// <param name="PhysicsScene">Reference to the physics scene</param>
        /// <param name="density">Density of physics object</param>
        private void CreateHeightfieldActor(IPhysicsScene physicsScene, float[,] heightData, float scaleFactor)
        {
            var heightfieldShape = CreateHeightfieldShape(heightData, scaleFactor);

            if (heightfieldShape == null)
            {
                throw new Exception("Shape did not load properly");
            }

            var desc = new ActorDesc(Game)
            {
                Orientation = ParentEntity.Rotation,
                Density = 0.0f,
                IsDynamic = false,
                Position = ParentEntity.Position,
                EntityId = ParentEntity.UniqueId
            };

            desc.Shapes.Add(heightfieldShape);

            Actor = physicsScene.CreateActor(desc);
        }

        protected IShapeDesc CreateShapeFromType(ShapeType type)
        {
            var scale = ParentEntity.Scale;

            switch (type)
            {
                case ShapeType.Box:
                    return new BoxShapeDesc { Extents = new Vector3(scale, scale, scale) };

                case ShapeType.Sphere:
                    return new SphereShapeDesc { Radius = scale };

                case ShapeType.Heightfield:
                    // Unsupported by this method, use CreateHeightfieldShape()
                    return null;

                case ShapeType.Capsule:
                    return new CapsuleShapeDesc { Radius = scale, Length = scale };

                case ShapeType.TriangleMesh:

                    if (IsDynamic)
                    {
                        throw new Exception("Triangle Mesh shapes do not support dynamic physics");
                    }

                    var shape = new TriangleMeshShapeDesc();

                    //if (physMesh == null)
                    //{
                    //    MsgGetModelVertices msgGetVerts = ObjectPool.Aquire<MsgGetModelVertices>();
                    //    msgGetVerts.UniqueTarget = this.parentEntity.UniqueID;
                    //    this.parentEntity.Game.SendMessage(msgGetVerts);

                    //    shape.Vertices = msgGetVerts.Vertices;

                    //    MsgGetModelIndices msgGetInds = ObjectPool.Aquire<MsgGetModelIndices>();
                    //    msgGetInds.UniqueTarget = this.parentEntity.UniqueID;
                    //    this.parentEntity.Game.SendMessage(msgGetInds);

                    //    shape.Indices = msgGetInds.Indices;
                    //}
                    //else
                    //{
                    //    shape.Vertices = physMesh.GetModelVertices(this.parentEntity.Scale);
                    //    shape.Indices = physMesh.GetModelIndices();
                    //}

                    if ((shape.Vertices.Count == 0) ||
                        (shape.Indices.Count == 0))
                        return null;

                    return shape;

                default:
                    // Throw exception
                    return null;
            };
        }

        private HeightFieldShapeDesc CreateHeightfieldShape(float[,] heightData, float scaleFactor)
        {
            var heightFieldDesc = new HeightFieldShapeDesc();
            heightFieldDesc.HeightField = heightData;
            heightFieldDesc.SizeX = heightData.GetLength(0) * scaleFactor;
            heightFieldDesc.SizeZ = heightData.GetLength(1) * scaleFactor;

            return heightFieldDesc;
        }

        public override bool ExecuteMessage(IMessage message)
        {
            if (message.UniqueTarget != ParentEntity.UniqueId)
                return false;

            switch (message.Type)
            {
                //case MessageType.SetPosition:
                //    {
                //        MsgSetPosition msgSetPos = message as MsgSetPosition;
                //        message.TypeCheck(msgSetPos);

                //        SetPosition(msgSetPos.position);
                //    }
                //    return true;
                //case MessageType.ModifyPosition:
                //    {
                //        MsgModifyPosition msgModPos = message as MsgModifyPosition;
                //        message.TypeCheck(msgModPos);

                //        SetPosition(this.actor.position + msgModPos.position);
                //    }
                //    return true;
                //case MessageType.SetRotation:
                //    {
                //        MsgSetRotation setRotMsg = message as MsgSetRotation;
                //        message.TypeCheck(setRotMsg);

                //        SetRotation(setRotMsg.rotation);
                //    }
                //    return true;
                //case MessageType.ModifyRotation:
                //    {
                //        MsgModifyRotation modRotMsg = message as MsgModifyRotation;
                //        message.TypeCheck(modRotMsg);

                //        SetRotation(this.actor.Orientation *= modRotMsg.rotation);
                //    }
                //    return true;
                //case MessageType.SetLinearVelocity:
                //    {
                //        MsgSetLinearVelocity setLinVelMsg = message as MsgSetLinearVelocity;
                //        message.TypeCheck(setLinVelMsg);

                //        this.actor.LinearVelocity = setLinVelMsg.LinearVelocity;
                //    }
                //    return true;
                //case MessageType.GetLinearVelocity:
                //    {
                //        MsgGetLinearVelocity getLinVelMsg = message as MsgGetLinearVelocity;
                //        message.TypeCheck(getLinVelMsg);

                //        getLinVelMsg.LinearVelocity = this.actor.LinearVelocity;
                //    }
                //    return true;
                //case MessageType.SetAngularVelocity:
                //    {
                //        MsgSetAngularVelocity setAngVelMsg = message as MsgSetAngularVelocity;
                //        message.TypeCheck(setAngVelMsg);

                //        this.actor.AngularVelocity = setAngVelMsg.AngularVelocity;
                //    }
                //    return true;
                //case MessageType.GetAngularVelocity:
                //    {
                //        MsgGetAngularVelocity getAngVelMsg = message as MsgGetAngularVelocity;
                //        message.TypeCheck(getAngVelMsg);

                //        getAngVelMsg.AngularVelocity = this.actor.AngularVelocity;
                //    }
                //    return true;
                //case MessageType.AddLinearForce:
                //    {
                //        MsgAddLinearForce addForceMsg = message as MsgAddLinearForce;
                //        message.TypeCheck(addForceMsg);

                //        if (addForceMsg.LinearVelocity.LengthSquared() > 0.0f)
                //        {
                //            this.actor.LinearVelocity += addForceMsg.LinearVelocity;
                //        }
                //    }
                //    return true;
                //case MessageType.AddAngularForce:
                //    {
                //        MsgAddAngularForce addForceMsg = message as MsgAddAngularForce;
                //        message.TypeCheck(addForceMsg);

                //        this.actor.AngularVelocity += addForceMsg.AngularVelocity;
                //    }
                //    return true;
                //case MessageType.ListenForCollision:
                //    {
                //        MsgListenForCollision msgListenColl = message as MsgListenForCollision;
                //        message.TypeCheck(msgListenColl);

                //        if (msgListenColl.ListenForCollisions == true)
                //        {
                //            this.actor.EnableCollisionListening();
                //        }
                //        else
                //        {
                //            this.actor.DisableCollisionListening();
                //        }
                //    }
                //    return true;
                //case MessageType.GetHasDynamicPhysics:
                //    {
                //        MsgGetHasDynamicPhysics msgGetDynPhys = message as MsgGetHasDynamicPhysics;
                //        message.TypeCheck(msgGetDynPhys);

                //        msgGetDynPhys.HasDynamicPhysics = this.isDynamic;
                //    }
                //    return true;
                //case MessageType.SetPhysicsMovableState:
                //    {
                //        MsgSetPhysicsMovableState msgSetDynPhys = message as MsgSetPhysicsMovableState;
                //        message.TypeCheck(msgSetDynPhys);

                //        if (this.actor == null)
                //            return false;

                //        this.actor.SetMovable(msgSetDynPhys.Movable);

                //        this.isDynamic = msgSetDynPhys.Movable;
                //    }
                //    return true;
                default:
                    return false;
            }
        }
    }

}
