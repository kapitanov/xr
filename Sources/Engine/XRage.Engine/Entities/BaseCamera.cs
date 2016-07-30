using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Entities
{
    public abstract class BaseCamera : BaseEntity, ICamera
    {
        protected BaseCamera(XGame game, string name, BaseEntity parent)
            : base(game, name, parent)
        {
            Position = Vector3.Zero;
            Target = -Vector3.UnitZ;
            UpVector = Vector3.UnitY;
            AspectRatio = Game.Graphics.GraphicsDevice.Viewport.AspectRatio;
            NearClippingPlane = 1.0f;
            FarClippingPlane = 100.0f;
            FieldOfView = 45.0f;
        }

        #region Public properties

        public override Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                Invalidate();
            }
        }

        public Vector3 Target
        {
            get { return target; }
            set
            {
                target = value;
                Invalidate();
            }
        }

        public Vector3 UpVector
        {
            get { return upVector; }
            set
            {
                upVector = value;
                Invalidate();
            }
        }

        public override Matrix Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                Invalidate();
            }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                Invalidate();
            }
        }

        public float NearClippingPlane
        {
            get { return nearClippingPlane; }
            set
            {
                nearClippingPlane = value;
                Invalidate();
            }
        }

        public float FarClippingPlane
        {
            get { return farClippingPlane; }
            set
            {
                farClippingPlane = value;
                Invalidate();
            }
        }

        public float FieldOfView
        {
            get { return fieldOdView; }
            set
            {
                fieldOdView = value;
                Invalidate();
            }
        }

        public Matrix View { get; protected set; }

        public Matrix Projection { get; protected set; }

        public BoundingFrustum BoundingFrustum { get; protected set; }

        #endregion

        #region Public methods

        #region Camera movement and rotationAngles

        public void Move(Vector3 movement)
        {
            Position += movement;
            Target += movement;
        }

        public void MoveView(Vector3 movement)
        {
            var eyeLookAxis = (Position - Target);
            eyeLookAxis.Normalize();
            var eyeUpAxis = -Vector3.Cross(eyeLookAxis, Vector3.UnitX);
            eyeUpAxis.Normalize();
            var eyeRightAxis = -Vector3.Cross(eyeLookAxis, Vector3.UnitY);
            eyeRightAxis.Normalize();

            var translatedMovement = eyeLookAxis * movement.Z
                + eyeUpAxis * movement.Y
                + eyeRightAxis * movement.X;

            Position += translatedMovement;
            Target += translatedMovement;
        }

        public void RotateCamera(CameraRotation rotationAngles)
        {
            rotation = Matrix.CreateFromYawPitchRoll(
                MathHelper.ToRadians(rotationAngles.Yaw), 
                MathHelper.ToRadians(rotationAngles.Pitch), 
                MathHelper.ToRadians(rotationAngles.Roll));
            Target = Position + Vector3.Transform(-Vector3.UnitZ, rotation);
        }

        public void RotateCameraView(CameraRotation rotationAngles)
        {
            rotation = Rotation * Matrix.CreateFromYawPitchRoll(
                MathHelper.ToRadians(rotationAngles.Yaw),
                MathHelper.ToRadians(rotationAngles.Pitch),
                MathHelper.ToRadians(rotationAngles.Roll));
            Target = Position + Vector3.Transform(-Vector3.UnitZ, rotation);
        }

        public void AssignCamera(GraphicsSystem graphics)
        {
            var variables = graphics.Variables;
            variables.Set(VariableFloatId.FarClipPlane, FarClippingPlane);
            variables.Set(VariableFloat3Id.CameraPosition, position);
            variables.Set(VariableMatrixId.World, Matrix.Identity);
            variables.Set(VariableMatrixId.View, View);
            variables.Set(VariableMatrixId.Projection, Projection);
        }

        #endregion

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (needsUpdate)
            {
                ComputeCameraView();
                needsUpdate = false;
            }
        }

        public abstract string GetCameraInfo();

        protected abstract void ComputeCameraView();

        protected void Invalidate()
        {
            needsUpdate = true;
        }

        #region Private fields
        
        private bool needsUpdate;

        private Vector3 position;

        private Vector3 target;

        private Vector3 upVector;

        private Matrix rotation;

        private float aspectRatio;

        private float nearClippingPlane;

        private float farClippingPlane;

        private float fieldOdView;

        #endregion
    }
}
