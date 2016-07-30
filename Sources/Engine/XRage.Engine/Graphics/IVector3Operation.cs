using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public interface IVector3Operation
    {
        VariableFloat3Id FirstOperandId { get; }

        VariableFloat3Id SecondOperandId { get; }

        VariableFloat3Id ResultId { get; }

        void PerformOperation(ref Vector3 x, ref Vector3 y, out Vector3 result);
    }
}
