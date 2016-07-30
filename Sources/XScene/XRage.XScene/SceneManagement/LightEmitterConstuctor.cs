using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using AISTek.XRage.Graphics;
using AISTek.XRage.Content.SceneManagement;

namespace AISTek.XRage.SceneManagement
{
    public class LightEmitterConstuctor : IEntityConstuctor
    {
        public string Class { get { return "light.generic"; } }

        public BaseEntity CreateEntity(XGame game, CompiledEntity entity)
        {
            var name = entity.StringProperty(Properties.Id);
            var type = entity.LightTypeProperty(Properties.Type);
            var color = entity.ColorProperty(Properties.Color);

            var light = new LightEmitter(game, name, color, type);

            if (entity.HasProperty(Properties.Radius))
            {
                light.Radius = entity.FloatProperty(Properties.Radius);
            }

            if (entity.HasProperty(Properties.Position))
            {
                light.Position = entity.Vector3Property(Properties.Position);
            }

            if (entity.HasProperty(Properties.Direction))
            {
                light.Direction = entity.Vector3Property(Properties.Direction);
            }

            if (entity.HasProperty(Properties.Intensity))
            {
                light.Intensity = entity.FloatProperty(Properties.Intensity);
            }

            if (entity.HasProperty(Properties.Falloff))
            {
                light.Falloff = entity.FalloffTypeProperty(Properties.Falloff);
            }

            if (entity.HasProperty(Properties.OuterAngle))
            {
                light.OuterConeAngle = entity.FloatProperty(Properties.OuterAngle);
            }

            if (entity.HasProperty(Properties.InnerAngle))
            {
                light.InnerConeAngle = entity.FloatProperty(Properties.InnerAngle);
            }

            if (entity.HasProperty(Properties.CastsShadows))
            {
                light.CastsShadows = entity.BoolProperty(Properties.CastsShadows);
            }

            if (entity.HasProperty(Properties.SoftShadows))
            {
                light.SoftShadows = entity.BoolProperty(Properties.SoftShadows);
            }

            return light;
        }
    }
}
