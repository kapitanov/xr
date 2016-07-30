using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Entities
{
    public abstract class BasePerspectiveCamera : BaseCamera
    {
        protected BasePerspectiveCamera(XGame game, string name, BaseEntity parent)
            : base(game, name, parent)
        { }

        protected override void ComputeCameraView()
        {
            View = Matrix.CreateLookAt(Position, Target, UpVector);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(FieldOfView),
                AspectRatio,
                NearClippingPlane,
                FarClippingPlane);

            BoundingFrustum = new BoundingFrustum(View * Projection);
        }
    }
}
