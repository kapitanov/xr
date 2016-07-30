using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to float3 constant binding.
    /// </summary>
    internal struct ConstantFloat3Binding
    {
        public EffectParameter parameter;
        public Vector3 constant;
    }
}
