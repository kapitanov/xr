using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// The X-Rae material class.  Materials be loaded through the content pipeline or directly at run-time from the .xm files.
    /// </summary>
    public class Material : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of a material.  Only callable from internal library code.
        /// </summary>
        internal Material()
        {
            CurrentTechnique = string.Empty;
            textureBindings = new List<TextureBinding>();
            constantFloatBindings = new List<ConstantFloatBinding>();
            constantFloat2Bindings = new List<ConstantFloat2Binding>();
            constantFloat3Bindings = new List<ConstantFloat3Binding>();
            constantFloat4Bindings = new List<ConstantFloat4Binding>();
            constantMatrixBindings = new List<ConstantMatrixBinding>();
            variableFloatBindings = new List<VariableFloatBinding>();
            variableFloat2Bindings = new List<VariableFloat2Binding>();
            variableFloat3Bindings = new List<VariableFloat3Binding>();
            variableFloat4Bindings = new List<VariableFloat4Binding>();
            variableMatrixBindings = new List<VariableMatrixBinding>();
            variableTexture2DBindings = new List<VariableTexture2DBinding>();
        }

        #endregion

        #region Public properties

        public string CurrentTechnique { get; set; }

        public Effect Effect { get; internal set; } 

        #endregion

        #region Public methods

        /// <summary>
        /// Binds the material to the graphics device for rendering.
        /// </summary>
        /// <param name="graphics">
        /// The game's GraphicsSystem instance.
        /// </param>
        /// <returns>
        /// The number of passes required for this material.
        /// </returns>
        public int BindMaterial(GraphicsSystem graphics)
        {
            BindConstants();

            BindVariables(graphics);

            if (CurrentTechnique.Length > 0)
            {
                Effect.CurrentTechnique = Effect.Techniques[CurrentTechnique];
            }

            return Effect.CurrentTechnique.Passes.Count;
        }

        /// <summary>
        /// Unbinds the material from the graphics device.
        /// </summary>
        public void UnbindMaterial()
        { }

        /// <summary>
        /// Sets up shaders for specified rendering pass.
        /// </summary>
        /// <param name="i">
        /// The material rendering pass.
        /// </param>
        public void BeginPass(int i)
        {
            currPass = Effect.CurrentTechnique.Passes[i];
            currPass.Apply();
        }

        /// <summary>
        /// Finalizes rendering for the current pass.
        /// </summary>
        public void EndPass()
        { }

        public void Dispose()
        {
           // Effect.Dispose();
        }

        #endregion

        #region Private methods

        private void BindVariables(GraphicsSystem graphics)
        {
            for (var i = 0; i < variableFloatBindings.Count; ++i)
            {
                variableFloatBindings[i].parameter
                    .SetValue(graphics.Variables.Get(variableFloatBindings[i].varId));
            }

            for (int i = 0; i < variableFloat2Bindings.Count; ++i)
            {
                variableFloat2Bindings[i].parameter
                    .SetValue(graphics.Variables.Get(variableFloat2Bindings[i].varId));
            }

            for (int i = 0; i < variableFloat3Bindings.Count; ++i)
            {
                variableFloat3Bindings[i].parameter
                    .SetValue(graphics.Variables.Get(variableFloat3Bindings[i].varId));
            }

            for (int i = 0; i < variableFloat4Bindings.Count; ++i)
            {
                variableFloat4Bindings[i].parameter
                    .SetValue(graphics.Variables.Get(variableFloat4Bindings[i].varId));
            }

            // Bind render matrices
            for (int i = 0; i < variableMatrixBindings.Count; ++i)
            {
                variableMatrixBindings[i].parameter
                    .SetValue(graphics.Variables.Get(variableMatrixBindings[i].matrixId));
            }

            for (int i = 0; i < variableTexture2DBindings.Count; ++i)
            {
                variableTexture2DBindings[i].parameter
                    .SetValue(graphics.Variables.Get(variableTexture2DBindings[i].textureId));
            }
        }

        private void BindConstants()
        {
            // Bind textures
            foreach (var binding in textureBindings)
            {
                binding.parameter.SetValue(binding.texture);
            }

            for (var i = 0; i < textureBindings.Count; ++i)
            {
                textureBindings[i].parameter.SetValue(textureBindings[i].texture);
            }

            // Bind constants
            for (var i = 0; i < constantFloatBindings.Count; ++i)
            {
                constantFloatBindings[i].parameter
                    .SetValue(constantFloatBindings[i].constant);
            }

            // Bind constants
            for (var i = 0; i < constantFloat2Bindings.Count; ++i)
            {
                constantFloat2Bindings[i].parameter
                    .SetValue(constantFloat2Bindings[i].constant);
            }

            // Bind constants
            for (var i = 0; i < constantFloat3Bindings.Count; ++i)
            {
                constantFloat3Bindings[i].parameter
                    .SetValue(constantFloat3Bindings[i].constant);
            }

            for (var i = 0; i < constantFloat4Bindings.Count; ++i)
            {
                constantFloat4Bindings[i].parameter
                    .SetValue(constantFloat4Bindings[i].constant);
            }

            for (var i = 0; i < constantMatrixBindings.Count; ++i)
            {
                constantMatrixBindings[i].parameter
                    .SetValue(constantMatrixBindings[i].constant);
            }
        }

        #endregion

        #region Internal fields

        internal List<TextureBinding> textureBindings;
        internal List<ConstantFloatBinding> constantFloatBindings;
        internal List<ConstantFloat2Binding> constantFloat2Bindings;
        internal List<ConstantFloat3Binding> constantFloat3Bindings;
        internal List<ConstantFloat4Binding> constantFloat4Bindings;
        internal List<ConstantMatrixBinding> constantMatrixBindings;
        internal List<VariableFloatBinding> variableFloatBindings;
        internal List<VariableFloat2Binding> variableFloat2Bindings;
        internal List<VariableFloat3Binding> variableFloat3Bindings;
        internal List<VariableFloat4Binding> variableFloat4Bindings;
        internal List<VariableMatrixBinding> variableMatrixBindings;
        internal List<VariableTexture2DBinding> variableTexture2DBindings;

        private EffectPass currPass;

        #endregion
    }
}
