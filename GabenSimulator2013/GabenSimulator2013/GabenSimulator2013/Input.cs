using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GabenSimulator2013
{
    //delegate MouseClickDelegate;
    class MouseEventArgs : EventArgs
    {
        public Vector2 Position;
        public MouseState MouseState;
    }
    static class Input
    {
        private static int MaxInputs = 4;

        private static KeyboardState oldKeyboardState, newKeyboardState;
        private static GamePadState[] oldGamePadStates, newGamePadStates;

        private static MouseState oldMouseState, newMouseState;

        public delegate void MouseClickDelegate(object sender, MouseEventArgs e);
        public static event MouseClickDelegate LeftClick;
        public static event MouseClickDelegate RightClick;

        static Input()
        {
            oldGamePadStates = new GamePadState[MaxInputs];
            newGamePadStates = new GamePadState[MaxInputs];
            //oldKeyboardState = new KeyboardState[MaxInputs];
            //newKeyboardState = new KeyboardState[MaxInputs];
        }

        public static void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                oldGamePadStates[i] = newGamePadStates[i];


                newGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

            }
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();

            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();

            if (IsLeftClicked())
            {
                if (LeftClick != null)
                    LeftClick(null, new MouseEventArgs()
                    {
                        Position = MousePosition,
                        MouseState = newMouseState
                    }
                    );
            }
            if (IsRightClicked())
            {
                if (RightClick != null)
                    RightClick(null, new MouseEventArgs()
                    {
                        Position = MousePosition,
                        MouseState = newMouseState
                    }
                    );
            }
            //oldMouseState.
        }

        public static bool IsKeyDown(Keys key)
        {
            return newKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return oldKeyboardState.IsKeyUp(key) && newKeyboardState.IsKeyDown(key);
        }

        public static bool IsButtonDown(Buttons button, PlayerIndex index)
        {
            int i = (int)index;
            return newGamePadStates[i].IsButtonDown(button);
        }

        public static bool IsButtonPressed(Buttons button, PlayerIndex index)
        {
            int i = (int)index;
            return oldGamePadStates[i].IsButtonUp(button) && newGamePadStates[i].IsButtonDown(button);
        }

        public static Vector2 GetLeftStick(PlayerIndex index)
        {
            return newGamePadStates[(int)index].ThumbSticks.Left;
        }
        public static Vector2 GetRightStick(PlayerIndex index)
        {
            return newGamePadStates[(int)index].ThumbSticks.Right;
        }

        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(newMouseState.X, newMouseState.Y);
            }
            set
            {
                Mouse.SetPosition((int)value.X, (int)value.Y);
            }
        }

        public static Vector2 GetMousePos(Matrix transform)
        {
            return Vector2.Transform(MousePosition, transform);
        }

        public static bool IsLeftDown()
        {
            return newMouseState.LeftButton == ButtonState.Pressed;
        }
        public static bool IsLeftClicked()
        {
            return (newMouseState.LeftButton == ButtonState.Pressed) && (oldMouseState.LeftButton == ButtonState.Released);
        }
        public static bool IsRightDown()
        {
            return newMouseState.RightButton == ButtonState.Pressed;
        }
        public static bool IsRightClicked()
        {
            return (newMouseState.RightButton == ButtonState.Pressed) && (oldMouseState.RightButton == ButtonState.Released);
        }

        public static int MouseWheelValue
        {
            get
            {
                return newMouseState.ScrollWheelValue;
            }
        }
    }
}
