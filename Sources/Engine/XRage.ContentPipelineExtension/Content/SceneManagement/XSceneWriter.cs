using AISTek.XRage.SceneManagement;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace AISTek.XRage.Content.SceneManagement
{
    [ContentTypeWriter]
    public class XSceneWriter : ContentTypeWriter<CompiledXScene>
    {
        protected override void Write(ContentWriter output, CompiledXScene scene)
        {
            scene.WriteToContent(new ContentWriterWrapper(output));
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(CompiledXSceneReader).AssemblyQualifiedName;
        }
    }
}
