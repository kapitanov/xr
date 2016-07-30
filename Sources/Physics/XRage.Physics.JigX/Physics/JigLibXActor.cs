using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JigLibX.Physics;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using JigLibX.Utils;
using JigLibX.Math;

namespace AISTek.XRage.Physics
{
    internal class JigLibXActor : XComponent, IPhysicsActor
    {
        internal JigLibXActor(ActorDesc desc)
            : base(desc.Game)
        {
            shapes = desc.Shapes;
            ownerEntityID = desc.EntityId;
            
            bool applyCOM = true;

            // Construct the JigLibX body/skin for this actor.
            if (desc.IsCharacter)
            {
                jigLibXBody = new CharacterBody();
                jigLibXBody.AllowFreezing = false;
            }
            else
            {
                jigLibXBody = new Body();
            }

            if (desc.IsPhantom)
            {
                jigLibXSkin = new PhantomCollisionSkin(this.ownerEntityID, jigLibXBody);
                desc.IsDynamic = false; // Phantoms cannot move
                desc.Orientation = Matrix.CreateRotationX(MathHelper.PiOver2);
            }
            else
            {
                jigLibXSkin = new XCollisionSkin(ownerEntityID, jigLibXBody);
            }

            // Build all shapes that make up the actor.
            for (int i = 0; i < desc.Shapes.Count; ++i)
            {
                var shapeDesc = desc.Shapes[i];

                // The basic concept is the same for all shapes: create JigLibX shape of proper size and bind it to the skin.
                // TODO: Implement materials.                

                if (shapeDesc is BoxShapeDesc)
                {
                    var boxDesc = shapeDesc as BoxShapeDesc;
                    var box = new Box(Vector3.Zero, Matrix.Identity, boxDesc.Extents);
                    jigLibXSkin.AddPrimitive(box, (int)MaterialTable.MaterialID.NotBouncyRough);
                }
                else if (shapeDesc is SphereShapeDesc)
                {
                    var sphereDesc = shapeDesc as SphereShapeDesc;
                    var  sphere = new Sphere(Vector3.Zero, sphereDesc.Radius);
                    jigLibXSkin.AddPrimitive(sphere, (int)MaterialTable.MaterialID.NormalSmooth);
                }
                else if (shapeDesc is CapsuleShapeDesc)
                {
                    var capDesc = shapeDesc as CapsuleShapeDesc;
                    var cyl = new Capsule(Vector3.Zero, desc.Orientation, capDesc.Radius, capDesc.Length);
                    jigLibXSkin.AddPrimitive(cyl, (int)MaterialTable.MaterialID.NotBouncyNormal);
                }
                else if (shapeDesc is TriangleMeshShapeDesc)
                {
                    var triDesc = shapeDesc as TriangleMeshShapeDesc;
                    var triMesh = new TriangleMesh();
                    var triInds = new List<TriangleVertexIndices>();
                    var numPrimitives = triDesc.Indices.Count;

                    for (int j = 0; j < (numPrimitives - 2); j++)
                    {
                        var triIndex = new TriangleVertexIndices(triDesc.Indices[j + 2], triDesc.Indices[j + 1], triDesc.Indices[j]);

                        triInds.Add(triIndex);
                    }

                    triMesh.CreateMesh(triDesc.Vertices.ToList(), triInds.ToList(), 1024, 64);
                    jigLibXSkin.AddPrimitive(triMesh, (int)MaterialTable.MaterialID.NotBouncyRough);
                }
                else if (shapeDesc is HeightFieldShapeDesc)
                {
                    // For height fields, we need to copy the data into an Array2D.
                    var heightFieldDesc = shapeDesc as HeightFieldShapeDesc;
                    var field = new Array2D(heightFieldDesc.HeightField.GetUpperBound(0), heightFieldDesc.HeightField.GetUpperBound(1));

                    for (var x = 0; x < heightFieldDesc.HeightField.GetUpperBound(0); x++)
                    {
                        for (var z = 0; z < heightFieldDesc.HeightField.GetUpperBound(1); z++)
                        {
                            field.SetAt(x, z, heightFieldDesc.HeightField[x, z]);
                        }
                    }

                    var heightmap = new Heightmap(
                        field,
                        heightFieldDesc.SizeX / 2.0f,
                        heightFieldDesc.SizeZ / 2.0f,
                        heightFieldDesc.SizeX / heightFieldDesc.HeightField.GetLength(0),
                        heightFieldDesc.SizeZ / heightFieldDesc.HeightField.GetLength(1));
                    jigLibXSkin.AddPrimitive(heightmap, (int)MaterialTable.MaterialID.NotBouncyRough);

                    applyCOM = false;
                }
                else
                {
                    throw new Exception("Bad shape.");
                }
            }

            // Finalize the body/skin and set center-of-mass.
            var primitiveProperties = new PrimitiveProperties(PrimitiveProperties.MassDistributionEnum.Solid, PrimitiveProperties.MassTypeEnum.Density, desc.Density);

            float junk;
            Vector3 com;
            Matrix it, itCoM;

            jigLibXBody.CollisionSkin = jigLibXSkin;
            jigLibXSkin.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);
            jigLibXBody.BodyInertia = itCoM;
            jigLibXBody.Mass = junk;

            jigLibXBody.MoveTo(desc.Position + com, desc.Orientation);

            if (applyCOM)
            {
                jigLibXSkin.ApplyLocalTransform(new Transform(-com, Matrix.Identity));
            }

            if (desc.IsCharacter)
            {
                jigLibXBody.SetBodyInvInertia(0.0f, 0.0f, 0.0f);
            }

            // Let JigLibX know this body should be a part of the simulation.
            jigLibXBody.EnableBody();

            jigLibXBody.Immovable = !desc.IsDynamic;
        }

        public void Dispose()
        {
        }

        public float Density
        {
            get
            {
                // Doesn't seem to be supported by JigLibX.
                // @todo: Write GetDensity methods for every shape type.
                throw new Exception("Not implemented");
            }
            set
            {
                // Reset the body's inertia and mass with the new density value.
                var primitiveProperties = new PrimitiveProperties(PrimitiveProperties.MassDistributionEnum.Solid, PrimitiveProperties.MassTypeEnum.Density, value);

                float junk;
                Vector3 com;
                Matrix it, itCoM;

                jigLibXSkin.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);
                jigLibXBody.BodyInertia = itCoM;
                jigLibXBody.Mass = junk;
            }
        }

        public float Mass
        {
            get {return jigLibXBody.Mass;}
            set
            {
                // Reset the body's inertia and mass with the new mass value.
                var primitiveProperties = new PrimitiveProperties(PrimitiveProperties.MassDistributionEnum.Solid, PrimitiveProperties.MassTypeEnum.Mass, value);

                float junk;
                Vector3 com;
                Matrix it, itCoM;

                jigLibXSkin.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);
                jigLibXBody.BodyInertia = itCoM;
                jigLibXBody.Mass = junk;
            }
        }

        public Vector3 Position
        {
            get { return jigLibXBody.Position; }
            set
            {
                jigLibXBody.EnableBody();
                jigLibXBody.Position = value;
            }
        }

        public Matrix Orientation
        {
            get { return jigLibXBody.Orientation;}
            set
            {
                jigLibXBody.EnableBody();
                jigLibXBody.Orientation = value;
            }
        }

        public Vector3 LinearVelocity
        {
            get
            {
                if (jigLibXBody is CharacterBody)
                {
                    return (jigLibXBody as CharacterBody).DesiredVelocity;
                }
                else
                {
                    return jigLibXBody.Velocity;
                }
            }
            set
            {
                if (jigLibXBody is CharacterBody)
                {
                    (jigLibXBody as CharacterBody).DesiredVelocity = value;
                }
                else
                {
                    jigLibXBody.Velocity = value;
                    jigLibXBody.VelocityAux = value;
                }
            }
        }

        public void AddForceFromOutsideSimulation(Vector3 force)
        {
            jigLibXBody.ApplyWorldImpulse(force);
        }

        public Vector3 AngularVelocity
        {
            get { return jigLibXBody.AngularVelocity; }
            set { jigLibXBody.AngularVelocity = value; }
        }

        public IList<IShapeDesc> Shapes
        {
            get { return shapes; }
        }
        
        private long OwnerEntityId
        {
            get { return this.ownerEntityID; }
        }

        public void EnableCollisionListening()
        {
            // If listener isn't already enabled
            if (listener == null)
            {
                currentCollisions = new Dictionary<long, EntityCollisionInfo>();
                lastFramesCollisions = new Dictionary<long, EntityCollisionInfo>();

                listener = new CollisionCallbackFn(CollisionHandler);
                jigLibXSkin.callbackFn += new CollisionCallbackFn(CollisionHandler);
            }
        }

        public void DisableCollisionListening()
        {
            // If listener is enabled
            if (listener != null)
            {
                jigLibXSkin.callbackFn -= listener;
                listener = null;

                CompareCollisions();

                currentCollisions.Clear();
                currentCollisions = null;

                lastFramesCollisions.Clear();
                lastFramesCollisions = null;
            }
        }

        public bool CollisionHandler(CollisionSkin self, CollisionSkin other)
        {
            //CollisionData data;

            //bool SelfIsPhantom = (self is PhantomCollisionSkin);
            //bool OtherIsPhantom = (other is PhantomCollisionSkin);

            //// Fill in data for the two colliding entities
            //if (SelfIsPhantom)
            //{
            //    var phantom = self as PhantomCollisionSkin;
            //    data.ListeningEntity.IsPhantom = true;
            //    data.ListeningEntity.EntityID = phantom.EntityId;
            //}
            //else // If we ever have more than two skin types, this will need to be an 'else if', but for now we can assume it's a QSCollisionSkin
            //{
            //   XCollisionSkin qsSkin = self as XCollisionSkin;
            //    data.ListeningEntity.IsPhantom = false;
            //    data.ListeningEntity.EntityID = qsSkin.EntityId;
            //}

            //if (OtherIsPhantom)
            //{
            //    PhantomCollisionSkin phantom = other as PhantomCollisionSkin;
            //    data.OtherEntity.IsPhantom = true;
            //    data.OtherEntity.EntityID = phantom.EntityId;
            //}
            //else // If we ever have more than two skin types, this will need to be an 'else if', but for now we can assume it's a QSCollisionSkin
            //{
            //    XCollisionSkin qsSkin = other as XCollisionSkin;
            //    data.OtherEntity.IsPhantom = false;
            //    data.OtherEntity.EntityID = qsSkin.EntityId;
            //}

            //// A new collision has occured
            //EntityCollisionInfo info;
            //if (!currentCollisions.TryGetValue(data.OtherEntity.EntityId, out info))
            //{
            //    currentCollisions.Add(data.OtherEntity.EntityId, data.OtherEntity);
            //}

            //// If either skin is a phantom, then no collision happens
            //if (SelfIsPhantom || OtherIsPhantom)
            //{
            //    return false;
            //}

            // Let collision be handled by physics engine
            return true;
        }

        public void UpdateCollisions()
        {
            if (listener == null)
                return;

            if (currentCollisions.Count == 0 && lastFramesCollisions.Count == 0)
                return;

            CompareCollisions();

            // Clear out last frames collision info, and copy current frames info
            lastFramesCollisions.Clear();
            foreach (EntityCollisionInfo collInfo in currentCollisions.Values)
            {
                lastFramesCollisions.Add(collInfo.EntityId, collInfo);
            }

            currentCollisions.Clear();
        }

        public void RemoveFromSimulation()
        {
            jigLibXBody.DisableBody();
        }

        public void SetMovable(bool movable)
        {
            jigLibXBody.Immovable = !movable;
        }

        public BoundingBox GetBoundingBox()
        {
            return jigLibXSkin.WorldBoundingBox;
        }

        public void CharacterDoJump()
        {
            if (jigLibXBody is CharacterBody)
            {
                (jigLibXBody as CharacterBody).DoJump();
            }
        }

        private void CompareCollisions()
        {
            //// Compare current collisions with last frames collisions
            //var currentValues = currentCollisions.Values;

            //EntityCollisionInfo info;
            //foreach (EntityCollisionInfo entityInfo in currentValues)
            //{
            //    // If there is a collision that wasn't there last frame, send out a message
            //    if (!lastFramesCollisions.TryGetValue(entityInfo.EntityID, out info))
            //    {
            //        //MsgOnCollision msgCollData = ObjectPool.Aquire<MsgOnCollision>();
            //        //msgCollData.EntityID = entityInfo.EntityID;
            //        //msgCollData.IsPhantom = entityInfo.IsPhantom;
            //        //msgCollData.UniqueTarget = this.OwnerEntityID;
            //        //this.game.SendMessage(msgCollData);
            //    }
            //}

            //Dictionary<Int64, EntityCollisionInfo>.ValueCollection lastFrameValues = lastFramesCollisions.Values;
            //foreach (EntityCollisionInfo entityInfo in lastFrameValues)
            //{
            //    // If there is no longer a collision that was there last frame, send out a message.
            //    if (!currentCollisions.TryGetValue(entityInfo.EntityID, out info))
            //    {
            //        MsgOffCollision msgCollData = ObjectPool.Aquire<MsgOffCollision>();
            //        msgCollData.EntityID = entityInfo.EntityID;
            //        msgCollData.IsPhantom = entityInfo.IsPhantom;
            //        msgCollData.UniqueTarget = this.OwnerEntityID;
            //        this.game.SendMessage(msgCollData);
            //    }
            //}
        }

        private Body jigLibXBody;
        private CollisionSkin jigLibXSkin;
        private IList<IShapeDesc> shapes;

        private long ownerEntityID;

        private Dictionary<long, EntityCollisionInfo> currentCollisions;
        private Dictionary<long, EntityCollisionInfo> lastFramesCollisions;
        private CollisionCallbackFn listener;
    }

}
