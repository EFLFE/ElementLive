using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ElementLive.Src
{
    enum MouseButtonEnum
    {
        LeftButton,
        MiddleButton,
        RightButton,
        //XButton1,
        //XButton2,
    }

    /// <summary> Статически класс для получения информации по клавиатуе и мыши. </summary>
    static class EFInput
    {
        private static Rectangle rect = new Rectangle(-2, -2, 1, 1);
        private static RectangleF rectf = new RectangleF(-2f, -2f, 1f, 1f);

        /// <summary> Получить квадрат коллизии курсора. </summary>
        public static Rectangle GetCursorRect => rect;

        public static RectangleF GetCursorRectF => rectf;

#if true //WINDOWS
        private static MouseState newMouse, oldMouse;
        private static KeyboardState newKeyboard, oldKeyboard;
        private static bool anyKeyDown, shiftDown;

        public enum MouseCursorEnum
        {
            Normal,
            Sell,
        }

        /// <summary> Поулчить состояние мыши. </summary>
        public static MouseState GetMouseState => newMouse;

        /// <summary> Получить состояние клавиатуры. </summary>
        public static KeyboardState GetKeyboardState => newKeyboard;

        /// <summary> true - отключить обработку мыши. </summary>
        public static bool DisableMouse = false;

        private static bool blockNextInput;

        public static bool InputIsBlocked => blockNextInput;

        /// <summary>
        /// Блокировать получение данных на текущий цикл.
        /// </summary>
        public static void BlockNextInput()
        {
            blockNextInput = true;
        }

        /// <summary>
        /// Разблокировать получение данных на текущий цикл.
        /// </summary>
        public static void UnblockNextInput()
        {
            blockNextInput = false;
        }

        // делегат обытия
        public delegate void OnKeyPressedDelegate(Keys k);

        /// <summary>
        /// событие нажания на кнопку (однажды)
        /// </summary>
        public static event OnKeyPressedDelegate OnKeyPressed;

        // нажатые в настоящий момент клавишы
        private static Keys[] presseds, oldPresseds;

        // список новых нажатых клавиш
        private static List<Keys> pressedKeysList = new List<Keys>(12);

        private static int currentScrollWheel;

        public static bool IsScrollWheelUp { get; private set; }

        public static bool IsScrollWheelDown { get; private set; }

        static EFInput()
        {
            OnKeyPressed += GInput_OnKeyPressed;
        }

        static void GInput_OnKeyPressed(Keys k)
        {
            pressedKeysList.Add(k);
        }

        /// <summary> 0бновление. </summary>
        public static void Update()
        {
            blockNextInput = false;
            if (!DisableMouse)
            {
                oldMouse = newMouse;
                newMouse = Mouse.GetState();

                rect.X = newMouse.X;
                rect.Y = newMouse.Y;
                rectf.X = newMouse.X;
                rectf.Y = newMouse.Y;

                currentScrollWheel = newMouse.ScrollWheelValue;
                IsScrollWheelUp = false;
                IsScrollWheelDown = false;
                if (currentScrollWheel > oldMouse.ScrollWheelValue)
                {
                    IsScrollWheelUp = true;
                }
                else if (currentScrollWheel < oldMouse.ScrollWheelValue)
                {
                    IsScrollWheelDown = true;
                }
            }

            oldKeyboard = newKeyboard;
            newKeyboard = Keyboard.GetState();

            presseds = newKeyboard.GetPressedKeys();
            oldPresseds = oldKeyboard.GetPressedKeys();

            anyKeyDown = presseds.Length > 0;
            shiftDown = presseds.Contains(Keys.LeftShift);

            pressedKeysList.Clear();

            for (int i = 0; i < presseds.Length; i++)
            {
                if (!oldPresseds.Contains(presseds[i]))
                {
                    // БЕЗУМИЕ!!!
                    if (presseds[i] != Keys.LeftShift && presseds[i] != Keys.RightShift &&
                        presseds[i] != Keys.LeftAlt &&
                        presseds[i] != Keys.RightAlt && presseds[i] != Keys.LeftControl &&
                        presseds[i] != Keys.RightControl && presseds[i] != Keys.Up && presseds[i] != Keys.Down &&
                        presseds[i] != Keys.Left && presseds[i] != Keys.Right && presseds[i] != Keys.Tab &&
                        presseds[i] != Keys.CapsLock && presseds[i] != Keys.NumLock && presseds[i] != Keys.Delete)
                        OnKeyPressed?.Invoke(presseds[i]);
                }
            }
        }

        /// <summary> Касание квадрата с курсором. </summary>
        public static bool CollCursor(Rectangle rect1)
        {
            if (blockNextInput)
                return false;
            return !CursorOut && rect.Intersects(rect1);
        }

        /// <summary> Касание квадрата с курсором. </summary>
        public static bool CollCursor(int x, int y, int w, int h)
        {
            if (blockNextInput)
                return false;
            return !CursorOut && rect.Intersects(new Rectangle(x, y, w, h));
        }

        /// <summary> Кнопка была нажана. </summary>
        /// <param name="key">Символ кнопки.</param>
        public static bool KeyWasPressed(Keys key)
        {
            if (blockNextInput)
                return false;
            return (newKeyboard.IsKeyDown(key) && oldKeyboard.IsKeyUp(key));
        }

        /// <summary> Любая кнопка была нажана. </summary>
        public static bool KeyWasPressed()
        {
            if (blockNextInput)
                return false;
            return (newKeyboard.GetPressedKeys().Length > 0 && oldKeyboard.GetPressedKeys().Length == 0);
        }

        /// <summary> Кнопка в нажатом состоянии. </summary>
        /// <param name="key">Символ кнопки.</param>
        public static bool KeyIsPressed(Keys key)
        {
            if (blockNextInput)
                return false;
            return newKeyboard.IsKeyDown(key);
        }

        /// <summary> Любая кнопка в нажатом состоянии. </summary>
        public static bool KeyIsPressed()
        {
            if (blockNextInput)
                return false;
            return newKeyboard.GetPressedKeys().Length > 0;
        }

        /// <summary> Получить нажатую кнопку. </summary>
        public static Keys GetPressedKey()
        {
            if (blockNextInput)
                return Keys.None;
            return !KeyWasPressed() ? Keys.None : newKeyboard.GetPressedKeys()[0];
        }

        /// <summary> Кнопка мыши была нажата. </summary>
        /// <param name="right">false - левая, true - правая.</param>
        public static bool MouseWasPressed(MouseButtonEnum mouseButton = MouseButtonEnum.LeftButton)
        {
            if (CursorOut || blockNextInput)
                return false;

            switch (mouseButton)
            {
                case MouseButtonEnum.LeftButton:
                    return newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released;

                case MouseButtonEnum.MiddleButton:
                    return newMouse.MiddleButton == ButtonState.Pressed && oldMouse.MiddleButton == ButtonState.Released;

                case MouseButtonEnum.RightButton:
                    return newMouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released;

                default:
                    throw new Exception("Missing '" + mouseButton.ToString() + "' to input.");
            }
        }

        /// <summary> Кнопка мыши в нажатом состоянии. </summary>
        /// <param name="right">false - левая, true - правая.</param>
        public static bool MouseIsPressed(MouseButtonEnum mouseButton = MouseButtonEnum.LeftButton)
        {
            if (CursorOut || blockNextInput)
                return false;

            switch (mouseButton)
            {
                case MouseButtonEnum.LeftButton:
                    return newMouse.LeftButton == ButtonState.Pressed;

                case MouseButtonEnum.MiddleButton:
                    return newMouse.MiddleButton == ButtonState.Pressed;

                case MouseButtonEnum.RightButton:
                    return newMouse.RightButton == ButtonState.Pressed;

                default:
                    throw new Exception("Missing '" + mouseButton.ToString() + "' to input.");
            }
        }

        /// <summary> Кнопка мыши была отжата. </summary>
        /// <param name="right">false - левая, true - правая.</param>
        public static bool MouseWasReleased(MouseButtonEnum mouseButton = MouseButtonEnum.LeftButton)
        {
            if (CursorOut || blockNextInput)
                return false;

            switch (mouseButton)
            {
                case MouseButtonEnum.LeftButton:
                    return newMouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed;

                case MouseButtonEnum.MiddleButton:
                    return newMouse.MiddleButton == ButtonState.Released && oldMouse.MiddleButton == ButtonState.Pressed;

                case MouseButtonEnum.RightButton:
                    return newMouse.RightButton == ButtonState.Released && oldMouse.RightButton == ButtonState.Pressed;

                default:
                    throw new Exception("Missing '" + mouseButton.ToString() + "' to input.");
            }
        }

        /// <summary> true - если курсор все окна игры. </summary>
        public static bool CursorOut =>
            newMouse.X < 0
            || newMouse.Y < 0
            || newMouse.X > UIHelper.ScreenWidth
            || newMouse.Y > UIHelper.ScreenHeight;

        //Point mouse
        public static Point GetMousePoint => newMouse.Position;

        //Point mouse in vector
        public static Vector2 GetMouseVector => new Vector2(newMouse.X, newMouse.Y);

        private static char charInput = '\0';

        /// <summary> Получить/изменить символ нажатой клавиши. </summary>
        public static char TextInput
        {
            get
            {
                //if (charInput == '\0')
                //    return '\0';
                //
                //var rc = charInput;
                //charInput = '\0';
                //return rc;
                if (blockNextInput)
                    return '\0';
                return charInput;
            }
            set { charInput = value; }
        }

#elif ANDROID
        /// <summary> true - отключить обработку мыши. </summary>
        public static bool DisableMouse = true;

        public static TouchCollection Touches;

        public static Vector2 TouchPos;

        public static bool AnyIsPressed => Touches.Count > 0;
        public static bool AnyWasPressed => Touches.Count > 0 && !oldWasPressed;
        public static bool AnyWasReressed => Touches.Count == 0 && oldWasPressed;

        private static bool wasPressed, oldWasPressed;

        public static void Update()
        {
            Touches = TouchPanel.GetState();
            oldWasPressed = wasPressed;

            if (Touches.Count > 0)
            {
                TouchPos = Touches[0].Position;
                rect.X = (int)Touches[0].Position.X;
                rect.Y = (int)Touches[0].Position.Y;
                wasPressed = true;
            }
            else
            {
                wasPressed = false;
            }

            //(newMouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed);

        }
#endif
    }
}
