using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to float2 constant binding.
    /// </summary>
    internal struct ConstantFloat2Binding
    {
        public EffectParameter parameter;

        public Vector2 constant;
    }
}
