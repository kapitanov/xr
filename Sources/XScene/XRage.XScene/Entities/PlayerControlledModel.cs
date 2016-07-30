using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using AISTek.XRage.Components;
using AISTek.XRage.InputManagement;

namespace AISTek.XRage.Entities
{
    public class PlayerControlledModel : BaseCamera
    {
        public PlayerControlledModel(XGame game, string modelPath, string materialPath)
            : base(game, "player", null)
        {
            CameraRotation = new CameraRotation();
            FieldOfView = 35.0f;
            NearClippingPlane = 0.1f;
            FarClippingPlane = 10000.0f;

            ComputeCameraView();

            var renderComponent = new ViewModelRenderComponent(this, modelPath, materialPath)
            {
                Offset = new Vector3(0, -25, -75)
            };

            AddComponent(renderComponent);
           // ActivateComponent(renderComponent);

            Game.Interfaces.QueryInterface<CameraInterface>().ActiveCamera = this;
                        
            Game.Input.GetAction("camera.moveForward").OnFire += (_, e) => ApplyMovement(KeyboardSense);
            Game.Input.GetAction("camera.moveBackward").OnFire += (_, e) => ApplyMovement(-KeyboardSense);
        }

        public CameraRotation CameraRotation { get; set; }

        public override void Update(GameTime gameTime)
        {
            if (Game.Input.GetAction("camera.fastMovement").IsOn)
            {
                KeyboardSense = 25f;
            }
            else
            {
                KeyboardSense = 0.25f;
            }

            const float  x = 1000.0f/60.0f;
            var rotationQuant = (float)gameTime.ElapsedGameTime.TotalMilliseconds / x;

            ApplyRotation(-MouseSense * rotationQuant * Game.Input.InputState.Mouse.MouseDeltaPosition);                            
            ComputeCameraView();
            base.Update(gameTime);
        }

        public override string GetCameraInfo()
        {
            return string.Format("info.player_model {0} {1}", Position, CameraRotation);
        }

        protected override void ComputeCameraView()
        {
            Rotation = Matrix.CreateFromYawPitchRoll(CameraRotation.Yaw, CameraRotation.Pitch, CameraRotation.Roll);
            Target = Vector3.Transform(-Vector3.UnitZ, Rotation) + Position;

            View = Matrix.CreateLookAt(Position, Target, UpVector);
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(FieldOfView),
                AspectRatio,
                NearClippingPlane,
                FarClippingPlane);

            BoundingFrustum = new BoundingFrustum(View * Projection);
        }

        private void ApplyRotation(Vector2 yawPitch)
        {
            ApplyRotation(yawPitch.X, yawPitch.Y);
        }

        private void ApplyRotation(float yaw, float pitch)
        {
            CameraRotation.Yaw += yaw;
            CameraRotation.Pitch += pitch;
        }

        private void ApplyMovement(float distance)
        {
            Position += distance * Vector3.Transform(-Vector3.UnitZ, Rotation);
        }

        private int oldX;
        private int oldY;

        private float MouseSense = 0.05f;
        private float KeyboardSense = 0.25f;
    }
}
