using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    public struct VertexPositionNormalBinormalTangentTexture : IVertexType
    {
        public Vector3 Position;
        public Vector2 TextureCoordinate;
        public Vector3 Normal;
        public Vector3 Binormal;
        public Vector3 Tangent;

        private static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(3 * sizeof(float), VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(5 * sizeof(float), VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(8 * sizeof(float), VertexElementFormat.Vector3, VertexElementUsage.Binormal, 0),
            new VertexElement(11 * sizeof(float), VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0));

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
    }
}
