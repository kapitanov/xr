using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AISTek.XRage.Entities;

namespace AISTek.XRage.Composition
{
    public class TerrainStatisticsScreen : XComponent, ICompositionScreen
    {
        public TerrainStatisticsScreen(XGame game, Terrain terrain)
            : base(game)
        {
            this.terrain = terrain;
            IsEnabled = true;
        }

        public string Name { get { return "terrain_statistics"; } }

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
                string.Format("{0} terrain nodes drawn", terrain.Statistics.NodesVisualized),
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.75f,
                    Game.Graphics.GraphicsDevice.Viewport.Height - 50),
                color);

            Game.Graphics.SpriteBatch.End();
        }

        public void UnloadContent()
        { }

        private SpriteFont font;
        private Terrain terrain;
    }
}
