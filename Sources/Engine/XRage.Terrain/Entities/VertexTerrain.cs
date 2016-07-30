using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Entities
{
	internal struct VertexTerrain : IVertexType
	{
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 TexCoord;

		public static VertexDeclaration VertexDeclaration = new VertexDeclaration(
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
			new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}
	}
}
