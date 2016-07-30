using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Composition;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Sample
{
    public class PausedScreen : XComponent, ICompositionScreen
    {
        public PausedScreen(XGame game)
            : base(game)
        {
            Description = string.Empty;
        }

        public string Name { get { return "paused_screen"; } }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public void Initialize(VisualComposer visualComposer)
        { }

        public void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("sprites/fonts/preloader");
        }

        public void Update(GameTime gameTime)
        { }

        public void Draw(GameTime gameTime)
        {
            currentFrame += (gameTime.ElapsedGameTime.TotalMilliseconds / 40.0f);
            if (currentFrame >= AnimationFrames.Length)
                currentFrame = 0;

            Game.Graphics.SpriteBatch.Begin();

            Game.Graphics.SpriteBatch.DrawString(
                font,
                AnimationFrames[(int)currentFrame],
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.40f,
                    Game.Graphics.GraphicsDevice.Viewport.Height * 0.45f),
                Color.YellowGreen);

            Game.Graphics.SpriteBatch.End();
        }

        public void UnloadContent()
        { }

        private SpriteFont font;
        private double currentFrame;

        private readonly string[] AnimationFrames = new string[]
        {
            @"[ (|) PAUSED (|) ]",
            @"[ (/) PAUSED (/) ]",
            @"[ (-) PAUSED (-) ]",
            @"[ (\) PAUSED (\) ]",
            @"[ (|) PAUSED (|) ]",
            @"[ (/) PAUSED (/) ]",
            @"[ (-) PAUSED (-) ]",
            @"[ (\) PAUSED (\) ]",
        };
    }
}
