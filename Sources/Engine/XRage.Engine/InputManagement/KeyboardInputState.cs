using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.InputManagement
{
    public class KeyboardInputState
    {
        internal KeyboardInputState()
        {
            currentKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) &&
                previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyHolded(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) &&
                previousKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) &&
                 previousKeyboardState.IsKeyDown(key);
        }

        internal void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
    }
}
