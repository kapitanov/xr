using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Content.SceneManagement;
using AISTek.XRage.Entities;

namespace AISTek.XRage.SceneManagement
{
    public class PlayerControlledModelConstructor :IEntityConstuctor
    {
        public string Class { get { return "info.player_model"; } }

        public BaseEntity CreateEntity(XGame game, CompiledEntity entity)
        {
            var model = entity.StringProperty(Properties.Model);
            var material = entity.StringProperty(Properties.Material);

            var player = new PlayerControlledModel(game, PropertyConvertor.ModelPath(model), PropertyConvertor.MaterialPath(material));

            if (entity.HasProperty(Properties.Position))
                player.Position = entity.Vector3Property(Properties.Position);

            return player;
        }
    }
}
