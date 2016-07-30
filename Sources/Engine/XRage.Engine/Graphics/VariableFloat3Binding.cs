using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to float3 variable binding.
    /// </summary>
    internal struct VariableFloat3Binding
    {
        public EffectParameter parameter;

        public VariableFloat3Id varId;
    }
}
