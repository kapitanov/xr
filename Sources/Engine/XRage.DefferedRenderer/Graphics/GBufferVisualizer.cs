using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    internal class GBufferVisualizer : XComponent
    {
        public GBufferVisualizer(XGame game, DeferredRenderer renderer)
            : base(game)
        {
            Renderer = renderer;
        }

        public DeferredRenderer Renderer { get; private set; }

        public GraphicsSystem Graphics { get { return Game.Graphics; } }

        public void Draw()
        {
            if (Renderer.IsDebugVisualMode)
            {
                DrawDebugVisualMode();
            }
            else
            {
                DrawNormalVisualMode();
            }
        }

        private void DrawNormalVisualMode()
        {
            Graphics.GraphicsDevice.SetRenderTarget(null);
            Graphics.GraphicsDevice.Clear(Color.Black);

            Graphics.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            Graphics.SpriteBatch.Draw(Renderer.BackBuffer, Vector2.Zero, Color.White);

            Graphics.SpriteBatch.End();
        }

        private void DrawDebugVisualMode()
        {
            var halfWidth = Graphics.GraphicsDevice.Viewport.Width / 2;
            var halfHeight = Graphics.GraphicsDevice.Viewport.Height / 2;

            Graphics.GraphicsDevice.SetRenderTarget(null);
            Graphics.GraphicsDevice.Clear(Color.Black);

            Graphics.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            Graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            Graphics.SpriteBatch.Draw(Renderer.GBuffer.DiffuseBuffer, new Rectangle(0, 0, halfWidth, halfHeight), Color.White);
            Graphics.SpriteBatch.Draw(Renderer.GBuffer.NormalBuffer, new Rectangle(0, halfHeight, halfWidth, halfHeight), Color.White);
            //Graphics.SpriteBatch.Draw(Renderer.LightBuffer, new Rectangle(halfWidth, 0, halfWidth, halfHeight), Color.White);

            Graphics.SpriteBatch.Draw(Renderer.ShadowMapRenderer.ShadowOcclusion, new Rectangle(halfWidth, 0, halfWidth, halfHeight), Color.White);


            Graphics.SpriteBatch.Draw(Renderer.ShadowMapRenderer.ShadowOcclusionMask, new Rectangle(halfWidth, 0, halfWidth, halfHeight), Color.White);
           // Graphics.SpriteBatch.Draw(Renderer.LightBuffer, new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight), Color.White);

            //Graphics.SpriteBatch.Draw(Renderer.LightBuffer, new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight), Color.White);

            //   Graphics.SpriteBatch.Draw(Renderer.BackBuffer, new Rectangle(0, 0, halfWidth*2, halfHeight*2), Color.White);

            Graphics.SpriteBatch.End();
        }
    }
}
