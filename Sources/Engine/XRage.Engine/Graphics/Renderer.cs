using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AISTek.XRage.Graphics
{
    public abstract class Renderer : XComponent
    {
        protected Renderer(XGame game)
            : base(game)
        {
            Graphics = game.Graphics;
        }

        public bool IsDebugVisualMode { get; set; }
        
        public GraphicsSystem Graphics { get; private set; }

        public GraphicsDevice GraphicsDevice { get { return Graphics.GraphicsDevice; } }

        public virtual void Initialize()
        { }

        public virtual void SaveSettings()
        { }

        public abstract void DrawFrame(ICamera camera, GameTime gameTime);
    }
}
