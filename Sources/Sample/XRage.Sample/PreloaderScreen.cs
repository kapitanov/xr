using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISTek.XRage.Composition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Sample
{
    public class PreloaderScreen : XComponent, ICompositionScreen
    {
        public PreloaderScreen(XGame game)
            : base(game)
        {
            Description = string.Empty;
            Game.SceneManager.SceneLoadingProgress += (_, e) =>
            {
                lock (this)
                {
                    progress = e.LoadingState.Progress;
                    Description = string.Format("{0}\n    [{1} %]", e.LoadingState.Stage, e.LoadingState.Progress);
                    Scene = e.LoadingState.Scene;
                }
            };
        }

        public string Name { get { return "preloader"; } }

        public string Scene { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public void Initialize(VisualComposer visualComposer)
        { }

        public void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("sprites/fonts/preloader");

            animationFrames = new Texture2D[8];
            for (int i = 0; i < animationFrames.Length; i++)
            {
                animationFrames[i] = Game.Content.Load<Texture2D>(string.Format("sprites/preloader/frame_{0}", i));
            }

            progressBarBackground = Game.Content.Load<Texture2D>("sprites/preloader/progressBarBackground");
            progressBarForeground = Game.Content.Load<Texture2D>("sprites/preloader/progressBarForeground");
        }

        public void Update(GameTime gameTime)
        { }

        public void Draw(GameTime gameTime)
        {
            currentFrame += (gameTime.ElapsedGameTime.TotalMilliseconds / 50.0f);
            if (currentFrame >= animationFrames.Length)
                currentFrame = 0;
            Game.Graphics.SpriteBatch.Begin();

            var frame = animationFrames[(int)currentFrame];
            Game.Graphics.SpriteBatch.Draw(
                frame,
                0.5f * new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width - frame.Width,
                    Game.Graphics.GraphicsDevice.Viewport.Height - frame.Height),
                Color.White);

            Game.Graphics.SpriteBatch.DrawString(
               font,
               Scene,
               new Vector2(
                   Game.Graphics.GraphicsDevice.Viewport.Width * 0.35f,
                   Game.Graphics.GraphicsDevice.Viewport.Height * 0.50f),
               new Color(0, 127, 127));

            Game.Graphics.SpriteBatch.DrawString(
                font,
                Description,
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.35f,
                    Game.Graphics.GraphicsDevice.Viewport.Height * 0.55f),
                new Color(0, 127, 127));

            var progressBarPosition = new Vector2(
                    0.5f * (Game.Graphics.GraphicsDevice.Viewport.Width - progressBarBackground.Width),
                    Game.Graphics.GraphicsDevice.Viewport.Height - progressBarBackground.Height);

            Game.Graphics.SpriteBatch.Draw(
                progressBarBackground,
                progressBarPosition,
                Color.White);

            int progressValue;
            lock (this)
            {
                progressValue = progress;
            }

            for (var i = 0; i < progressValue / 10; i++)
            {
                Game.Graphics.SpriteBatch.Draw(
                    progressBarForeground,
                    progressBarPosition + new Vector2(progressBarForeground.Width * i, 0),
                Color.White);
            }

            Game.Graphics.SpriteBatch.End();
        }

        public void UnloadContent()
        { }

        private int progress;
        private SpriteFont font;
        private double currentFrame;
        private Texture2D[] animationFrames;
        private Texture2D progressBarBackground;
        private Texture2D progressBarForeground;
    }
}
