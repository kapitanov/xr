using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Entities;
using Microsoft.Xna.Framework.Graphics;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace AISTek.XRage.Components
{
    public class ViewModelRenderComponent : BaseRenderComponent
    {
        public ViewModelRenderComponent(BaseEntity parent, string modelPath, string materialPath)
            : base(parent, materialPath)
        {
            this.modelPath = modelPath;
            CastsShadows = true;
        }

        public Model Model { get; private set; }

        public Vector3 Offset { get; set; }

        public override void LoadContent()
        {
            base.LoadContent();
            Model = Game.Content.Load<Model>(modelPath);

            BoundingSphere boundingSphere = new BoundingSphere();
            for (var i = 0; i < Model.Meshes.Count; i++)
            {
                var meshBoundingSphere = Model.Meshes[0].BoundingSphere;
                if (i == 0)
                {
                    boundingSphere = meshBoundingSphere;
                    continue;
                }

                boundingSphere = BoundingSphere.CreateMerged(boundingSphere, meshBoundingSphere);
            }

            boundary = new BoundingSphere(ParentEntity.Position + boundingSphere.Center, boundingSphere.Radius);
        }

        public override void Update(GameTime gameTime)
        {
            WorldTransform = Matrix.CreateScale(ParentEntity.Scale) *
                ParentEntity.Rotation *
                Matrix.CreateTranslation(Vector3.Transform(Offset, ParentEntity.Rotation)) *
                Matrix.CreateTranslation(ParentEntity.Position);
        }

        public override void UnloadContent()
        { }

        protected override bool CheckVisibility(ref RenderPassDescriptor pass)
        {
            return true;
        }

        protected override void ProvideGeometryChunk(ref RenderPassDescriptor pass)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    var chunk = Game.Graphics.RenderChunkManager.AllocateGeometryChunk();

                    chunk.Indices = part.IndexBuffer;
                    chunk.Material = Material;
                    chunk.Opacity = Opacity;
                    chunk.PrimitiveCount = part.PrimitiveCount;
                    chunk.StartIndex = part.StartIndex;
                    chunk.Type = PrimitiveType.TriangleList;
                    chunk.NumVertices = part.NumVertices;
                    chunk.VertexStreamOffset = part.VertexOffset;
                    chunk.VertexStreams.Add(part.VertexBuffer);
                    chunk.WorldTransform = WorldTransform;
                }
            }
        }

        private string modelPath;
        private BoundingSphere boundary;
    }
}
