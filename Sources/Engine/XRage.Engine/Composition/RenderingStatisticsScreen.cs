using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Composition
{
    public class RenderingStatisticsScreen : XComponent, ICompositionScreen
    {
        public RenderingStatisticsScreen(XGame game)
            : base(game)
        { }

        public string Name { get { return "rendering_statistics"; } }

        public bool IsEnabled { get; set; }
        
        public void Initialize(VisualComposer visualComposer)
        { }

        public void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("sprites/fonts/statisticsFont");
        }

        public void Update(GameTime gameTime)
        { }

        public void Draw(GameTime gameTime)
        {
            var color = Color.Green;

            Game.Graphics.SpriteBatch.Begin();
            Game.Graphics.SpriteBatch.DrawString(
                font,
                string.Format("{0} geometry chunks drawn", Game.Graphics.RenderingStatistics.GeometryChunksDrawn),
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.75f,
                    50),
                color);
            Game.Graphics.SpriteBatch.DrawString(
                font,
                string.Format("{0} lighting chunks drawn", Game.Graphics.RenderingStatistics.LightChunksDrawn),
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.75f,
                    70),
                color);
            Game.Graphics.SpriteBatch.DrawString(
                font,
                string.Format("{0:N0} primitives drawn", Game.Graphics.RenderingStatistics.PrimitivesDraw),
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.75f,
                    90),
                color);

            Game.Graphics.SpriteBatch.End();
        }

        public void UnloadContent()
        { }

        private SpriteFont font;
    }
}
