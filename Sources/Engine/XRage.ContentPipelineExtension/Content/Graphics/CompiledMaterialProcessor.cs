using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.IO;

namespace AISTek.XRage.Content.Graphics
{
    /// <summary>
    /// Content pipeline processor for X-Rage compiled materials.
    /// </summary>
    [ContentProcessor(DisplayName = "XMaterial Processor")]
    public class CompiledMaterialProcessor : ContentProcessor<CompiledMaterial, CompiledMaterial>
    {
        /// <summary>
        /// Processes a compiled material and prepares it for disk writing.
        /// </summary>
        /// <param name="action">
        /// The CompiledMaterial instance to process.
        /// </param>
        /// <param name="context">
        /// The current content processor context.
        /// </param>
        /// <returns></returns>
        public override CompiledMaterial Process(
            CompiledMaterial input,
            ContentProcessorContext context)
        {
            input.CompiledEffect = context.BuildAsset<EffectContent, CompiledEffectContent>(
                new ExternalReference<EffectContent>(
                    string.Format("{0}{1}.fx", Path.GetFullPath("shaders/"), input.Effect)),
                    "EffectProcessor");

            // TODO: ExternalReference to textures, or leave management to run-time renderer?

            return input;
        }
    }
}
