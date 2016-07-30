using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Content.SceneManagement
{
    internal sealed class XBrushCompiler
    {
        private XBrushCompiler()
        { }

        public static XBrushCompiler Compile(IEnumerable<CompiledXFace> faces)
        {
            var compiler = new XBrushCompiler();
            compiler.CompileInternal(faces.ToList());
            return compiler;
        }

        public VertexPositionNormalBinormalTangentTexture[] Vertices { get; private set; }

        public short[] Indices { get; private set; }

        public BoundingBox BoundingBox { get; private set; }

        private void CompileInternal(List<CompiledXFace> faces)
        {
            foreach (var face in faces)
            {
                var index = 0;
                foreach (var position in face.Positions)
                {
                    AddVertex(position, face.TexCoords[index++], face.Normal, face.Binormal, face.Tangent);
                }
            }

            foreach (var face in faces)
            {
                for (var i = 1; i < face.Positions.Count - 1; i++)
                {
                    AddIndex(face.Positions[0], face.TexCoords[0], face.Normal, face.Binormal, face.Tangent);
                    AddIndex(face.Positions[i], face.TexCoords[i], face.Normal, face.Binormal, face.Tangent);
                    AddIndex(face.Positions[i + 1], face.TexCoords[i + 1], face.Normal, face.Binormal, face.Tangent);
                }
            }

            Vertices = vertices.ToArray();
            Indices = indices.ToArray();

            BuildBoundingBox();
        }

        private void BuildBoundingBox()
        {
            var firstCorner = vertices.First().Position;
            var lastCorner = vertices.First().Position;

            foreach (var vertex in vertices.Skip(1).Select(x => x.Position))
            {
                firstCorner = new Vector3(
                   MathHelper.Min(firstCorner.X, vertex.X),
                   MathHelper.Min(firstCorner.Y, vertex.Y),
                   MathHelper.Min(firstCorner.Z, vertex.Z));

                lastCorner = new Vector3(
                    MathHelper.Max(lastCorner.X, vertex.X),
                    MathHelper.Max(lastCorner.Y, vertex.Y),
                    MathHelper.Max(lastCorner.Z, vertex.Z));
            }

            BoundingBox = new BoundingBox(firstCorner, lastCorner);
        }

        private void AddVertex(Vector3 position, Vector2 texCoords, Vector3 normal, Vector3 binormal, Vector3 tangent)
        {
            var vertex = new VertexPositionNormalBinormalTangentTexture
            {
                Position = position,
                TextureCoordinate = texCoords,
                Normal = normal,
                Binormal = binormal,
                Tangent = tangent
            };

            if (!vertices.Contains(vertex))
            {
                vertices.Add(vertex);
            }
        }

        private void AddIndex(Vector3 position, Vector2 texCoords, Vector3 normal, Vector3 binormal, Vector3 tangent)
        {
            var vertex = new VertexPositionNormalBinormalTangentTexture
            {
                Position = position,
                TextureCoordinate = texCoords,
                Normal = normal,
                Binormal = binormal,
                Tangent = tangent
            };

            var index = (short)vertices.IndexOf(vertex);
            if (index < 0)
            {
                throw new InvalidOperationException(string.Format(
                    "Specified vertex doesn't exist in vertex buffer:\nPos: {0}\nTexCoord:{1}\nN: {2}\nB:{3}\nT: {4}",
                    position, texCoords, normal, binormal, tangent));
            }

            indices.Add(index);
        }

        private List<short> indices = new List<short>();
        private List<VertexPositionNormalBinormalTangentTexture> vertices = new List<VertexPositionNormalBinormalTangentTexture>();
    }
}
