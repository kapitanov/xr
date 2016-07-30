using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public interface IVector4Operation
    {
        VariableFloat4Id FirstOperandId { get; }

        VariableFloat4Id SecondOperandId { get; }

        VariableFloat4Id ResultId { get; }

        void PerformOperation(ref Vector4 x, ref Vector4 y, out Vector4 result);
    }
}
