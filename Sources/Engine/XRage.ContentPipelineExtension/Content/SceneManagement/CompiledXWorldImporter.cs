using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.SceneManagement;
using System.Xml.Linq;

namespace AISTek.XRage.Content.SceneManagement
{
    internal class CompiledXWorldImporter
    {
        public CompiledXWorld Process(XElement worldRoot)
        {
            var brushes = worldRoot.Descendants("world.statics")
                                   .Descendants("brush")
                                   .Select(brushData => xBrushImporter.Process(brushData));


            CompiledXTerrain terrain = null;
            var terrainNode = worldRoot.Descendants("world.terrain").Descendants("terrain").FirstOrDefault();
            if (terrainNode != null)
            {
                terrain = new CompiledXTerrain
                {
                    MaterialPath = terrainNode.Attribute("material").Value,
                    HeightMapPath = terrainNode.Attribute("heightMap").Value,
                    Scale = terrainNode.ReadFloatAttribute("scale"),
                    Elevation = terrainNode.ReadFloatAttribute("elevation")
                };
            }

            return new CompiledXWorld(brushes, terrain);
        }

        private CompiledXBrushImporter xBrushImporter = new CompiledXBrushImporter();
    }
}
