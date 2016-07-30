using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Composition
{
    public interface ICompositionScreen
    {
        string Name { get; }
        bool IsEnabled { get; }

        void Initialize(VisualComposer visualComposer);
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void UnloadContent();
    }
}
