using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Entities
{
    internal class BoundingBoxMesh : XComponent, IDisposable
    {
        public BoundingBoxMesh(QuadTree node)
            : base(node.Terrain.Game)
        {
            CreateVertexBuffer(node.BoundingBox);
            CreateIndexBuffer();
            VisibleMaterial = Game.Content.Load<Material>("materials/wireframe_green");
            HiddenMaterial = Game.Content.Load<Material>("materials/wireframe_red");
        }

        private void CreateVertexBuffer(BoundingBox boundingBox)
        {
            var vertices = new VertexPositionColor[8];
            var corners = boundingBox.GetCorners();

            for (int i = 0; i < 8; ++i)
            {
                vertices[i].Position = corners[i];
            }

            VertexBuffer = new VertexBuffer(Game.Graphics.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);            
        }

        private void CreateIndexBuffer()
        {
            var indices = new short[36];

            // Front
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            // Back
            indices[6] = 4;
            indices[7] = 5;
            indices[8] = 6;
            indices[9] = 4;
            indices[10] = 6;
            indices[11] = 7;

            // Top
            indices[12] = 4;
            indices[13] = 5;
            indices[14] = 1;
            indices[15] = 4;
            indices[16] = 1;
            indices[17] = 0;

            // Bottom
            indices[18] = 7;
            indices[19] = 6;
            indices[20] = 2;
            indices[21] = 7;
            indices[22] = 2;
            indices[23] = 3;

            // Left
            indices[24] = 0;
            indices[25] = 4;
            indices[26] = 7;
            indices[27] = 0;
            indices[28] = 7;
            indices[29] = 3;

            // Right
            indices[30] = 1;
            indices[31] = 5;
            indices[32] = 6;
            indices[33] = 1;
            indices[34] = 6;
            indices[35] = 2;

            IndexBuffer = new IndexBuffer(Game.Graphics.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(indices);
        }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            if (pass.Type == RenderPassType.SolidGeometry)
            {
                var chunk = Game.Graphics.RenderChunkManager.AllocateGeometryChunk();
                chunk.Indices = IndexBuffer;
                chunk.VertexStreams.Add(VertexBuffer);
                chunk.WorldTransform = Matrix.Identity;
                chunk.NumVertices = VertexBuffer.VertexCount;
                chunk.StartIndex = 0;
                chunk.VertexStreamOffset = 0;
                chunk.PrimitiveCount = IndexBuffer.IndexCount / 3;
                chunk.Material = IsVisible ? VisibleMaterial : HiddenMaterial;
                chunk.Type = PrimitiveType.TriangleList;
            }
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
            VisibleMaterial.Dispose();
        }

        public VertexBuffer VertexBuffer { get; private set; }

        public IndexBuffer IndexBuffer { get; private set; }

        public Material VisibleMaterial { get; private set; }

        public Material HiddenMaterial { get; private set; }

        public bool IsVisible { get; set; }
    }
}
