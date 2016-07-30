using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Content.Graphics
{
    /// <summary>
    /// Writes a compiled material instance to a content pipeline data file.
    /// </summary>
    [ContentTypeWriter]
    public class CompiledMaterialWriter : ContentTypeWriter<CompiledMaterial>
    {
        /// <summary>
        /// Writes the CompiledMaterial instance to a content pipeline data file.
        /// </summary>
        /// <param name="scene">The ContentWriter instance for the data file.</param>
        /// <param name="value">The CompiledMaterial instance to write.</param>
        protected override void Write(ContentWriter output, CompiledMaterial value)
        {
            value.Write(output);
        }

        /// <summary>
        /// Retrieves the type string for the content reader responsible for reading compiled material content files.
        /// </summary>
        /// <param name="targetPlatform">The target platform for this compiled model.</param>
        /// <returns>The type string for the type reader.</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(MaterialReader).AssemblyQualifiedName;
        }
    }
}
