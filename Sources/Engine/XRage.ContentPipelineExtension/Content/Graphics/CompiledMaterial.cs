using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace AISTek.XRage.Content.Graphics
{
    /// <summary>
    /// Content pipeline container for materials being compiled 
    /// into content pipeline data files for efficient run-time loading.
    /// </summary>
    public class CompiledMaterial
    {
        public CompiledMaterial()
        {
            Constants = new List<ConstantParameter>();
            Variables = new List<VariableParameter>();
        }

        public string Effect { get; set; }

        public List<ConstantParameter> Constants { get; set; }

        public List<VariableParameter> Variables { get; set; }

        public ExternalReference<CompiledEffectContent> CompiledEffect { get; set; }

        /// <summary>
        /// Writes the material to a content pipeline data file.
        /// </summary>
        /// <param name="scene"></param>
        public void Write(ContentWriter output)
        {
            // Write a reference to the associated compiled effect file
            output.WriteExternalReference<CompiledEffectContent>(CompiledEffect);

            // Write shader constants
            output.Write(Constants.Count);

            foreach (var constant in Constants)
            {
                output.Write(constant.Semantic);
                output.Write(constant.NumValues);

                foreach (var value in constant.Values)
                {
                    output.Write(value);
                }
            }

            // Write shader variables
            output.Write(Variables.Count);

            foreach (var variable in Variables)
            {
                output.Write(variable.Semantic);
                output.Write(string.Format("{0}:{1}", variable.Type.ToString(), variable.Variable));
            }
        }
    }
}
