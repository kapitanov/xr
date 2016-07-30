using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public interface IVector2Operation
    {
        VariableFloat2Id FirstOperandId { get; }

        VariableFloat2Id SecondOperandId { get; }

        VariableFloat2Id ResultId { get; }

        void PerformOperation(ref Vector2 x, ref Vector2 y, out Vector2 result);
    }
}
