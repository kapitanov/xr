using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Shader parameter to matrix variable binding.
    /// </summary>
    internal struct VariableMatrixBinding
    {
        public EffectParameter parameter;

        public VariableMatrixId matrixId;
    }
}
