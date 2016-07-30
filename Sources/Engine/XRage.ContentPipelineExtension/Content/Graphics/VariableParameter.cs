using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.Graphics
{
    /// <summary>
    /// Shader variable definition.
    /// </summary>
    public struct VariableParameter
    {
        public string Semantic { get; set; }

        public string Variable { get; set; }

        public VariableType Type { get; set; }
    }
}
