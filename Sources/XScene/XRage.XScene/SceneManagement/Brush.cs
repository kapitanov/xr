using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework.Graphics;
using AISTek.XRage.Content.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace AISTek.XRage.SceneManagement
{
    public class Brush : XComponent
    {
        public Brush(XGame game)
            : base(game)
        { }

        internal void LoadContent(CompiledXBrush xBrush)
        {
            try
            {
                Material = Game.Content.Load<Material>(PropertyConvertor.MaterialPath(xBrush.MaterialPath));
            }
            catch (ContentLoadException e)
            {
                Trace.TraceError(
                    "Unable to load material \"{0}\": {1}.\nUsing \"{2}\" instead",
                    xBrush.MaterialPath,
                    e.Message,
                    DefaultMaterialPath);
                Material = Game.Content.Load<Material>(DefaultMaterialPath);
            }

            BoundingBox = xBrush.BoundingBox;

            Debug.Print("\nBrush: [{0}]", xBrush.MaterialPath);
            foreach (var v in xBrush.Vertices)
            {
                Debug.Print("Vertex P:{0} N:{1}", v.Position, v.Normal);
            }

            VertexBuffer = new VertexBuffer(Game.Graphics.GraphicsDevice, typeof(VertexPositionNormalBinormalTangentTexture), xBrush.Vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(xBrush.Vertices);

            IndexBuffer = new IndexBuffer(Game.Graphics.GraphicsDevice, IndexElementSize.SixteenBits, xBrush.Indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(xBrush.Indices);
        }

        public Material Material { get; private set; }

        public VertexBuffer VertexBuffer { get; private set; }

        public IndexBuffer IndexBuffer { get; private set; }

        public BoundingBox BoundingBox { get; private set; }

        public static bool CastsShadows = true;

        public void QueryForChunks(ref RenderPassDescriptor pass)
        {
            // TODO: solid geometry may cast shadows
            if ((pass.Type == RenderPassType.SolidGeometry) ||
                (pass.Type == RenderPassType.ShadowTargets) ||
                (CastsShadows && pass.Type == RenderPassType.ShadowCasters))
            {
                //if (!pass.RenderCamera.BoundingFrustum.Intersects(BoundingBox))
                //    return;

                var chunk = Game.Graphics.RenderChunkManager.AllocateGeometryChunk();

                chunk.Indices = IndexBuffer;
                chunk.Material = Material;
                chunk.Opacity = 1;
                chunk.PrimitiveCount = IndexBuffer.IndexCount / 3;
                chunk.StartIndex = 0;
                chunk.Type = PrimitiveType.TriangleList;
                chunk.NumVertices = VertexBuffer.VertexCount;
                chunk.VertexStreamOffset = 0;
                chunk.VertexStreams.Add(VertexBuffer);
                chunk.WorldTransform = Matrix.Identity;
            }
        }

        private const string DefaultMaterialPath = "materials/empty";
    }
}
