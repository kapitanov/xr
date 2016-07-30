using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Composition
{
    public class VisualComposer : XComponent
    {
        public VisualComposer(XGame game)
            : base(game)
        {
            screens = new List<ICompositionScreen>();
        }

        public ICollection<ICompositionScreen> Screens { get { return screens; } }

        public void AddScreen(ICompositionScreen screen)
        {
            screen.Initialize(this);
            if (isLoaded)
                screen.LoadContent();

            lock (screens)
            {
                screens.Add(screen);
            }
        }

        public void RemoveScreen(ICompositionScreen screen)
        {
            lock (screens)
            {
                screens.Remove(screen);
            }

            screen.UnloadContent();
        }

        public void LoadContent()
        {
            lock (screens)
            {
                foreach (var screen in screens)
                {
                    screen.LoadContent();
                }
            }

            font = Game.Content.Load<SpriteFont>("sprites/fonts/vcf");
            isLoaded = true;
        }

        public void Update(GameTime gameTime)
        {
            lock (screens)
            {
                foreach (var screen in screens)
                {
                    screen.Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            Game.Graphics.GraphicsDevice.SetRenderTarget(null);
            Game.Graphics.GraphicsDevice.Clear(Color.Black);

            lock (screens)
            {
                foreach (var screen in screens)
                {
                    if (screen.IsEnabled)
                    {
                        Game.Graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                        screen.Draw(gameTime);
                    }
                }

                Game.Graphics.SpriteBatch.Begin();
                Game.Graphics.SpriteBatch.DrawString(
                    font,
                    screens.Select(s => s.Name + (s.IsEnabled ? "\n" : " [disabled]\n"))
                           .Aggregate((x, y) => x + y),
                    25 * Vector2.One,
                    Color.Red);
                Game.Graphics.SpriteBatch.End();
            }
        }

        public void UnloadContent()
        {
            lock (screens)
            {
                foreach (var screen in screens)
                {
                    screen.UnloadContent();
                }
            }
        }

        private SpriteFont font;
        private List<ICompositionScreen> screens;
        private bool isLoaded = false;
    }
}
