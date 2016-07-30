using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Messaging.EntityMessages;

namespace AISTek.XRage.Entities
{
    public class AnimateColorController : BasePropertyController
    {
        public AnimateColorController(XGame game, string name, string controlledEntityName, string propertyName)
            : base(game, name, controlledEntityName, propertyName)
        { }

        public Color FromColor { get; set; }

        public Color ToColor { get; set; }

        public float Speed
        {
            get { return MathHelper.ToDegrees(speed); }
            set { speed = MathHelper.ToRadians(value); }
        }

        public float Position
        {
            get { return MathHelper.ToDegrees(position); }
            set { position = MathHelper.ToRadians(value); }
        }

        protected override void UpdateControlledEntity(GameTime gameTime)
        {
            position += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);

            var value = new Color(FromColor.ToVector3() * (float)Math.Cos(position) +
                                  ToColor.ToVector3() * (float)Math.Sin(position));
            var msg = XObjectPool.Acquire<MessageSetColorProperty>();
            msg.PropertyName = ControlledPropertyName;
            msg.PropertyValue = value;
            ControlledEntity.ExecuteMessage(msg);
        }

        private float speed;
        private float position;
    }
}
