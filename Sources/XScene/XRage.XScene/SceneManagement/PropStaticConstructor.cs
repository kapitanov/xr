using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Content.SceneManagement;
using AISTek.XRage.Entities;

namespace AISTek.XRage.SceneManagement
{
    public class PropStaticConstructor : IEntityConstuctor
    {
        public string Class { get { return "prop.static"; } }

        public BaseEntity CreateEntity(XGame game, CompiledEntity entity)
        {
            var name = entity.StringProperty(Properties.Id);
            var model = entity.StringProperty(Properties.Model);
            var material = entity.StringProperty(Properties.Material);

            var propStatic = new StaticModelEntity(game, name, PropertyConvertor.ModelPath(model), PropertyConvertor.MaterialPath(material));

            if (entity.HasProperty(Properties.Position))
                propStatic.Position = entity.Vector3Property(Properties.Position);

            return propStatic;
        }
    }
}
