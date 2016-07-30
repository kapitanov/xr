using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    /// <summary>
    /// Content type reader for materials.
    /// </summary>
    public class MaterialReader : ContentTypeReader<Material>
    {
        /// <summary>
        /// Creates a <see cref="Material"/> instance from an XNB data file.
        /// </summary>
        /// <param name="action">
        /// <see cref="ContentReader"/> instance for the data file.
        /// </param>
        /// <param name="existingInstance">
        /// Reference to an existing <see cref="Material"/> instance.
        /// </param>
        /// <returns>
        /// The <see cref="Material"/> instance read from the data file.
        /// </returns>
        protected override Material Read(ContentReader input, Material existingInstance)
        {
            var material = new Material();
            LoadFromContent(material, input);

            return material;
        }

        /// <summary>
        /// Loads a compiled material file from a content pipeline data file.
        /// </summary>
        /// <param name="reader">
        /// The ContentReader instance for the data file.
        /// </param>
        private void LoadFromContent(Material material, ContentReader reader)
        {
            // Read referenced effect
            material.Effect = reader.ReadExternalReference<Effect>();

            var numConstants = reader.ReadInt32();
            for (var i = 0; i < numConstants; i++)
            {
                var semantic = reader.ReadString();
                var numValues = reader.ReadInt32();

                if (numValues == 1)
                {
                    var value = reader.ReadSingle();

                    var binding = new ConstantFloatBinding
                    {
                        constant = value,
                        parameter = material.Effect.Parameters.GetParameterBySemantic(semantic)
                    };

                    if (binding.parameter == null)
                    {
                        throw new Exception("Error in material: Undefined semantic.");
                    }

                    material.constantFloatBindings.Add(binding);
                }
                else if (numValues == 2)
                {
                    var value = new Vector2(reader.ReadSingle(), reader.ReadSingle());

                    var binding = new ConstantFloat2Binding
                    {
                        constant = value,
                        parameter = material.Effect.Parameters.GetParameterBySemantic(semantic)
                    };

                    if (binding.parameter == null)
                    {
                        throw new Exception("Error in material: Undefined semantic.");
                    }

                    material.constantFloat2Bindings.Add(binding);
                }
                else if (numValues == 3)
                {
                    var value = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    var binding = new ConstantFloat3Binding
                    {
                        constant = value,
                        parameter = material.Effect.Parameters.GetParameterBySemantic(semantic)
                    };

                    if (binding.parameter == null)
                    {
                        throw new Exception("Error in material: Undefined semantic.");
                    }

                    material.constantFloat3Bindings.Add(binding);
                }
                else if (numValues == 4)
                {
                    var value = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    var binding = new ConstantFloat4Binding
                    {
                        constant = value,
                        parameter = material.Effect.Parameters.GetParameterBySemantic(semantic)
                    };

                    if (binding.parameter == null)
                    {
                        throw new Exception("Error in material: Undefined semantic.");
                    }

                   material.constantFloat4Bindings.Add(binding);
                }
                else if (numValues == 16)
                {
                    var val = new Matrix();
                    val.M11 = reader.ReadSingle();
                    val.M12 = reader.ReadSingle();
                    val.M13 = reader.ReadSingle();
                    val.M14 = reader.ReadSingle();

                    val.M21 = reader.ReadSingle();
                    val.M22 = reader.ReadSingle();
                    val.M23 = reader.ReadSingle();
                    val.M24 = reader.ReadSingle();

                    val.M31 = reader.ReadSingle();
                    val.M32 = reader.ReadSingle();
                    val.M33 = reader.ReadSingle();
                    val.M34 = reader.ReadSingle();

                    val.M41 = reader.ReadSingle();
                    val.M42 = reader.ReadSingle();
                    val.M43 = reader.ReadSingle();
                    val.M44 = reader.ReadSingle();

                    var binding = new ConstantMatrixBinding
                    {
                        constant = val,
                        parameter = material.Effect.Parameters.GetParameterBySemantic(semantic)
                    };

                    if (binding.parameter == null)
                    {
                        throw new Exception("Error in material: Undefined semantic.");
                    }

                    material.constantMatrixBindings.Add(binding);
                }
                else
                    throw new Exception("Error loading material: Invalid constant size.");
            }

            int numVariables = reader.ReadInt32();

            for (int i = 0; i < numVariables; i++)
            {
                var semantic = reader.ReadString();
                var variable = reader.ReadString();

                int idx = variable.IndexOf(':');
                var type = variable.Substring(0, idx);
                var varName = variable.Substring(idx + 1);

                switch (type)
                {
                    case "Sampler":
                        {
                            var binding = new TextureBinding();
                            binding.parameter = material.Effect.Parameters.GetParameterBySemantic(semantic);
                            if (binding.parameter == null)
                            {
                                throw new Exception("Error in material: Undefined semantic.");
                            }

                            binding.texture = reader.ContentManager.Load<Texture>(string.Format("textures/{0}", varName));
                            material.textureBindings.Add(binding);
                            break;
                        }

                    case "Texture2D":
                        {
                            var binding = new VariableTexture2DBinding();
                            binding.parameter = material.Effect.Parameters.GetParameterBySemantic(semantic);
                            if (binding.parameter == null)
                            {
                                throw new Exception("Error in material: Undefined semantic.");
                            }


                            try
                            {
                                binding.textureId = (VariableTexture2d)Enum.Parse(typeof(VariableTexture2d), varName, true);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Error in material file: <variable_texture2D> node contained unknown rendervar.");
                            }

                            material.variableTexture2DBindings.Add(binding);

                            break;
                        }

                    case "Matrix":
                        {
                            VariableMatrixBinding binding = new VariableMatrixBinding();

                            binding.parameter = material.Effect.Parameters.GetParameterBySemantic(semantic);
                            if (binding.parameter == null)
                            {
                                throw new Exception("Error in material: Undefined semantic.");
                            }


                            try
                            {
                                binding.matrixId = (VariableMatrixId)Enum.Parse(typeof(VariableMatrixId), varName, true);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Error in material file: <variable_matrix> node contained unknown rendervar.");
                            }

                            material.variableMatrixBindings.Add(binding);

                            break;
                        }

                    case "Float4":
                        {
                            var binding = new VariableFloat4Binding();

                            binding.parameter = material.Effect.Parameters.GetParameterBySemantic(semantic);
                            if (binding.parameter == null)
                            {
                                throw new Exception("Error in material: Undefined semantic.");
                            }


                            try
                            {
                                binding.varId = (VariableFloat4Id)Enum.Parse(typeof(VariableFloat4Id), varName, true);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Error in material file: <variable_float4> node contained unknown rendervar.");
                            }

                            material.variableFloat4Bindings.Add(binding);
                            break;
                        }

                    case "Float3":
                        {
                            var binding = new VariableFloat3Binding();

                            binding.parameter = material.Effect.Parameters.GetParameterBySemantic(semantic);
                            if (binding.parameter == null)
                            {
                                throw new Exception("Error in material: Undefined semantic.");
                            }

                            try
                            {
                                binding.varId = (VariableFloat3Id)Enum.Parse(typeof(VariableFloat3Id), varName, true);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Error in material file: <variable_float3> node contained unknown rendervar.");
                            }

                            material.variableFloat3Bindings.Add(binding);
                            break;
                        }

                    case "Float2":
                        {
                            var binding = new VariableFloat2Binding();

                            binding.parameter = material.Effect.Parameters.GetParameterBySemantic(semantic);
                            if (binding.parameter == null)
                            {
                                throw new Exception("Error in material: Undefined semantic.");
                            }


                            try
                            {
                                binding.varId = (VariableFloat2Id)Enum.Parse(typeof(VariableFloat2Id), varName, true);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Error in material file: <variable_float2> node contained unknown rendervar.");
                            }

                            material.variableFloat2Bindings.Add(binding);
                            break;
                        }

                    case "Float":
                        {
                            var binding = new VariableFloatBinding();

                            binding.parameter = material.Effect.Parameters.GetParameterBySemantic(semantic);
                            if (binding.parameter == null)
                            {
                                throw new Exception("Error in material: Undefined semantic.");
                            }


                            try
                            {
                                binding.varId = (VariableFloatId)Enum.Parse(typeof(VariableFloatId), varName, true);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Error in material file: <variable_float> node contained unknown rendervar.");
                            }

                            material.variableFloatBindings.Add(binding);
                            break;
                        }
                }
            }
        }
    }
}
