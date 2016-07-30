using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using AISTek.XRage.SceneManagement;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace AISTek.XRage.Content.SceneManagement
{
    [ContentTypeWriter]
    public class VmfSceneWriter : ContentTypeWriter<CompiledVmfScene>
    {
        protected override void Write(ContentWriter output, CompiledVmfScene value)
        {
            output.Write(value.Brushes.Count);

            foreach (var brush in value.Brushes)
            {
                output.Write(brush.MaterialPath);
                output.Write(brush.Sides.Count);

                foreach (var side in brush.Sides)
                {
                    foreach (var vertex in side.Vertices)
                    {
                        output.Write(vertex.Position);
                        output.Write(vertex.TextureCoordinate);
                        output.Write(vertex.Normal);
                    }
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(VmfSceneReader).AssemblyQualifiedName;
        }
    }
}
