using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AISTek.XRage.InputManagement
{
    public class MouseInputState
    {
        internal MouseInputState(InputManager inputManager)
        {
            currentMouseState = Mouse.GetState();
            this.inputManager = inputManager;
            screenHalfWidth = inputManager.Game.GraphicsDevice.Viewport.Width / 2;
            screenHalfHeigth = inputManager.Game.GraphicsDevice.Viewport.Height / 2;
        }

        public Vector2 MousePosition { get; private set; }

        public Vector2 MouseDeltaPosition { get; private set; }

        public int ScrollWheelValue { get; private set; }

        public int ScrollWheelDeltaValue { get; private set; }

        public bool IsButtonPressed(MouseButton button)
        {
            return (GetMouseButtonState(currentMouseState, button) == ButtonState.Pressed) &&
                (GetMouseButtonState(prevoiusMouseState, button) == ButtonState.Released);
        }

        public bool IsButtonHolded(MouseButton button)
        {
            return (GetMouseButtonState(currentMouseState, button) == ButtonState.Pressed) &&
                 (GetMouseButtonState(prevoiusMouseState, button) == ButtonState.Pressed);
        }

        public bool IsButtonReleased(MouseButton button)
        {
            return (GetMouseButtonState(currentMouseState, button) == ButtonState.Released) &&
                (GetMouseButtonState(prevoiusMouseState, button) == ButtonState.Pressed);
        }

        internal void Update(GameTime gameTime)
        {
            prevoiusMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            ScrollWheelValue = currentMouseState.ScrollWheelValue;
            ScrollWheelDeltaValue = currentMouseState.ScrollWheelValue - prevoiusMouseState.ScrollWheelValue;

            if (inputManager.IsMouseLocked)
            {
                MousePosition = new Vector2(screenHalfWidth, screenHalfHeigth);
                MouseDeltaPosition = inputManager.MouseSensitivity * 
                    (new Vector2(currentMouseState.X, currentMouseState.Y) - new Vector2(screenHalfWidth, screenHalfHeigth));
                Mouse.SetPosition(screenHalfWidth, screenHalfHeigth);
            }
            else
            {
                MousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
                MouseDeltaPosition = inputManager.MouseSensitivity * 
                    (MousePosition - new Vector2(prevoiusMouseState.X, prevoiusMouseState.Y));
            }
        }

        private static ButtonState GetMouseButtonState(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    return state.LeftButton;

                case MouseButton.MiddleButton:
                    return state.MiddleButton;

                case MouseButton.RightButton:
                    return state.RightButton;

                case MouseButton.X1Button:
                    return state.XButton1;

                case MouseButton.X2Button:
                    return state.XButton2;

                default:
                    throw new ArgumentException(string.Format("{0} is not a value of {1}", button, typeof(MouseButton)));
            }
        }

        private MouseState currentMouseState;
        private MouseState prevoiusMouseState;

        private readonly InputManager inputManager;
        private int screenHalfWidth;
        private int screenHalfHeigth;
    }
}
