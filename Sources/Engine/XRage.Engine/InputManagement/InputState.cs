using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.InputManagement
{
    public class InputState
    {
        internal InputState(InputManager inputManager)
        {
            Keyboard = new KeyboardInputState();
            Mouse = new MouseInputState(inputManager);
        }

        public KeyboardInputState Keyboard { get; private set; }

        public MouseInputState Mouse { get; private set; }

        internal void Update(GameTime gameTime)
        {
            Keyboard.Update(gameTime);
            Mouse.Update(gameTime);
        }
    }
}
