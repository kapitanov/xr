using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    internal class SpotLightCamera : LightCamera
    {
        public SpotLightCamera(LightRenderChunk chunk)
            : base(chunk)
        { }
              
        protected override void CreateProjection()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(MathHelper.Clamp(1.75f * FieldOfView, 0, 179.99f)),
                AspectRatio,
                NearClippingPlane,
                FarClippingPlane);

            if (Vector3.Cross(Direction, DefaultUpVector).Length() > 0)
            {
                View = Matrix.CreateLookAt(Position, Position + Direction, DefaultUpVector);
            }
            else
            {
                View = Matrix.CreateLookAt(Position, Position + Direction, AltUpVector);
            }

            BoundingFrustum = new BoundingFrustum(View * Projection);
        }

        private const float UpVectorOffset = 0.0001f;
        private readonly Vector3 DefaultUpVector = Vector3.Up;
        private readonly Vector3 AltUpVector = Vector3.Up + UpVectorOffset * Vector3.One;
    }
}
