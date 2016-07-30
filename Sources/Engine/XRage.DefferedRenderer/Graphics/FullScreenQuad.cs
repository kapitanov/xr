using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Graphics
{
    internal class FullScreenQuad
    {
        public FullScreenQuad(GraphicsDevice graphicsDevice)
        {
            CreateFullScreenQuad(graphicsDevice);
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect shader, BlendState customBlendState)
        {
            graphicsDevice.BlendState = customBlendState;

            foreach (var pass in shader.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.SetVertexBuffer(vertexBuffer);
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, EffectTechnique technique)
        {
            foreach (var pass in technique.Passes)
            {
                pass.Apply();

                graphicsDevice.SetVertexBuffer(vertexBuffer);
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect shader)
        {
            Draw(graphicsDevice, shader, blendState);
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }

        private void CreateFullScreenQuad(GraphicsDevice graphicsDevice)
        {
            // Create a vertex buffer for the quad, and fill it in
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(QuadVertex), 4, BufferUsage.None);
            var vbData = new QuadVertex[4];

            // Upper right
            vbData[0].Position = new Vector3(1, 1, 1);
            vbData[0].TexCoordAndCornerIndex = new Vector3(1, 0, 1);

            // Lower right
            vbData[1].Position = new Vector3(1, -1, 1);
            vbData[1].TexCoordAndCornerIndex = new Vector3(1, 1, 2);

            // Upper left
            vbData[2].Position = new Vector3(-1, 1, 1);
            vbData[2].TexCoordAndCornerIndex = new Vector3(0, 0, 0);

            // Lower left
            vbData[3].Position = new Vector3(-1, -1, 1);
            vbData[3].TexCoordAndCornerIndex = new Vector3(0, 1, 3);


            vertexBuffer.SetData(vbData);
        }

        private VertexBuffer vertexBuffer;
        private BlendState blendState = new BlendState
            {
                AlphaBlendFunction = BlendFunction.Add,              
                ColorWriteChannels = ColorWriteChannels.All,
                ColorWriteChannels1 = ColorWriteChannels.All,
                ColorWriteChannels2 = ColorWriteChannels.All,
                ColorWriteChannels3 = ColorWriteChannels.All
            };
    }
}
