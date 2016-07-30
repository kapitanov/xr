using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Components;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Entities
{
    public class LightEmitter : BaseEntity
    {
        public LightEmitter(XGame game, string name, Color color, LightType type)
            : base(game, name)
        {
            lightEmitterComponent = new LightEmitterComponent(this, color, color, type);
            AddComponent(lightEmitterComponent);
            ActivateComponent(lightEmitterComponent);

            // Initialize properties with their default values
            Direction = Vector3.One;
            InnerConeAngle = 30;
            OuterConeAngle = 90;
            Radius = 100;
            CastsShadows = false;
        }

        public Color DiffuseColor
        {
            get { return lightEmitterComponent.DiffuseColor; }
            set { lightEmitterComponent.DiffuseColor = value; }
        }

        public Color SpecularColor
        {
            get { return lightEmitterComponent.SpecularColor; }
            set { lightEmitterComponent.SpecularColor = value; }
        }

        public LightType Type
        {
            get { return lightEmitterComponent.Type; }
            set { lightEmitterComponent.Type = value; }
        }

        public Vector3 Direction
        {
            get { return lightEmitterComponent.Direction; }
            set { lightEmitterComponent.Direction = value; }
        }

        public override Vector3 Position
        {
            get
            {
                if (lightEmitterComponent != null)
                    return lightEmitterComponent.Position;
                return Vector3.Zero;
            }
            set
            {
                if (lightEmitterComponent != null)
                    lightEmitterComponent.Position = value;
            }
        }

        public float Intensity
        {
            get { return lightEmitterComponent.Intensity; }
            set { lightEmitterComponent.Intensity = value; }
        }

        public float Radius
        {
            get { return lightEmitterComponent.Radius; }
            set { lightEmitterComponent.Radius = value; }
        }

        public float InnerConeAngle
        {
            get { return lightEmitterComponent.InnerConeAngle; }
            set 
            {
                // Limit inner cone angle to range: [0, OuterConeAngle]
                lightEmitterComponent.InnerConeAngle = MathHelper.Clamp(value, MinimumConeAngle, OuterConeAngle); 
            }
        }

        public float OuterConeAngle
        {
            get { return lightEmitterComponent.OuterConeAngle; }
            set 
            {
                // Limit outer cone angle to range: [0, Pi/2]
                lightEmitterComponent.OuterConeAngle = MathHelper.Clamp(value, MinimumConeAngle, MaximumConeAngle); 
            }
        }

        public FalloffType Falloff
        {
            get { return lightEmitterComponent.Falloff; }
            set { lightEmitterComponent.Falloff = value; }
        }

        public bool CastsShadows
        {
            get { return lightEmitterComponent.CastsShadows; }
            set { lightEmitterComponent.CastsShadows = value; }
        }

        public bool SoftShadows 
        {
            get { return lightEmitterComponent.SoftShadows; }
            set { lightEmitterComponent.SoftShadows = value; }
        }

        private readonly LightEmitterComponent lightEmitterComponent;
        private const float MinimumConeAngle = 0.0f;
        private const float MaximumConeAngle = 90.0f;
    }
}
