using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using JigLibX.Physics;

namespace AISTek.XRage.Physics
{
    internal class CharacterBody : Body
    {
        public CharacterBody()
            : base()
        { }

        public Vector3 DesiredVelocity { get; set; }
                
        public void DoJump()
        {
            doJump = true;
        }

        public override void AddExternalForces(float dt)
        {
            ClearForces();

            if (doJump)
            {
                foreach (var info in CollisionSkin.Collisions)
                {
                    var N = info.DirToBody0;
                    if (info.SkinInfo.Skin1.Owner == this )
                        Vector3.Negate(ref N, out N);

                    if (Vector3.Dot(N, Orientation.Up) > 0.7f)
                    {
                        var vel = Velocity;
                        vel.Y = 50.0f;
                        Velocity = vel;
                        break;
                    }
                }

                doJump = false;
            }

            // Very important
            AngularVelocity = Vector3.Zero;

            Velocity += DesiredVelocity;
            DesiredVelocity = Vector3.Zero;

            var velocityNoY = Velocity;
            velocityNoY.Y = 0;

            var forceFactor = 2000.0f;
            AddWorldForce((-velocityNoY * (1 - (5 * dt))) * Mass * dt * forceFactor);

            AddGravityToExternalForce();
        }

        private bool doJump = false;
    }
}
