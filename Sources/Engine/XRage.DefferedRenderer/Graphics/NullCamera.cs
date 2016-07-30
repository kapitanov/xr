using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    internal sealed class NullCamera : ICamera
    {
        public float FieldOfView { get { return 0.0f; } }

        public float AspectRatio { get { return 0.0f; } }

        public float NearClippingPlane { get { return 0.0f; } }

        public float FarClippingPlane { get { return 0.0f; } }

        public Vector3 Position { get { return Vector3.Zero; } }
        
        public Matrix View { get { return Matrix.Identity; } }
        
        public Matrix Projection { get { return Matrix.Identity; } }
        
        public BoundingFrustum BoundingFrustum { get { return new BoundingFrustum(Matrix.Identity); } }
        

        public void AssignCamera(GraphicsSystem graphics)
        { }
    }
}
