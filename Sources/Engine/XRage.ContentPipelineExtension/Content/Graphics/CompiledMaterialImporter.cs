using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using AISTek.XRage.Graphics;
using System.Xml;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Content.Graphics
{
    /// <summary>
    /// Content pipeline importer for X-Rage material files.
    /// </summary>
    [ContentImporter(".xm", DisplayName = "XMaterial Importer")]
    public class CompiledMaterialImporter : ContentImporter<CompiledMaterial>
    {
        /// <summary>
        /// Parses the specified file into a CompiledMaterial instance.
        /// </summary>
        /// <param name="filename">
        /// The material file to import.
        /// </param>
        /// <param name="context">
        /// The current content importer context.
        /// </param>
        /// <returns>
        /// The created CompiledMaterial instance.
        /// </returns>
        public override CompiledMaterial Import(string filename, ContentImporterContext context)
        {
            material = new CompiledMaterial();

            LoadFromFile(filename);

            return material;
        }

        /// <summary>
        /// Parses the specified material file.
        /// </summary>
        /// <param name="name">The material file to parse.</param>
        /// <returns>True if successful, false otherwise.</returns>
        private bool LoadFromFile(string name)
        {
            var reader = new XmlTextReader(name);
            var document = new XmlDocument();
            document.Load(reader);
            reader.Close();


            if (!ParseRootXMLNode(document.DocumentElement))
                return false;

            return true;
        }

        /// <summary>
        /// Parses the root XML node in the material file.
        /// </summary>
        /// <param name="node">The root XML node.</param>
        /// <returns>True is successful, false otherwise.</returns>
        private bool ParseRootXMLNode(XmlNode node)
        {
            switch (node.Name)
            {
                case "material":
                    {
                        XmlNode effectNode = node.Attributes.GetNamedItem("effect");
                        if (effectNode == null)
                            throw new Exception("Error in material file: Root <material> node must have effect attribute.");

                        material.Effect = effectNode.InnerText;

                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (!ParseXMLBodyNode(child))
                                return false;
                        }
                        break;
                    }
                default:
                    {
                        throw new Exception("Error in material file: Root node is not <material>.");
                    }
            }
            return true;
        }

        /// <summary>
        /// Parses a single XML node.
        /// </summary>
        /// <param name="node">The node to parse.</param>
        /// <returns>True if successful, false otherwise.</returns>
        private bool ParseXMLBodyNode(XmlNode node)
        {
            switch (node.Name)
            {
                case "sampler":
                    {
                        // Parse sampler
                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <sampler> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <sampler> node must have semantic usage.");


                        var variable = new VariableParameter
                        {
                            Semantic = semanticNode.InnerText,
                            Variable = node.InnerText,
                            Type = VariableType.Sampler
                        };
                        material.Variables.Add(variable);

                        break;
                    }

                case "variable_bool":
                    {
                        // Parse matrix variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <variable_bool> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <variable_bool> node must have semantic usage.");

                        XmlNode matIDNode = node.Attributes.GetNamedItem("varID");
                        if (matIDNode == null)
                            throw new Exception("Error in material file: <variable_bool> node must have rendervar attribute.");

                        var var = new VariableParameter();
                        var.Semantic = semanticNode.InnerText;
                        var.Variable = matIDNode.InnerText;
                        var.Type = VariableType.Bool;

                        material.Variables.Add(var);

                        break;
                    }

                case "variable_float":
                    {
                        // Parse float variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <variable_float> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <variable_float> node must have semantic usage.");

                        XmlNode floatIDNode = node.Attributes.GetNamedItem("varID");
                        if (floatIDNode == null)
                            throw new Exception("Error in material file: <variable_float> node must have rendervar attribute.");

                        var var = new VariableParameter();
                        var.Semantic = semanticNode.InnerText;
                        var.Variable = floatIDNode.InnerText;
                        var.Type = VariableType.Float;

                        material.Variables.Add(var);

                        break;
                    }

                case "variable_float2":
                    {
                        // Parse float variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <variable_float2> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <variable_float2> node must have semantic usage.");

                        XmlNode floatIDNode = node.Attributes.GetNamedItem("varID");
                        if (floatIDNode == null)
                            throw new Exception("Error in material file: <variable_float2> node must have rendervar attribute.");

                        var var = new VariableParameter();
                        var.Semantic = semanticNode.InnerText;
                        var.Variable = floatIDNode.InnerText;
                        var.Type = VariableType.Float2;

                        material.Variables.Add(var);

                        break;
                    }

                case "variable_float3":
                    {
                        // Parse float variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <variable_float3> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <variable_float3> node must have semantic usage.");

                        XmlNode floatIDNode = node.Attributes.GetNamedItem("varID");
                        if (floatIDNode == null)
                            throw new Exception("Error in material file: <variable_float3> node must have rendervar attribute.");

                        var var = new VariableParameter();
                        var.Semantic = semanticNode.InnerText;
                        var.Variable = floatIDNode.InnerText;
                        var.Type = VariableType.Float3;

                        material.Variables.Add(var);

                        break;
                    }

                case "variable_float4":
                    {
                        // Parse float variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <variable_float4> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <variable_float4> node must have semantic usage.");

                        XmlNode floatIDNode = node.Attributes.GetNamedItem("varID");
                        if (floatIDNode == null)
                            throw new Exception("Error in material file: <variable_float4> node must have rendervar attribute.");

                        var var = new VariableParameter();
                        var.Semantic = semanticNode.InnerText;
                        var.Variable = floatIDNode.InnerText;
                        var.Type = VariableType.Float4;

                        material.Variables.Add(var);

                        break;
                    }

                case "variable_matrix":
                    {
                        // Parse matrix variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <variable_matrix> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <variable_matrix> node must have semantic usage.");

                        XmlNode matIDNode = node.Attributes.GetNamedItem("varID");
                        if (matIDNode == null)
                            throw new Exception("Error in material file: <variable_matrix> node must have varID attribute.");

                        var var = new VariableParameter();
                        var.Semantic = semanticNode.InnerText;
                        var.Variable = matIDNode.InnerText;
                        var.Type = VariableType.Matrix;

                        material.Variables.Add(var);

                        break;
                    }

                case "variable_texture2D":
                    {
                        // Parse texture2D variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <variable_texture2D> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <variable_texture2D> node must have semantic usage.");

                        XmlNode textureIDNode = node.Attributes.GetNamedItem("varID");
                        if (textureIDNode == null)
                            throw new Exception("Error in material file: <variable_texture2D> node must have varID attribute.");

                        var var = new VariableParameter();
                        var.Semantic = semanticNode.InnerText;
                        var.Variable = textureIDNode.InnerText;
                        var.Type = VariableType.Texture2D;

                        material.Variables.Add(var);

                        break;
                    }

                case "constant_float":
                    {
                        // Parse constant variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <constant_float1> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <constant_float1> node must have semantic usage.");

                        float value;
                        if (!float.TryParse(
                            node.InnerText,
                            NumberStyles.Float |
                            NumberStyles.AllowDecimalPoint |
                            NumberStyles.AllowThousands |
                            NumberStyles.AllowExponent,
                            NumberFormatInfo.InvariantInfo,
                            out value))
                            throw new Exception("Error in material file: <constant_float1> node only supports floating-point constants.");

                        var cons = new ConstantParameter();
                        cons.Semantic = semanticNode.InnerText;
                        cons.NumValues = 1;
                        cons.Values = new float[cons.NumValues];
                        cons.Values[0] = value;

                        material.Constants.Add(cons);

                        break;
                    }

                case "constant_float4":
                    {
                        // Parse constant variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <constant_float4> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <constant_float4> node must have semantic usage.");

                        Vector4 value;

                        string[] vars = node.InnerText.Split(',');
                        if (vars.Length != 4)
                            throw new Exception("Error in material file: <constant_float4> node must specify 4 floats in the form: f1, f2, f3, f4");

                        if (!float.TryParse(vars[0], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.X) ||
                            !float.TryParse(vars[1], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.Y) ||
                            !float.TryParse(vars[2], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.Z) || 
                            !float.TryParse(vars[3], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.W))
                            throw new Exception("Error in material file: <constant_float4> node only supports floating-point constants.");

                        var cons = new ConstantParameter();
                        cons.Semantic = semanticNode.InnerText;
                        cons.NumValues = 4;
                        cons.Values = new float[cons.NumValues];
                        cons.Values[0] = value.X;
                        cons.Values[1] = value.Y;
                        cons.Values[2] = value.Z;
                        cons.Values[3] = value.W;

                        material.Constants.Add(cons);

                        break;
                    }

                case "constant_matrix":
                    {
                        // Parse constant variable

                        if (node.ChildNodes.Count > 1)
                            throw new Exception("Error in material file: <constant_matrix> node cannot have Children nodes.");

                        XmlNode semanticNode = node.Attributes.GetNamedItem("semantic");
                        if (semanticNode == null)
                            throw new Exception("Error in material file: <constant_matrix> node must have semantic usage.");

                        Matrix value;

                        string[] vars = node.InnerText.Split(',');
                        if (vars.Length != 16)
                            throw new Exception("Error in material file: <constant_matrix> node must specify 4 floats in the form: f1, f2, f3, f4, ..., f16");

                        if (!float.TryParse(vars[0], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M11) || !float.TryParse(vars[1], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M12) || !float.TryParse(vars[2], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M13) || !float.TryParse(vars[3], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M14) ||
                           !float.TryParse(vars[4], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M21) || !float.TryParse(vars[5], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M22) || !float.TryParse(vars[6], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M23) || !float.TryParse(vars[7], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M24) ||
                           !float.TryParse(vars[8], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M31) || !float.TryParse(vars[9], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M32) || !float.TryParse(vars[10], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M33) || !float.TryParse(vars[11], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M34) ||
                           !float.TryParse(vars[12], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M41) || !float.TryParse(vars[13], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M42) || !float.TryParse(vars[14], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M43) || !float.TryParse(vars[15], NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out value.M44))
                            throw new Exception("Error in material file: <constant_matrix> node only supports floating-point constants.");


                        var cons = new ConstantParameter();
                        cons.Semantic = semanticNode.InnerText;
                        cons.NumValues = 16;
                        cons.Values = new float[cons.NumValues];
                        cons.Values[0] = value.M11;
                        cons.Values[1] = value.M12;
                        cons.Values[2] = value.M13;
                        cons.Values[3] = value.M14;

                        cons.Values[4] = value.M21;
                        cons.Values[5] = value.M22;
                        cons.Values[6] = value.M23;
                        cons.Values[7] = value.M24;

                        cons.Values[8] = value.M31;
                        cons.Values[9] = value.M32;
                        cons.Values[10] = value.M33;
                        cons.Values[11] = value.M34;

                        cons.Values[12] = value.M41;
                        cons.Values[13] = value.M42;
                        cons.Values[14] = value.M43;
                        cons.Values[15] = value.M44;

                        material.Constants.Add(cons);

                        break;
                    }


                default:
                    {
                        // Ignore unknown node
                        // TODO: Warning message
                        break;
                    }
            }
            return true;
        }

        private CompiledMaterial material;
    }

}
