using Microsoft.Xna.Framework;
using System;

namespace ElementLive.Src
{
    /// <summary> Интерфейс с цифрами для переключения правил. </summary>
    sealed class UINumeric
    {
        const float letterWidth = 18;

        Vector2 drawPos;
        string title;
        int from, to;
        bool[] flags;

        float titleWidth;

        public Action<int, bool> OnToggle;

        public bool SingleValue;

        /// <summary>
        /// Интерфейс с цифрами для переключения правил.
        /// </summary>
        public UINumeric(Vector2 drawPos, string title, int from, int to)
        {
            this.drawPos = drawPos;
            this.title = title;
            this.from = from;
            this.to = to + 1;
            flags = new bool[this.to - from];
        }

        /// <summary>
        /// Применить данные из массива (правила).
        /// </summary>
        public void LoadArray(bool[] array)
        {
            for (int i = 0; i < flags.Length && i < array.Length; i++)
            {
                flags[i] = array[i];
            }
        }

        /// <summary>
        /// Сброс.
        /// </summary>
        public void AllToFalse()
        {
            for (int i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }
        }

        /// <summary>
        /// Переключить значение.
        /// </summary>
        /// <param name="index">Число, которое нужно переключить (не индекс).</param>
        /// <param name="raiseEvent">Вызвать событие OnToggle.</param>
        public void Toggle(int index, bool raiseEvent)
        {
            int i = index - from;
            flags[i] = !flags[i];

            if (raiseEvent)
                OnToggle?.Invoke(index, flags[i]);
        }

        public void Update()
        {
            Vector2 pos = drawPos;
            pos.X += titleWidth;

            for (int i = 0; i < flags.Length; i++)
            {
                if (EFInput.MouseWasPressed(MouseButtonEnum.RightButton) &&
                    EFInput.GetCursorRectF.Intersects(new RectangleF(pos.X, pos.Y, letterWidth, letterWidth)))
                {
                    // click
                    if (SingleValue)
                        AllToFalse();
                    Toggle(i + from, true);
                }

                pos.X += letterWidth;
            }
        }

        public void Draw(Render render)
        {
            if (titleWidth == 0f)
            {
                titleWidth = render.TextSize(title).X;
            }

            Vector2 pos = drawPos;
            render.DrawText(title, pos, Color.Gray);

            pos.X += titleWidth;

            for (int i = 0; i < flags.Length; i++)
            {
                render.DrawText((i + from).ToString(), pos, flags[i] ? Color.White : Color.Gray);
                pos.X += letterWidth;
            }
        }

    }
}
