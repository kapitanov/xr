using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using AISTek.XRage.SceneManagement;

namespace AISTek.XRage.Content.SceneManagement
{
    [ContentProcessor(DisplayName = "XScene default processor")]
    public class XSceneProcessor : ContentProcessor<XDocument, CompiledXScene>
    {
        public override CompiledXScene Process(XDocument input, ContentProcessorContext context)
        {
            var root = input.Descendants("xscene").First();

            var levelProperties = root.Descendants("xscene.properties")
                                      .Descendants()
                                      .ToDictionary(property => property.Name.ToString(), property => property.Value);

            var world = xWorldImporter.Process(root.Descendants("xscene.world").Descendants("world").First());

            var entities = root.Descendants("xscene.entities")
                               .Descendants()
                               .Select(ReadEntity);

            return new CompiledXScene(levelProperties, world, entities);
        }

        private static CompiledEntity ReadEntity(XElement root)
        {
            return new CompiledEntity(
                root.Name.ToString(), 
                root.Attributes().ToDictionary(
                    attribute => attribute.Name.ToString(), 
                    attribute => attribute.Value));

        }

        private CompiledXWorldImporter xWorldImporter = new CompiledXWorldImporter();
    }
}
