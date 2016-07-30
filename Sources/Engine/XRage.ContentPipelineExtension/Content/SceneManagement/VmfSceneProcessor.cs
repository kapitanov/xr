using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace AISTek.XRage.Content.SceneManagement
{
    [ContentProcessor(DisplayName = "VMF scene processor")]
    public class VmfSceneProcessor : ContentProcessor<CompiledVmfScene, CompiledVmfScene>
    {
        public override CompiledVmfScene Process(CompiledVmfScene input, ContentProcessorContext context)
        {
            return input;
        }
    }
}
