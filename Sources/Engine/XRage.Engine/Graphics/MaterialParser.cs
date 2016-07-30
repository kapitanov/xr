using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace AISTek.XRage.Graphics
{
	public class MaterialParser
	{
		public MaterialParser()
		{
			Material = new Material();
		}

		public Material Material { get; private set; }

		/// <summary>
		/// Loads a non-compiled material directly from disk.
		/// </summary>
		/// <param name="name">
		/// The material file to load.
		/// </param>
		/// <param name="content">
		/// The ContentManager instance to use for asset loading.
		/// </param>
		/// <returns>
		/// True if successful, false otherwise.
		/// </returns>
		public bool LoadFromFile(string name, ContentManager content)
		{
			var reader = new XmlTextReader(name);
			var document = new XmlDocument();
			document.Load(reader);
			reader.Close();

			if (!ParseRootXmlNode(document.DocumentElement, content))
			{
				return false;
			}

			if (Material.Effect == null)
			{
				throw new Exception("Error in XMaterial file: Every material must have a shader.");
			}

			return true;
		}

		#region Private methods

		/// <summary>
		/// Parses the root XML node of the material file, then recurses for Children.
		/// </summary>
		/// <param name="node">
		/// The root XML node.
		/// </param>
		/// <param name="content">
		/// The ContentManager instance to use for asset loading.
		/// </param>
		/// <returns>
		/// True if successful, false otherwise.
		/// </returns>
		private bool ParseRootXmlNode(XmlNode node, ContentManager content)
		{
			if (node.Name == MaterialParserConstants.XMaterialRoot)
			{
				var effectNode = node.Attributes.GetNamedItem("shader");
				if (effectNode == null)
				{
					throw new Exception("Error in XMaterial file: Root <material> node must have \"shader\" attribute.");
				}

				var name = effectNode.InnerText;

				Material.Effect = content.Load<Effect>(string.Format("shaders/{0}", name));
				if (Material.Effect == null)
				{
					throw new Exception("Error in XMaterial file: failed to load shader.");
				}

				foreach (XmlNode child in node.ChildNodes)
				{
					if (!ParseXmlBodyNode(child, content))
					{
						return false;
					}

					throw new Exception("Error in material file: Root node is not <material>.");
				}
			}

			return true;
		}

		/// <summary>
		/// Parses an XML node in the material file.
		/// </summary>
		/// <param name="node">The XML node to parse.</param>
		/// <param name="content">The ContentManager instance to use for asset loading.</param>
		/// <returns>True if successful, false otherwise.</returns>
		private bool ParseXmlBodyNode(XmlNode node, ContentManager content)
		{
			switch (node.Name)
			{
				case "sampler":
					ParseSampler(node, content);
					break;

				case "variable_float":
					ParseFloatVariable(node);
					break;

				case "variable_float2":
					ParseFloat2Variable(node);
					break;

				case "variable_float3":
					ParseFloat3Variable(node);
					break;

				case "variable_float4":
					ParseFloat4Variable(node);
					break;

				case "variable_matrix":
					ParseMatrixVariable(node);
					break;

				case "variable_texture2D":
					ParseTextureVariable(node);
					break;

				case "constant_float":
					ParseFloatConstant(node);
					break;

				case "constant_float2":
					ParseFloat2Constant(node);
					break;

				case "constant_float3":
					ParseFloat3Constant(node);
					break;

				case "constant_float4":
					ParseFloat4Constant(node);
					break;

				case "constant_matrix":
					ParseMatrixConstant(node);
					break;

				default:
					// Ignore unknown node
					// TODO: Warning message
					break;
			}

			return true;
		}

		#region Parsing constants

		private void ParseMatrixConstant(XmlNode node)
		{
			// Parse constant variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <constant_matrix> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <constant_matrix> node must have semantic usage.");
			}

			Matrix value;

			var vars = node.InnerText.Split(',');
			if (vars.Length != 16)
			{
				throw new Exception("Error in XMaterial file: <constant_matrix> node must specify 4 floats in the form: f1, f2, f3, f4, ..., f16");
			}

#if XBOX360
			value = Matrix.Identity;
#else
			var numberStyle = NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
			if (!float.TryParse(vars[0], numberStyle, NumberFormatInfo.InvariantInfo, out value.M11) ||
				!float.TryParse(vars[1], numberStyle, NumberFormatInfo.InvariantInfo, out value.M12) ||
				!float.TryParse(vars[2], numberStyle, NumberFormatInfo.InvariantInfo, out value.M13) ||
				!float.TryParse(vars[3], numberStyle, NumberFormatInfo.InvariantInfo, out value.M14) ||

			   !float.TryParse(vars[4], numberStyle, NumberFormatInfo.InvariantInfo, out value.M21) ||
			   !float.TryParse(vars[5], numberStyle, NumberFormatInfo.InvariantInfo, out value.M22) ||
			   !float.TryParse(vars[6], numberStyle, NumberFormatInfo.InvariantInfo, out value.M23) ||
			   !float.TryParse(vars[7], numberStyle, NumberFormatInfo.InvariantInfo, out value.M24) ||

			   !float.TryParse(vars[8], numberStyle, NumberFormatInfo.InvariantInfo, out value.M31) ||
			   !float.TryParse(vars[9], numberStyle, NumberFormatInfo.InvariantInfo, out value.M32) ||
			   !float.TryParse(vars[10], numberStyle, NumberFormatInfo.InvariantInfo, out value.M33) ||
			   !float.TryParse(vars[11], numberStyle, NumberFormatInfo.InvariantInfo, out value.M34) ||

			   !float.TryParse(vars[12], numberStyle, NumberFormatInfo.InvariantInfo, out value.M41) ||
			   !float.TryParse(vars[13], numberStyle, NumberFormatInfo.InvariantInfo, out value.M42) ||
			   !float.TryParse(vars[14], numberStyle, NumberFormatInfo.InvariantInfo, out value.M43) ||
			   !float.TryParse(vars[15], numberStyle, NumberFormatInfo.InvariantInfo, out value.M44))
			{
				throw new Exception("Error in material file: <constant_matrix> node only supports floating-point constants.");
			}
#endif

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);
			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: effect does not contain semantic for <constant_matrix> node.");
			}

			var binding = new ConstantMatrixBinding { constant = value, parameter = parameter };

			Material.constantMatrixBindings.Add(binding);
		}

		private void ParseFloat4Constant(XmlNode node)
		{
			// Parse constant variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <constant_float4> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <constant_float4> node must have semantic usage.");
			}

			Vector4 value;

			var vars = node.InnerText.Split(',');
			if (vars.Length != 4)
			{
				throw new Exception("Error in XMaterial file: <constant_float4> node must specify 4 floats in the form: f1, f2, f3, f4");
			}

#if XBOX360
			value = Vector4.One;
#else
			var numberStyle = NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
			if (!float.TryParse(vars[0], numberStyle, NumberFormatInfo.InvariantInfo, out value.X) ||
				!float.TryParse(vars[1], numberStyle, NumberFormatInfo.InvariantInfo, out value.Y) ||
				!float.TryParse(vars[2], numberStyle, NumberFormatInfo.InvariantInfo, out value.Z) ||
				!float.TryParse(vars[3], numberStyle, NumberFormatInfo.InvariantInfo, out value.W))
			{
				throw new Exception("Error in XMaterial file: <constant_float4> node only supports floating-point constants.");
			}
#endif

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);
			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: effect does not contain semantic for <constant_float4> node.");
			}

			var binding = new ConstantFloat4Binding { constant = value, parameter = parameter };

			Material.constantFloat4Bindings.Add(binding);
		}

		private void ParseFloat3Constant(XmlNode node)
		{
			// Parse constant variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <constant_float3> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <constant_float3> node must have semantic usage.");
			}

			Vector3 value;

			var vars = node.InnerText.Split(',');
			if (vars.Length != 3)
			{
				throw new Exception("Error in XMaterial file: <constant_float3> node must specify 3 floats in the form: f1, f2, f3");
			}

#if XBOX360
			value = Vector3.One;
#else
			var numberStyle = NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
			if (!float.TryParse(vars[0], numberStyle, NumberFormatInfo.InvariantInfo, out value.X) ||
				!float.TryParse(vars[1], numberStyle, NumberFormatInfo.InvariantInfo, out value.Y) ||
				!float.TryParse(vars[2], numberStyle, NumberFormatInfo.InvariantInfo, out value.Z))
			{
				throw new Exception("Error in XMaterial file: <constant_float3> node only supports floating-point constants.");
			}
#endif

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);
			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: effect does not contain semantic for <constant_float3> node.");
			}

			var binding = new ConstantFloat3Binding { constant = value, parameter = parameter };

			Material.constantFloat3Bindings.Add(binding);
		}

		private void ParseFloat2Constant(XmlNode node)
		{
			// Parse constant variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <constant_float2> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <constant_float2> node must have semantic usage.");
			}

			Vector2 value;

			var vars = node.InnerText.Split(',');
			if (vars.Length != 2)
			{
				throw new Exception("Error in XMaterial file: <constant_float2> node must specify 2 floats in the form: f1, f2");
			}

#if XBOX360
			value = Vector2.One;
#else
			var numberStyle = NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
			if (!float.TryParse(vars[0], numberStyle, NumberFormatInfo.InvariantInfo, out value.X) ||
				!float.TryParse(vars[1], numberStyle, NumberFormatInfo.InvariantInfo, out value.Y))
			{
				throw new Exception("Error in XMaterial file: <constant_float2> node only supports floating-point constants.");
			}
#endif

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);
			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: effect does not contain semantic for <constant_float2> node.");
			}

			var binding = new ConstantFloat2Binding { constant = value, parameter = parameter };

			Material.constantFloat2Bindings.Add(binding);
		}

		private void ParseFloatConstant(XmlNode node)
		{
			// Parse constant variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <constant_float1> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <constant_float1> node must have semantic usage.");
			}

#if XBOX360
			float value = 1.0f;
#else
			float value;
			if (!float.TryParse(
				node.InnerText,
				NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent,
				NumberFormatInfo.InvariantInfo,
				out value))
			{
				throw new Exception("Error in XMaterial file: <constant_float1> node only supports floating-point constants.");
			}
#endif

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);
			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: effect does not contain semantic for <constant_float1> node.");
			}

			var binding = new ConstantFloatBinding { constant = value, parameter = parameter };

			Material.constantFloatBindings.Add(binding);
		}

		#endregion

		#region Parsing variables

		private void ParseTextureVariable(XmlNode node)
		{
			// Parse texture variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <variable_texture2D> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_texture2D> node must have semantic usage.");
			}

			var textureIdNode = node.Attributes.GetNamedItem("varID");
			if (textureIdNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_texture2D> node must have varID attribute.");
			}

			var param = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);

			if (param == null)
			{
				throw new Exception("Error in XMaterial file: semantic not found in effect.");
			}

			VariableTexture2d textureId;

			try
			{
				textureId = (VariableTexture2d)Enum.Parse(typeof(VariableTexture2d), textureIdNode.InnerText, true);
			}
			catch (Exception)
			{
				throw new Exception("Error in XMaterial file: <variable_texture2D> node contained unknown rendervar.");
			}

			var binding = new VariableTexture2DBinding { textureId = textureId, parameter = param };

			Material.variableTexture2DBindings.Add(binding);
		}

		private void ParseMatrixVariable(XmlNode node)
		{
			// Parse matrix variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in Xmaterial file: <variable_matrix> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_matrix> node must have semantic usage.");
			}

			var matIdNode = node.Attributes.GetNamedItem("varID");
			if (matIdNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_matrix> node must have varID attribute.");
			}

			EffectParameter param = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);

			if (param == null)
			{
				throw new Exception("Error in XMaterial file: semantic not found in effect.");
			}

			VariableMatrixId matId;

			try
			{
				matId = (VariableMatrixId)Enum.Parse(typeof(VariableMatrixId), matIdNode.InnerText, true);
			}
			catch (Exception)
			{
				throw new Exception("Error in XMaterial file: <variable_matrix> node contained unknown rendervar.");
			}

			var binding = new VariableMatrixBinding { matrixId = matId, parameter = param };

			Material.variableMatrixBindings.Add(binding);
		}

		private void ParseFloat4Variable(XmlNode node)
		{
			// Parse float variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <variable_float4> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float4> node must have semantic usage.");
			}

			var floatIdNode = node.Attributes.GetNamedItem("varID");
			if (floatIdNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float4> node must have rendervar attribute.");
			}

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);

			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: semantic not found in effect.");
			}

			VariableFloat4Id floatId;

			try
			{
				floatId = (VariableFloat4Id)Enum.Parse(typeof(VariableFloat4Id), floatIdNode.InnerText, true);
			}
			catch (Exception)
			{
				throw new Exception("Error in XMaterial file: <variable_float4> node contained unknown rendervar.");
			}

			var binding = new VariableFloat4Binding { varId = floatId, parameter = parameter };

			Material.variableFloat4Bindings.Add(binding);
		}

		private void ParseFloat3Variable(XmlNode node)
		{
			// Parse float variable

			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <variable_float3> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float3> node must have semantic usage.");
			}

			var floatIdNode = node.Attributes.GetNamedItem("varID");
			if (floatIdNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float3> node must have rendervar attribute.");
			}

			var param = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);

			if (param == null)
			{
				throw new Exception("Error in XMaterial file: semantic not found in effect.");
			}

			VariableFloat3Id floatId;

			try
			{
				floatId = (VariableFloat3Id)Enum.Parse(typeof(VariableFloat3Id), floatIdNode.InnerText, true);
			}
			catch (Exception)
			{
				throw new Exception("Error in  XMaterial file: <variable_float4> node contained unknown rendervar.");
			}

			var binding = new VariableFloat3Binding { varId = floatId, parameter = param };

			Material.variableFloat3Bindings.Add(binding);
		}

		private void ParseFloat2Variable(XmlNode node)
		{
			// Parse matrix variable
			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <variable_float2> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float2> node must have semantic usage.");
			}

			var matIdNode = node.Attributes.GetNamedItem("varID");
			if (matIdNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float2> node must have rendervar attribute.");
			}

			var param = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);

			if (param == null)
			{
				throw new Exception("Error in XMaterial file: semantic not found in effect.");
			}

			VariableFloat2Id matId;

			try
			{
				matId = (VariableFloat2Id)Enum.Parse(typeof(VariableFloat2Id), matIdNode.InnerText, true);
			}
			catch (Exception)
			{
				throw new Exception("Error in XMaterial file: <variable_float2> node contained unknown rendervar.");
			}

			var binding = new VariableFloat2Binding { varId = matId, parameter = param };
			Material.variableFloat2Bindings.Add(binding);
		}

		private void ParseSampler(XmlNode node, ContentManager content)
		{
			// Parse sampler
			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <sampler> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <sampler> node must have semantic usage.");
			}

			var texture = content.Load<Texture>(string.Format("textures/{0}", node.InnerText));

			if (texture == null)
			{
				throw new Exception("Error in XMaterial file: failed to load texture.");
			}

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);

			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: Sampler semantic not found in effect.");
			}

			var binding = new TextureBinding { parameter = parameter, texture = texture };
			Material.textureBindings.Add(binding);
		}

		private void ParseFloatVariable(XmlNode node)
		{
			// Parse float variable
			if (node.ChildNodes.Count > 1)
			{
				throw new Exception("Error in XMaterial file: <variable_float> node cannot have Children nodes.");
			}

			var semanticNode = node.Attributes.GetNamedItem("semantic");
			if (semanticNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float> node must have semantic usage.");
			}

			var matIDNode = node.Attributes.GetNamedItem("varID");
			if (matIDNode == null)
			{
				throw new Exception("Error in XMaterial file: <variable_float> node must have rendervar attribute.");
			}

			var parameter = Material.Effect.Parameters.GetParameterBySemantic(semanticNode.InnerText);

			if (parameter == null)
			{
				throw new Exception("Error in XMaterial file: semantic not found in effect.");
			}

			VariableFloatId floatId;

			try
			{
				floatId = (VariableFloatId)Enum.Parse(
					typeof(VariableFloatId),
					matIDNode.InnerText,
					true);
			}
			catch (Exception)
			{
				throw new Exception("Error in XMaterial file: <variable_float> node contained unknown rendervar.");
			}

			var binding = new VariableFloatBinding { varId = floatId, parameter = parameter };
			Material.variableFloatBindings.Add(binding);
		}

		#endregion

		#endregion
	}
}
