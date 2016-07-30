using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Content.SceneManagement
{
    internal sealed class SmoothingXBrushCompiler
    {
        private SmoothingXBrushCompiler()
        { }

        public static SmoothingXBrushCompiler Compile(IEnumerable<CompiledXFace> faces)
        {
            var compiler = new SmoothingXBrushCompiler();
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
                    AddVertex(position, face.Normal, face.TexCoords[index++]);
                }
            }

            foreach (var face in faces)
            {
                for (var i = 1; i < face.Positions.Count - 1; i++)
                {
                    AddIndex(face.Positions[0], face.Normal, face.TexCoords[0]);
                    AddIndex(face.Positions[i], face.Normal, face.TexCoords[i]);
                    AddIndex(face.Positions[i + 1], face.Normal, face.TexCoords[i + 1]);
                }
            }

            Vertices = vertices.Select(v => v.Convert()).ToArray();
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

        private void AddVertex(Vector3 position, Vector3 normal, Vector2 texCoords)
        {
            if (vertices.Where(v => v.Position == position).Count() > 0)
            {
                // Combine normal
                var vertex = vertices.Where(v => v.Position == position).First();
                vertex.AddNormal(normal);
            }
            else
            {
                vertices.Add(new VertexData(position, normal, texCoords));
            }
        }

        private void AddIndex(Vector3 position, Vector3 normal, Vector2 texCoords)
        {
            var index = (short)vertices.Select((v, i) => new { v.Position, Index = i })
                                       .Where(v => v.Position == position)
                                       .Select(v => v.Index)
                                       .First();
            indices.Add(index);
        }

        private List<short> indices = new List<short>();
        private List<VertexData> vertices = new List<VertexData>();

        private struct VertexData
        {
            public VertexData(Vector3 position, Vector3 normal, Vector2 texCoords)
            {
                Position = position;
                CombinedNormal = normal;
                TextureCoordinate = texCoords;
                CombinedNormalsCount = 1;
            }

            public Vector3 Position;
            public Vector3 CombinedNormal;
            public Vector2 TextureCoordinate;
            public int CombinedNormalsCount;

            public void AddNormal(Vector3 normal)
            {
                CombinedNormal += normal;
                CombinedNormalsCount++;
            }

            public VertexPositionNormalBinormalTangentTexture Convert()
            {
                return new VertexPositionNormalBinormalTangentTexture
                {
                    Position = Position,
                    TextureCoordinate = TextureCoordinate,
                    Normal = Vector3.Normalize(CombinedNormal / CombinedNormalsCount),
                    Binormal = Vector3.Zero,
                    Tangent = Vector3.Zero
                };
            }
        }
    }
}
