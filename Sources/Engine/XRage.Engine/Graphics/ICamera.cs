using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public interface ICamera
    {
        float FieldOfView { get; }

        float AspectRatio { get; }

        float NearClippingPlane{get;}

        float FarClippingPlane { get; }

        Vector3 Position { get; }

        Matrix View { get; }

        Matrix Projection { get; }

        BoundingFrustum BoundingFrustum { get; }

        void AssignCamera(GraphicsSystem graphics);
    }
}
