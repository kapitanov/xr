using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.Contracts;

namespace AISTek.XRage.SceneManagement
{
    public class VmfBrush
    {
        public VmfBrush(VmfScene scene, VertexPositionNormalTexture[] vertices, string materialPath)
        {
            this.materialPath = materialPath;
            this.vertices = vertices;
            Scene = scene;
        }

        public VmfScene Scene { get; private set; }

        public XGame Game { get { return Scene.Game; } }

        public Material Material { get; private set; }

        public void LoadContent()
        {
            Material = Game.Content.Load<Material>(materialPath);

            vertexBuffer = new VertexBuffer(Game.Graphics.GraphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            var indices = new short[vertices.Length];
            for (short i = 0; i < vertices.Length; i++)
            {
                indices[i] = i;
            }

            indexBuffer = new IndexBuffer(Game.Graphics.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);

            var firstCorner = new Vector3(
                MathHelper.Min(vertices[0].Position.X, MathHelper.Min(vertices[1].Position.X, vertices[2].Position.X)),
                MathHelper.Min(vertices[0].Position.Y, MathHelper.Min(vertices[1].Position.Y, vertices[2].Position.Y)),
                MathHelper.Min(vertices[0].Position.Z, MathHelper.Min(vertices[1].Position.Z, vertices[2].Position.Z)));

            var lastCorner = new Vector3(
                MathHelper.Max(vertices[0].Position.X, MathHelper.Max(vertices[1].Position.X, vertices[2].Position.X)),
                MathHelper.Max(vertices[0].Position.Y, MathHelper.Max(vertices[1].Position.Y, vertices[2].Position.Y)),
                MathHelper.Max(vertices[0].Position.Z, MathHelper.Max(vertices[1].Position.Z, vertices[2].Position.Z)));


            for (var i = 1; i < vertices.Length / 3; i++)
            {
                var f = new Vector3(
                    MathHelper.Min(vertices[i].Position.X, MathHelper.Min(vertices[i + 1].Position.X, vertices[i + 2].Position.X)),
                    MathHelper.Min(vertices[i].Position.Y, MathHelper.Min(vertices[i + 1].Position.Y, vertices[i + 2].Position.Y)),
                    MathHelper.Min(vertices[i].Position.Z, MathHelper.Min(vertices[i + 1].Position.Z, vertices[i + 2].Position.Z)));

                var l = new Vector3(
                    MathHelper.Max(vertices[i].Position.X, MathHelper.Max(vertices[i + 1].Position.X, vertices[i + 2].Position.X)),
                    MathHelper.Max(vertices[i].Position.Y, MathHelper.Max(vertices[i + 1].Position.Y, vertices[i + 2].Position.Y)),
                    MathHelper.Max(vertices[i].Position.Z, MathHelper.Max(vertices[i + 1].Position.Z, vertices[i + 2].Position.Z)));

                firstCorner = new Vector3(
                    MathHelper.Min(firstCorner.X, f.X),
                    MathHelper.Min(firstCorner.Y, f.Y),
                    MathHelper.Min(firstCorner.Z, f.Z));

                lastCorner = new Vector3(
                    MathHelper.Max(lastCorner.X, l.X),
                    MathHelper.Max(lastCorner.Y, l.Y),
                    MathHelper.Max(lastCorner.Z, l.Z));
            }
            
            boundingBox = new BoundingBox(firstCorner, lastCorner);
        }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            if (pass.Type != RenderPassType.SolidGeometry)
                return;

            if (!pass.RenderCamera.BoundingFrustum.Intersects(boundingBox))
                return;

            var chunk = Game.Graphics.RenderChunkManager.AllocateGeometryChunk();

            chunk.Indices = indexBuffer;
            chunk.Material = Material;
            chunk.Opacity = 1;
            chunk.PrimitiveCount = indexBuffer.IndexCount / 3;
            chunk.StartIndex = 0;
            chunk.Type = PrimitiveType.TriangleList;
            chunk.NumVertices = vertexBuffer.VertexCount;
            chunk.VertexStreamOffset = 0;
            chunk.VertexStreams.Add(vertexBuffer);
            chunk.WorldTransform = Matrix.Identity;
        }

        private BoundingBox boundingBox;
        private VertexPositionNormalTexture[] vertices;
        private string materialPath;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
    }
}
