using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Messaging;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using System.Diagnostics;
using AISTek.XRage.Messaging.EntityMessages;

namespace AISTek.XRage.Components
{
    public class LightEmitterComponent : BaseComponent
    {
        public LightEmitterComponent(
            BaseEntity entity,
            Color diffuseColor,
            Color specularColor,
            LightType type)
            : base(entity)
        {
            DiffuseColor = diffuseColor;
            SpecularColor = specularColor;
            Type = type;
            Intensity = 1.0f;
            Direction = -Vector3.UnitY;
            Position = Vector3.Zero;
            Radius = 100.0f;
            Falloff = FalloffType.Linear;
        }

        public Color DiffuseColor { get; set; }

        public Color SpecularColor { get; set; }

        public LightType Type { get; set; }

        public Vector3 Direction { get; set; }

        public Vector3 Position { get; set; }

        public float Intensity { get; set; }

        public float Radius { get; set; }

        public float InnerConeAngle { get; set; }

        public float OuterConeAngle { get; set; }

        public FalloffType Falloff { get; set; }

        public bool CastsShadows { get; set; }

        public bool SoftShadows { get; set; }

        public override void QueryForChunks(ref RenderPassDescriptor pass)
        {
            if (pass.Type == RenderPassType.Lighting)
            {
                if (Type != LightType.Directional)
                {
                    if (!pass.RenderCamera.BoundingFrustum.Intersects(new BoundingSphere(Position, Radius)))
                        return;
                }

                var chunk = Game.Graphics.RenderChunkManager.AllocateLightChunk();

                chunk.Position = Position;
                chunk.Direction = Vector3.Transform(Direction, ParentEntity.Rotation);
                chunk.Type = Type;
                chunk.DiffuseColor = DiffuseColor.ToVector3();
                chunk.SpecularColor = SpecularColor.ToVector3();
                chunk.InnerConeAngle = InnerConeAngle;
                chunk.OuterConeAngle = OuterConeAngle;
                chunk.Intensity = Intensity;
                chunk.Radius = Radius;
                chunk.Falloff = Falloff;
                chunk.CastsShadows = CastsShadows;
                chunk.SoftShadows = SoftShadows;
            }
        }

        public override bool ExecuteMessage(IMessage message)
        {
            switch(message.Type)
            {
                case (int)MessageType.Entity.SetColorProperty:
                    var setColorMsg = message.TypeCheck<MessageSetColorProperty>();
                    SetColorProperty(setColorMsg.PropertyName, setColorMsg.PropertyValue);
                    return true;
            }
            return false;
        }

        private void SetColorProperty(string propertyName, Color value)
        {
            switch(propertyName)
            {
                case "diffuseColor":
                    DiffuseColor = value;
                    return;

                case "specularColor":
                    SpecularColor = value;
                    return;

                case "color":
                    DiffuseColor = value;
                    SpecularColor = value;
                    return;
            }
        }
    }
}
