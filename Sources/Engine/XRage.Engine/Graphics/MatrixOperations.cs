using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISTek.XRage.Graphics
{
    public static class MatrixOperations
    {
        static MatrixOperations()
        {
            WorldView = new MatrixOperationWorldView();
            ViewProjection = new MatrixOperationViewProjection();
            WorldViewProjection = new MatrixOperationWorldViewProjection();
        }

        public static void Perform(VariableManager variables)
        {
            variables.Operation(WorldView);
            variables.Operation(ViewProjection);
            variables.Operation(WorldViewProjection);
        }

        public static IMatrixOperation WorldView { get; private set; }

        public static IMatrixOperation WorldViewProjection { get; private set; }

        public static IMatrixOperation ViewProjection { get; private set; }
    }
}
