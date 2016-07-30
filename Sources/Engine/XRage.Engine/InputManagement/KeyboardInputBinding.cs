using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace AISTek.XRage.InputManagement
{
    public class KeyboardInputBinding : InputBinding
    {
        public KeyboardInputBinding(Keys key, ActionType actionType)
        {
            Key = key;
            ActionType = actionType;
        }

        public Keys Key { get; private set; }

        public ActionType ActionType { get; private set; }

        public override bool EvaluateBinding(InputState inputState)
        {
            var isPressed = new Lazy<bool>(() => inputState.Keyboard.IsKeyPressed(Key));
            var isReleased = new Lazy<bool>(() => inputState.Keyboard.IsKeyReleased(Key));
            var isHolded = new Lazy<bool>(() => inputState.Keyboard.IsKeyHolded(Key));

            switch(ActionType)
            {
                case ActionType.Press:
                    return isPressed.Value;

                case ActionType.Release:
                    return isReleased.Value;

                case ActionType.Hold:
                    return isPressed.Value || 
                           isHolded.Value;

                default: 
                    return false;
            }
            
        }
    }
}
