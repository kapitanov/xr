using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledXTerrain
    {
        public string MaterialPath { get; set; }

        public string HeightMapPath { get; set; }

        public float Scale { get; set; }

        public float Elevation { get; set; }

        public void WriteToContent(IContentWriterWrapper output)
        {
            output.Write(MaterialPath);
            output.Write(HeightMapPath);
            output.Write(Scale);
            output.Write(Elevation);
        }

        public static CompiledXTerrain ReadFromContent(ContentReader input)
        {
            return new CompiledXTerrain
            {
                MaterialPath = input.ReadString(),
                HeightMapPath = input.ReadString(),
                Scale = input.ReadSingle(),
                Elevation = input.ReadSingle()
            };
        }
    }
}
