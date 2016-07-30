using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Composition
{
    public class FpsCounterScreen : XComponent, ICompositionScreen
    {
        public FpsCounterScreen (XGame game)
            : base(game)
        { }

        public string Name { get { return "fps_counter"; } }

        public bool IsEnabled { get { return font != null; } }
        
        public FpsCounter FpsCounter { get; private set; } 

        public void Initialize(VisualComposer visualComposer)
        {
            FpsCounter = new FpsCounter(Game);
        }

        public void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("sprites/fonts/fpsCounterFont");
        }

        public void Update(GameTime gameTime)
        { }

        public void Draw(GameTime gameTime)
        {
            FpsCounter.OnDrawFrame();
            var color = Color.Red;
            if (FpsCounter.FramesPerSecond >= 25)
                color = Color.OrangeRed;
            if (FpsCounter.FramesPerSecond >= 30)
                color = Color.Yellow;
            if (FpsCounter.FramesPerSecond >= 40)
                color = Color.YellowGreen;
            if (FpsCounter.FramesPerSecond >= 50)
                color = Color.Green;

            Game.Graphics.SpriteBatch.Begin();
            Game.Graphics.SpriteBatch.DrawString(
                font,
                string.Format("{0} frames per second", FpsCounter.FramesPerSecond),
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.75f,
                    10),
                color);
            Game.Graphics.SpriteBatch.End();
        }

        public void UnloadContent()
        { }

        private SpriteFont font;
    }
}
