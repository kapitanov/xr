using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Physics
{
    public interface IPhysicsActor : IDisposable
    {
        float Density { get; set; }

        float Mass { get; set; }

        Vector3 Position { get; set; }

        Matrix Orientation { get; set; }

        //Vector3 LinearVelocity { get; set; }

        //Vector3 AngularVelocity { get; set; }

        IList<IShapeDesc> Shapes { get; }

        //void EnableCollisionListening();

        //void DisableCollisionListening();

        //void AddForceFromOutsideSimulation(Vector3 force);

        //void UpdateCollisions();

        //void SetMovable(bool movable);

        //BoundingBox GetBoundingBox();

        //void CharacterDoJump();
    }

}
