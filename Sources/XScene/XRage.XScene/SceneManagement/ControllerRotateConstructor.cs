using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using AISTek.XRage.Content.SceneManagement;

namespace AISTek.XRage.SceneManagement
{
    public class ControllerRotateConstructor : IEntityConstuctor
    {
        public string Class { get { return "controller.rotate"; } }

        public BaseEntity CreateEntity(XGame game, CompiledEntity entity)
        {
            var name = entity.StringProperty(Properties.Id);
            var controlledEntityName = entity.StringProperty(Properties.TargetName);

            var controller = new RotateEntityController(game, name, controlledEntityName);

            if (entity.HasProperty(Properties.Radius))
            {
                controller.Radius = entity.FloatProperty(Properties.Radius);
            }

            if (entity.HasProperty(Properties.Axis))
            {
                controller.RotationAxis = entity.Vector3Property(Properties.Axis);
            }

            if (entity.HasProperty(Properties.Speed))
            {
                controller.AngularSpeed = entity.FloatProperty(Properties.Speed);
            }

            if (entity.HasProperty(Properties.Center))
            {
                controller.RotationCenter = entity.Vector3Property(Properties.Center);
            }

            if (entity.HasProperty(Properties.Angle))
            {
                controller.Angle = entity.FloatProperty(Properties.Angle);
            }

            return controller;
        }
    }
}
