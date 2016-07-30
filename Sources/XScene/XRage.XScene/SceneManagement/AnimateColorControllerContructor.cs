using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Content.SceneManagement;
using AISTek.XRage.Entities;

namespace AISTek.XRage.SceneManagement
{
    public class AnimateColorControllerContructor : IEntityConstuctor
    {
        public string Class { get { return "controller.animateColor"; } }

        public Entities.BaseEntity CreateEntity(XGame game, CompiledEntity entity)
        {
            var name = entity.StringProperty(Properties.Id);
            var controlledEntityName = entity.StringProperty(Properties.TargetName);
            var controlledPropertyName = entity.StringProperty(Properties.Property);

            var controller = new AnimateColorController(game, name, controlledEntityName, controlledPropertyName);

            if (entity.HasProperty(Properties.From))
            {
                controller.FromColor = entity.ColorProperty(Properties.From);
            }

            if (entity.HasProperty(Properties.To))
            {
                controller.ToColor = entity.ColorProperty(Properties.To);
            }

            if (entity.HasProperty(Properties.Speed))
            {
                controller.Speed = entity.FloatProperty(Properties.Speed);
            }

            if (entity.HasProperty(Properties.Angle))
            {
                controller.Position = entity.FloatProperty(Properties.Angle);
            }

            return controller;
        }
    }
}
