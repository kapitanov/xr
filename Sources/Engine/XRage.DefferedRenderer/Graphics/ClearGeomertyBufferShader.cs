using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
   public sealed class ClearGeomertyBufferShader : Shader<DeferredRenderer>
    {
       public ClearGeomertyBufferShader(DeferredRenderer renderer)
           : base(renderer)
       {
           Initialize();
       }

       public override void Initialize()
       {
           LoadShader("shaders/deferred_renderer/clear_gbuffer");
       }

       public Effect Bind()
       {
           UseTechnique("ClearGBuffer");
           return Effect;
       }
    }
}
