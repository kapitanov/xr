using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Content.SceneManagement
{
    [DebuggerDisplay("{Color} ({Vertices[0].position}) ({Vertices[1].position}) ({Vertices[2].position})")]
    public class VmfBrushSide
    {
        public VmfBrushSide()
        {
            Vertices = new VertexPositionNormalTexture[3];

        }

        public VmfBrushSide(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 normal)
        {
            Color = new Color(Math.Abs(normal.X), Math.Abs(normal.Y), Math.Abs(normal.Z));
            Vertices = new[]
            {
                new VertexPositionNormalTexture{Position = v0, TextureCoordinate = CalcTexCoord(v0, v0,v1,v2), Normal = normal},
                new VertexPositionNormalTexture{Position = v1, TextureCoordinate = CalcTexCoord(v1, v0,v1,v2), Normal = normal},
                new VertexPositionNormalTexture{Position = v2, TextureCoordinate = CalcTexCoord(v2, v0,v1,v2), Normal = normal}
            };
        }

        public VertexPositionNormalTexture[] Vertices { get; set; }

        private static Vector2 CalcTexCoord(Vector3 vertex, params Vector3[] vertices)
        {
            var vertexA = vertices.Aggregate((x, y) => x + y) / vertices.Length;
            var u = Vector3.Normalize(vertexA - vertices[0]);
            var v = Vector3.Normalize(vertices[1] - u * Vector3.Dot(vertices[1], u));

            return new Vector2(
                    Vector3.Dot(vertex - vertexA, u),
                    Vector3.Dot(vertex - vertexA, v));
        }

        public Color Color { get; private set; }
    }
}
