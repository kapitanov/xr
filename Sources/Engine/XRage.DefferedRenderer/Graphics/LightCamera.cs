using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    internal abstract class LightCamera : ICamera
    {
        protected LightCamera(LightRenderChunk chunk)
        {
            FieldOfView = chunk.OuterConeAngle;
            AspectRatio = 1.0f;
            NearClippingPlane = 1.0f;
            FarClippingPlane = chunk.Radius;
            Position = chunk.Position;
            Direction = chunk.Direction;

            CreateProjection();
        }

        public Vector3 Direction { get; protected set; }

        public float FieldOfView { get; protected set; }

        public float AspectRatio { get; protected set; }

        public float NearClippingPlane { get; protected set; }

        public float FarClippingPlane { get; protected set; }

        public Vector3 Position { get; protected set; }

        public Matrix View { get; protected set; }

        public Matrix Projection { get; protected set; }

        public BoundingFrustum BoundingFrustum { get; protected set; }
        
        public void Update()
        {
            CreateProjection();
        }


        protected abstract void CreateProjection();

        public void AssignCamera(GraphicsSystem graphics)
        {
            var variables = graphics.Variables;
            variables.Set(VariableFloatId.FarClipPlane, FarClippingPlane);
            variables.Set(VariableFloat3Id.CameraPosition, Position);
            variables.Set(VariableMatrixId.World, Matrix.Identity);
            variables.Set(VariableMatrixId.View, View);
            variables.Set(VariableMatrixId.Projection, Projection);
        }
    }
}
