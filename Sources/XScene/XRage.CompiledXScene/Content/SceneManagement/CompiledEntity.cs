using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledEntity
    {
        public CompiledEntity(string className, IDictionary<string, string> properties)
        {
            ClassName = className;
            Properties = new Dictionary<string, string>(properties);
        }

        private CompiledEntity(string className, Dictionary<string, string> properties)
        {
            ClassName = className;
            Properties = properties;
        }

        public string ClassName { get; private set; }

        public Dictionary<string, string> Properties { get; private set; }

        public void WriteToContent(IContentWriterWrapper output)
        {
            // Write class
            output.Write(ClassName);

            // Write properties
            output.Write(Properties.Count);
            foreach (var pair in Properties)
            {
                output.Write(pair.Key);
                output.Write(pair.Value);
            }
        }

        public static CompiledEntity ReadFromContent(ContentReader input)
        {
            // Read class
            var className = input.ReadString();

            // Read properties
            var propertiesCount = input.ReadInt32();
            var properties = Enumerable.Range(0, propertiesCount)
                .Select(_ => new { Key = input.ReadString(), Value = input.ReadString() })
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            return new CompiledEntity(className, properties);
        }
    }
}
