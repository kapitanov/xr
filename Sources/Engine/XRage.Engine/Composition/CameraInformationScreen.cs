using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AISTek.XRage.Interfaces;

namespace AISTek.XRage.Composition
{
    public class CameraInformationScreen : XComponent, ICompositionScreen
    {
        public CameraInformationScreen(XGame game)
            : base(game)
        { }

        public string Name { get { return "camera_info"; } }

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
            var camera = Game.Interfaces.QueryInterface<CameraInterface>().ActiveCamera;

            Game.Graphics.SpriteBatch.Begin();
            Game.Graphics.SpriteBatch.DrawString(
                font,
                camera.GetCameraInfo(),
                new Vector2(
                    Game.Graphics.GraphicsDevice.Viewport.Width * 0.05f,
                    Game.Graphics.GraphicsDevice.Viewport.Height - 75),
                color);

            Game.Graphics.SpriteBatch.End();
        }

        public void UnloadContent()
        { }

        private SpriteFont font;
    }
}
