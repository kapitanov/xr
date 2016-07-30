using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledXSceneReader : ContentTypeReader<CompiledXScene>
    {
        protected override CompiledXScene Read(ContentReader input, CompiledXScene existingInstance)
        {
            return CompiledXScene.ReadFromContent(input);
        }
    }
}
