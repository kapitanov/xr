using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    public class VariableManager : XComponent
    {
        public VariableManager(XGame game)
            : base(game)
        {
            InitializeVariables();
        }

        #region Public methods

        #region Variable getters

        /// <summary>
        /// Retrieves the current value of the specified queriable variable texture2D.
        /// </summary>
        /// <param name="textureId">
        /// The enumerated ID of the variable texture2D to retrieve.
        /// </param>
        public Texture2D Get(VariableTexture2d textureId)
        {
            return variableTexture2D[(int)textureId];
        }

        /// <summary>
        /// Retrieves the current value of the specified queriable variable matrix.
        /// </summary>
        /// <param name="matId">
        /// The enumerated ID of the variable matrix to retrieve.
        /// </param>
        public Matrix Get(VariableMatrixId matId)
        {
            return variableMatrix[(int)matId];
        }

        /// <summary>
        /// Retrieves the current value of the specified queriable variable float.
        /// </summary>
        /// <param name="varID">The enumerated ID of the variable float to retrieve.</param>
        public float Get(VariableFloatId varId)
        {
            return variableFloat[(int)varId];
        }

        /// <summary>
        /// Retrieves the current value of the specified queriable variable float2.
        /// </summary>
        /// <param name="varId">The enuemrated ID of the variable float2 to retrieve.</param>
        /// <param name="val">The float2 value.</param>
        public Vector2 Get(VariableFloat2Id varId)
        {
            return variableFloat2[(int)varId];
        }

        /// <summary>
        /// Retrieves the current value of the specified queriable variable float3.
        /// </summary>
        /// <param name="varId">The enuemrated ID of the variable float3 to retrieve.</param>
        /// <param name="val">The float3 value.</param>
        public Vector3 Get(VariableFloat3Id varId)
        {
            return variableFloat3[(int)varId];
        }

        /// <summary>
        /// Retrieves the current value of the specified queriable variable float4.
        /// </summary>
        /// <param name="varId">The enuemrated ID of the variable float4 to retrieve.</param>
        /// <param name="val">The float4 value.</param>
        public Vector4 Get(VariableFloat4Id varId)
        {
            return variableFloat4[(int)varId];
        }

        #endregion

        #region Variable setters

        /// <summary>
        /// Sets the value of the specified queriable variable texture2D.
        /// </summary>
        /// <param name="textureId">
        /// Variable's identifier
        /// </param>
        /// <param name="value">
        /// Variable's value
        /// </param>
        public void Set(VariableTexture2d textureId, Texture2D value)
        {
            variableTexture2D[(int)textureId] = value;
        }

        /// <summary>
        /// Sets the value of the specified queriable variable matrix.
        /// </summary>
        /// <param name="textureId">
        /// Variable's identifier
        /// </param>
        /// <param name="value">
        /// Variable's value
        /// </param>
        public void Set(VariableMatrixId matId, Matrix value)
        {
            variableMatrix[(int)matId] = value;
        }

        /// <summary>
        /// Sets the value of the specified queriable variable float.
        /// </summary>
        /// <param name="textureId">
        /// Variable's identifier
        /// </param>
        /// <param name="value">
        /// Variable's value
        /// </param>
        public void Set(VariableFloatId varId, float value)
        {
            variableFloat[(int)varId] = value;
        }

        /// <summary>
        /// Sets the value of the specified queriable variable float2.
        /// </summary>
        /// <param name="textureId">
        /// Variable's identifier
        /// </param>
        /// <param name="value">
        /// Variable's value
        /// </param>
        public void Set(VariableFloat2Id varId, Vector2 value)
        {
            variableFloat2[(int)varId] = value;
        }

        /// <summary>
        /// Sets the value of the specified queriable variable float3.
        /// </summary>
        /// <param name="textureId">
        /// Variable's identifier
        /// </param>
        /// <param name="value">
        /// Variable's value
        /// </param>
        public void Set(VariableFloat3Id varId,  Vector3 value)
        {
            variableFloat3[(int)varId] = value;
        }

        /// <summary>
        /// Sets the value of the specified queriable variable float4.
        /// </summary>
        /// <param name="textureId">
        /// Variable's identifier
        /// </param>
        /// <param name="value">
        /// Variable's value
        /// </param>
        public void Set(VariableFloat4Id varId,  Vector4 value)
        {
            variableFloat4[(int)varId] = value;
        }

        #endregion

        #region Variable operations

        public void Operation(IFloatOperation operation)
        {
            operation.PerformOperation(
                variableFloat[(int)operation.FirstOperandId],
                variableFloat[(int)operation.SecondOperandId],
                out variableFloat[(int)operation.ResultId]); 
        }

        public void Operation(IVector2Operation operation)
        {
            operation.PerformOperation(
                ref variableFloat2[(int)operation.FirstOperandId],
                ref variableFloat2[(int)operation.SecondOperandId],
                out variableFloat2[(int)operation.ResultId]);
        }

        public void Operation(IVector3Operation operation)
        {
            operation.PerformOperation(
                ref variableFloat3[(int)operation.FirstOperandId],
                ref variableFloat3[(int)operation.SecondOperandId],
                out variableFloat3[(int)operation.ResultId]);
        }

        public void Operation(IVector4Operation operation)
        {
            operation.PerformOperation(
                ref variableFloat4[(int)operation.FirstOperandId],
                ref variableFloat4[(int)operation.SecondOperandId],
                out variableFloat4[(int)operation.ResultId]);
        }

        public void Operation(IMatrixOperation operation)
        {
            operation.PerformOperation(
                ref variableMatrix[(int)operation.FirstOperandId],
                ref variableMatrix[(int)operation.SecondOperandId],
                out variableMatrix[(int)operation.ResultId]);
        }

        #endregion

        #endregion

        #region Private methods

        private void InitializeVariables()
        {
            for (var i = 0; i < variableMatrix.Length; ++i)
            {
                variableMatrix[i] = Matrix.Identity;
            }

            for (var i = 0; i < variableFloat.Length; ++i)
            {
                this.variableFloat[i] = 0.0f;
            }

            for (var i = 0; i < variableFloat2.Length; ++i)
            {
                variableFloat2[i] = Vector2.Zero;
            }
 
            for (var i = 0; i < this.variableFloat3.Length; ++i)
            {
                variableFloat3[i] = Vector3.Zero;
            }

            for (var i = 0; i < variableFloat4.Length; ++i)
            {
                variableFloat4[i] = Vector4.Zero;
            }

            for (var i = 0; i < variableTexture2D.Length; ++i)
            {
                variableTexture2D[i] = null;
            }

            for (var i = 0; i < variableBool.Length; ++i)
            {
                variableBool[i] = false;
            }

        }

        #endregion

        #region Private fields

        internal Matrix[] variableMatrix = new Matrix[(int)VariableMatrixId.Identity + 1];
        internal float[] variableFloat = new float[(int)VariableFloatId.Unity + 1];
        internal Vector2[] variableFloat2 = new Vector2[(int)VariableFloat2Id.Unity + 1];
        internal Vector3[] variableFloat3= new Vector3[(int)VariableFloat3Id.Unity + 1];
        internal Vector4[] variableFloat4 = new Vector4[(int)VariableFloat4Id.Unity + 1];
        internal Texture2D[] variableTexture2D = new Texture2D[(int)VariableTexture2d.Unity + 1];
        internal bool[] variableBool = new bool[(int)VariableBoolId.Unity + 1];

        #endregion
    }
}
