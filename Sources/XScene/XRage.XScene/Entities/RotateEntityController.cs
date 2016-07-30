using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using AISTek.XRage.Messaging.EntityMessages;

namespace AISTek.XRage.Entities
{
    public class RotateEntityController : BaseController
    {
        public RotateEntityController(XGame game, string name, string controlledEntityName)
            : base(game, name, controlledEntityName)
        { }

        public Vector3 RotationCenter { get; set; }

        public Vector3 RotationAxis { get; set; }

        public float Radius { get; set; }

        public float AngularSpeed
        {
            get { return MathHelper.ToDegrees(angularSpeed); }
            set { angularSpeed = MathHelper.ToRadians(value); }
        }

        public float Angle
        {
            get { return MathHelper.ToDegrees(angle); }
            set { angle = MathHelper.ToRadians(value); }
        }

        protected override void UpdateControlledEntity(GameTime gameTime)
        {
            angle += (float)(angularSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            // TODO: this version can rotate only in XOZ plane
            // And keeps the controlled entity rotation
            //var newPosition = (Radius * new Vector3((float)Math.Cos(position), 0.0f, -(float)Math.Sin(position))) + RotationCenter;
            //ControlledEntity.Position = newPosition;

            var rotation = Matrix.CreateFromAxisAngle(RotationAxis, angle);
            var newPosition = Vector3.Transform(RotationCenter, Matrix.CreateTranslation(Radius, 0, 0) * rotation);

            var msgSetPosition = XObjectPool.Acquire<MessageSetPosition>();
            msgSetPosition.Data = newPosition;
            ControlledEntity.ExecuteMessage(msgSetPosition);

            var msgSetRotation = XObjectPool.Acquire<MessageSetRotation>();
            msgSetRotation.Data = rotation;
            ControlledEntity.ExecuteMessage(msgSetRotation);
        }

        private float angularSpeed;
        private float angle;
    }
}
