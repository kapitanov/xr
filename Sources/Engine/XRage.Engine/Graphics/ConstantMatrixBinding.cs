using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to matrix constant binding.
    /// </summary>
    internal struct ConstantMatrixBinding
    {
        public EffectParameter parameter;

        public Matrix constant;
    }
}
