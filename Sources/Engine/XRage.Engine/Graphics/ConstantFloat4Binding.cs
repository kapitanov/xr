using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to float4 constant binding.
    /// </summary>
    internal struct ConstantFloat4Binding
    {
        public EffectParameter parameter;

        public Vector4 constant;
    }
}
