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
    public class WorldModelRenderComponent : BaseRenderComponent
    {
        public WorldModelRenderComponent(BaseEntity parent, string modelPath, string materialPath)
            : base(parent, materialPath)
        {
            this.modelPath = modelPath;
            CastsShadows = true;
        }

        public Model Model { get; private set; }

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

            boundary = new BoundingSphere(boundingSphere.Center, boundingSphere.Radius);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UnloadContent()
        {
        }

        protected override bool CheckVisibility(ref RenderPassDescriptor pass)
        {
            var boundingSphere = new BoundingSphere(ParentEntity.Position + boundary.Center, boundary.Radius);
            ContainmentType containmentType;
            pass.RenderCamera.BoundingFrustum.Contains(ref boundingSphere, out containmentType);
            return containmentType != ContainmentType.Disjoint;
            //bool result;
            //pass.RenderCamera.BoundingFrustum.Intersects(ref boundingSphere, out result);
            //return result;

            //return pass.RenderCamera.BoundingFrustum.Intersects(boundingSphere);
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
