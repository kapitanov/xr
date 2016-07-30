using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Sample
{
    public class SampleModel : IRenderChunkProvider
    {
        public SampleModel(XGame game, string modelPath, string materialPath)
        {
            Game = game;
            Model = Game.Content.Load<Model>(modelPath);
            Material = Game.Content.Load<Material>(materialPath);
        }

        public XGame Game { get; private set; }

        public Model Model { get; private set; }

        public Material Material { get; private set; }

        public Vector3 Position { get; set; }

        public float YRotation { get; set; }

        public float XRotation { get; set; }

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            var chunk = Game.Graphics.RenderChunkManager.AllocateGeometryChunk();
            chunk.Material = Material;
            chunk.VertexStreams.Add(Model.Meshes[0].MeshParts[0].VertexBuffer);
            chunk.Indices = Model.Meshes[0].MeshParts[0].IndexBuffer;
            chunk.PrimitiveCount = Model.Meshes[0].MeshParts[0].PrimitiveCount;
            chunk.NumVertices = Model.Meshes[0].MeshParts[0].NumVertices;
            chunk.WorldTransform = Matrix.CreateRotationY(MathHelper.ToRadians(YRotation)) *
                Matrix.CreateRotationX(MathHelper.ToRadians(XRotation)) *
                Matrix.CreateTranslation(Position);
        }

        public void Move(Vector3 movement)
        {
            Position += movement;
        }

        public void RotateY(float angle)
        {
            YRotation += angle;
        }

        public void RotateX(float angle)
        {
            XRotation += angle;
        }
    }
}
