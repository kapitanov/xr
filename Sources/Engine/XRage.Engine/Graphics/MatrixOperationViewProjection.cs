using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    internal class MatrixOperationViewProjection : IMatrixOperation
    {
        public VariableMatrixId FirstOperandId { get { return VariableMatrixId.View; } }

        public VariableMatrixId SecondOperandId { get { return VariableMatrixId.Projection; } }

        public VariableMatrixId ResultId { get { return VariableMatrixId.ViewProjection; } }

        public void PerformOperation(ref Matrix x, ref Matrix y, out Matrix result)
        {
            result = x * y;
        }
    }
}
