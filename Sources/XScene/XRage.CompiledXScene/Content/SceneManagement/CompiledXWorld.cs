using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledXWorld
    {
        public CompiledXWorld(IEnumerable<CompiledXBrush> brushes, CompiledXTerrain terrain)
        {
            Brushes = new List<CompiledXBrush>(brushes);
            Terrain = terrain;
        }

        private CompiledXWorld(List<CompiledXBrush> brushes, CompiledXTerrain terrain)
        {
            Brushes = brushes;
            Terrain = terrain;
        }

        public List<CompiledXBrush> Brushes { get; set; }

        public CompiledXTerrain Terrain { get; set; }

        public bool HasTerrain { get { return Terrain != null; } }

        public void WriteToContent(IContentWriterWrapper output)
        {
            // Write brushes
            output.Write(Brushes.Count);
            foreach (var brush in Brushes)
            {
                brush.WriteToContent(output);
            }

            // Write terrain if has one
            output.Write(HasTerrain);
            if (HasTerrain)
            {
                Terrain.WriteToContent(output);
            }
        }

        public static CompiledXWorld ReadFromContent(ContentReader input)
        {
            // Read brushes
            var brushesCount = input.ReadInt32();
            var brushes = Enumerable.Range(0, brushesCount)
                .Select(_ => CompiledXBrush.ReadFromContent(input))
                .ToList();

            CompiledXTerrain terrain = null;
            if (input.ReadBoolean())
            {
                terrain = CompiledXTerrain.ReadFromContent(input);
            }

            return new CompiledXWorld(brushes, terrain);
        }
    }
}
