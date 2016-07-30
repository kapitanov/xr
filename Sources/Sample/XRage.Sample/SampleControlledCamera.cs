using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AISTek.XRage.Sample
{
    public class SampleControlledCamera : BaseCamera
    {
        public SampleControlledCamera(XGame game)
            : base(game, "sample_camera", null)
        {
            FieldOfView = 30.0f;
            ComputeCameraView();
        }

        public Vector2 ViewRotation
        {
            get { return viewRotation; }
            set
            {
                viewRotation = value;
                Invalidate();
            }
        }

        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var rotation = Vector2.Zero;
            var move = Vector3.Zero;

            // Move forward
            if (state.IsKeyDown(Keys.W))
                move += new Vector3(0f, 0f, -1f);
            // Move backward
            if (state.IsKeyDown(Keys.S))
                move += new Vector3(0f, 0f, 1f);

            // Move left
            if (state.IsKeyDown(Keys.A))
                move += new Vector3(-1f, 0f, 0f);
            // Move right
            if (state.IsKeyDown(Keys.D))
                move += new Vector3(1f, 0f, 0f);

            // Move up
            if (state.IsKeyDown(Keys.E))
                move += new Vector3(0f, 1f, 0f);
            // Move down
            if (state.IsKeyDown(Keys.Q))
                move += new Vector3(0f, -1f, 0f);

            // Fast move
            if (state.IsKeyDown(Keys.LeftShift) || 
                state.IsKeyDown(Keys.RightShift))
                move *= 25f;

            // Extra fast move
            if (state.IsKeyDown(Keys.LeftControl) ||
                state.IsKeyDown(Keys.RightControl))
                move *= 250f;

            // Extra super fast move
            if (state.IsKeyDown(Keys.LeftAlt) || 
                state.IsKeyDown(Keys.RightAlt))
                move *= 2000f;

            // Rotate up
            if (state.IsKeyDown(Keys.Up))
                rotation += Vector2.UnitY;
            // Rotate down
            if (state.IsKeyDown(Keys.Down))
                rotation -= Vector2.UnitY;

            // Rotate right
            if (state.IsKeyDown(Keys.Right))
                rotation -= Vector2.UnitX;
            // Rotate left
            if (state.IsKeyDown(Keys.Left))
                rotation += Vector2.UnitX;


            if (move.Length() > 0)
                MoveView(move);

            if (rotation.Length() > 0)
                RotateView(rotation * 0.25f);

            base.Update(gameTime);
        }

        public void RotateView(Vector2 viewRotation)
        {
            ViewRotation += viewRotation;
            ViewRotation = LimitY(ViewRotation, MinAngle, MaxAngle);

            Target = Position + Vector3.Transform(
                -Vector3.UnitZ,
                Matrix.CreateRotationX(MathHelper.ToRadians(ViewRotation.Y))
                    * Matrix.CreateRotationY(MathHelper.ToRadians(ViewRotation.X)));
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
            if (translatedMovement.X != float.NaN &&
                translatedMovement.Y != float.NaN &&
                translatedMovement.Z != float.NaN)
            {
                Position += translatedMovement;
                Target += translatedMovement;
            }
        }

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

        private static Vector2 LimitY(Vector2 vector, float min, float max)
        {
            if (vector.Y > max)
            {
                return new Vector2(vector.X, max);
            }

            if (vector.Y < min)
            {
                return new Vector2(vector.X, min);
            }

            return vector;
        }

        private Vector2 viewRotation;
        private const float MaxAngle = 90f - 0.1f;
        private const float MinAngle = -MaxAngle;

        public override string GetCameraInfo()
        {
            return string.Format("controlled_perspective_camera:\nposition: {0}\nrotation: {1}", Position, ViewRotation);
        }
    }
}
