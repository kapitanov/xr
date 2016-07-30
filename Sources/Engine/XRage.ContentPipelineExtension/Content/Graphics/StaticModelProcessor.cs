using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace AISTek.XRage.Content.Graphics
{
    [ContentProcessor(DisplayName = "X-Rage Static model Processor")]
    public class StaticModelProcessor : ModelProcessor
    {
        public override bool GenerateTangentFrames
        {
            get { return true; }
            set{ }
        }
        protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
        {
            material.Textures.Clear();
            return context.Convert<MaterialContent, MaterialContent>(material, typeof(MaterialProcessor).Name);
        }
    }
}
