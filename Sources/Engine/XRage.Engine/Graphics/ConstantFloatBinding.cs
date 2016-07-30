using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to float-point constant binding.
    /// </summary>
    internal struct ConstantFloatBinding
    {
        public EffectParameter parameter;

        public float constant;
    }
}
