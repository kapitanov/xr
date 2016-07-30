using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    public interface IFloatOperation
    {
        VariableFloatId FirstOperandId { get; }

        VariableFloatId SecondOperandId { get; }

        VariableFloatId ResultId { get; }

        void PerformOperation(float x, float y, out float result);
    }
}
