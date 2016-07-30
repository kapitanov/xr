using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Sample
{
    public class SampleRenderer : Renderer
    {
        public SampleRenderer(XGame game)
            : base(game)
        { }

        public override void Initialize()
        {
            basicEffect = new BasicEffect(Graphics.GraphicsDevice);
            basicEffect.LightingEnabled = true;

            basicEffect.EnableDefaultLighting();
            basicEffect.PreferPerPixelLighting = true;
        }

        public override void DrawFrame(ICamera camera, GameTime gameTime)
        {
            Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            ProcessGeometryChunks(camera);
        }

        private BasicEffect basicEffect;

        private void GetGeometryChunks(ICamera camera)
        {
            var pass = new RenderPassDescriptor
            {
                RequestedLod = LevelOfDetail.High,
                RenderCamera = camera,
                Type = RenderPassType.SolidGeometry
            };

            Graphics.RenderChunkProviders.QueryForChunks(ref pass);

            //passDesc.ViewFrustum = new BoundingFrustum(
            //    variableMatrix[(int)VariableMatrixId.View] *
            //    variableMatrix[(int)VariableMatrixId.Projection]);
        }

        private void ProcessGeometryChunks(ICamera camera)
        {
            GetGeometryChunks(camera);
            var geometryChunkList = Graphics.RenderChunkManager.GeometryRenderChunks;

            for (var orderToRender = PreferredRenderOrder.RenderFirst;
                 orderToRender < PreferredRenderOrder.NumOfRenderOrders;
                 orderToRender++)
            {
                for (int i = geometryChunkList.Count - 1; i >= 0; --i)
                {
                    var chunk = geometryChunkList[i];

                    if (chunk.RenderOrder != orderToRender)
                        continue;
                    Graphics.Variables.Set(VariableMatrixId.World, chunk.WorldTransform);

                    Graphics.Variables.Set(
                        VariableMatrixId.WorldView,
                        Graphics.Variables.Get(VariableMatrixId.World) * Graphics.Variables.Get(VariableMatrixId.View));

                    Graphics.Variables.Set(
                        VariableMatrixId.WorldViewProjection,
                        Graphics.Variables.Get(VariableMatrixId.WorldView) * Graphics.Variables.Get(VariableMatrixId.Projection));
                    
                    Graphics.GraphicsDevice.SetVertexBuffers(
                        chunk.VertexStreams
                        .Select(buffer => new VertexBufferBinding(buffer))
                        .ToArray());
                    Graphics.GraphicsDevice.Indices = chunk.Indices;

                    basicEffect.World = Graphics.Variables.Get(VariableMatrixId.World);
                    basicEffect.View = Graphics.Variables.Get(VariableMatrixId.View);
                    basicEffect.Projection = Graphics.Variables.Get(VariableMatrixId.Projection);

                    foreach (var pass in basicEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        Graphics.GraphicsDevice.DrawIndexedPrimitives(
                            chunk.Type,
                            chunk.VertexStreamOffset,
                            0,
                            chunk.NumVertices,
                            chunk.StartIndex,
                            chunk.PrimitiveCount);
                    }

                    Graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                }
            }
        }

    }
}
