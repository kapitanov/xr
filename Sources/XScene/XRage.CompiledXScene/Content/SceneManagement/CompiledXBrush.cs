using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using AISTek.XRage.Graphics;

namespace AISTek.XRage.Content.SceneManagement
{
    public class CompiledXBrush
    {
        public CompiledXBrush(string materialPath, IEnumerable<CompiledXFace> faces, bool useSmoothing)
        {
            MaterialPath = materialPath;

            if (useSmoothing)
            {
                var compiler = SmoothingXBrushCompiler.Compile(faces);
                Vertices = compiler.Vertices;
                Indices = compiler.Indices;
                BoundingBox = compiler.BoundingBox;
            }
            else
            {
                var compiler = XBrushCompiler.Compile(faces);
                Vertices = compiler.Vertices;
                Indices = compiler.Indices;
                BoundingBox = compiler.BoundingBox;
            }
        }

        private CompiledXBrush(
            string materialPath,
            VertexPositionNormalBinormalTangentTexture[] vertices,
            short[] indices,
            BoundingBox boundingBox)
        {
            MaterialPath = materialPath;
            Vertices = vertices;
            Indices = indices;
            BoundingBox = boundingBox;
        }

        public string MaterialPath { get; private set; }

        public VertexPositionNormalBinormalTangentTexture[] Vertices { get; private set; }

        public short[] Indices { get; private set; }

        public BoundingBox BoundingBox { get; private set; }

        public void WriteToContent(IContentWriterWrapper output)
        {
            // Write material
            output.Write(MaterialPath);

            // Write vertex buffer
            output.Write(Vertices.Length);
            foreach (var vertex in Vertices)
            {
                output.Write(vertex.Position);
                output.Write(vertex.TextureCoordinate);
                output.Write(vertex.Normal);
                output.Write(vertex.Binormal);
                output.Write(vertex.Tangent);
            }

            // Write index buffer
            output.Write(Indices.Length);
            foreach (var index in Indices)
            {
                output.Write(index);
            }

            // Write bounding box
            output.Write(BoundingBox.Min);
            output.Write(BoundingBox.Max);
        }

        public static CompiledXBrush ReadFromContent(ContentReader input)
        {
            // Read material
            var materialPath = input.ReadString();

            // Read vertex buffer
            var verticesCount = input.ReadInt32();
            var vertices = Enumerable.Range(0, verticesCount)
                .Select(_ => new VertexPositionNormalBinormalTangentTexture
                {
                    Position = input.ReadVector3(),
                    TextureCoordinate = input.ReadVector2(),
                    Normal = input.ReadVector3(),
                    Binormal = input.ReadVector3(),
                    Tangent = input.ReadVector3()
                })
                .ToArray();

            // Read index buffer
            var indicesCount = input.ReadInt32();
            var indices = Enumerable.Range(0, indicesCount)
                .Select(_ => input.ReadInt16())
                .ToArray();

            // Read bounding box
            var boundinBox = new BoundingBox(input.ReadVector3(), input.ReadVector3());

            return new CompiledXBrush(materialPath, vertices, indices, boundinBox);
        }
    }
}
