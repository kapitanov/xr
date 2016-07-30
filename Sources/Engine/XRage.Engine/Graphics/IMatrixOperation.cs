using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public interface IMatrixOperation
    {
        VariableMatrixId FirstOperandId { get; }

        VariableMatrixId SecondOperandId { get; }

        VariableMatrixId ResultId { get; }

        void PerformOperation(ref Matrix x, ref Matrix y, out Matrix result);
    }
}
