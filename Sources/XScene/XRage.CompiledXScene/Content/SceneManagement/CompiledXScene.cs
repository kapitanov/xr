using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledXScene
    {
        public CompiledXScene(
            IDictionary<string, string> properties,
            CompiledXWorld world,
            IEnumerable<CompiledEntity> entities)
        {
            Properties = new Dictionary<string, string>(properties);
            World = world;
            Entities = new List<CompiledEntity>(entities);
        }

        private CompiledXScene(
            Dictionary<string, string> properties,
            CompiledXWorld world,
            List<CompiledEntity> entities)
        {
            Properties = properties;
            World = world;
            Entities = entities;
        }

        public Dictionary<string, string> Properties { get; private set; }

        public CompiledXWorld World { get; private set; }

        public List<CompiledEntity> Entities { get; private set; }

        public void WriteToContent(IContentWriterWrapper output)
        {
            // Write properties
            output.Write(Properties.Count);
            foreach (var pair in Properties)
            {
                output.Write(pair.Key);
                output.Write(pair.Value);
            }

            // Write world geometry
            World.WriteToContent(output);

            // Write entities
            output.Write(Entities.Count);
            foreach (var entity in Entities)
            {
                entity.WriteToContent(output);
            }
        }

        public static CompiledXScene ReadFromContent(ContentReader input)
        {
            // Read properties
            var propertiesCount = input.ReadInt32();
            var properties = Enumerable.Range(0, propertiesCount)
                .Select(_ => new { Key = input.ReadString(), Value = input.ReadString() })
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            
            // Read world geometry
            var world = CompiledXWorld.ReadFromContent(input);

            // Read entities
            var entitiesCount = input.ReadInt32();
            var entities = Enumerable.Range(0, entitiesCount)
                .Select(_ => CompiledEntity.ReadFromContent(input))
                .ToList();

            return new CompiledXScene(properties, world, entities);
        }
    }
}
