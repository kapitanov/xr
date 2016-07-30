using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to texture variable binding.
    /// </summary>
    internal struct VariableTexture2DBinding
    {
        public EffectParameter parameter;

        public VariableTexture2d textureId;
    }
}
