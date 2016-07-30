using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Content.Graphics
{
    /// <summary>
    /// Shader constant definition.
    /// </summary>
    public struct ConstantParameter
    {
        public string Semantic { get; set; }

        public int NumValues { get; set; }

        public float[] Values { get; set; }
    }
}
