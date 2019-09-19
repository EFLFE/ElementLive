using Microsoft.Xna.Framework;
using System;

namespace ElementLive.Src
{
    enum LiveStatusEnum
    {
        Alive,
        Dying,
        Dead,
    }

    sealed class Live
    {
        int x;
        int y;

        public Vector2 Pos;
        public LiveStatusEnum Status;
        public int DyingTimes;

        /// <summary> 1 = клетка живая. 0 = мёртвая/умирающая. Быстрый подсчёт. </summary>
        int isAliveGetOne;

        Live[,] lives;
        Rule rule;

        // соседние клетки (H,W) (кэш) (оптимизация)
        Live live_UpLeft;
        Live live_UpMid;
        Live live_UpRight;
        Live live_MidLeft;
        Live live_MidRight;
        Live live_BottomLeft;
        Live live_BottomMid;
        Live live_BottomRight;

        /// <summary> кол живых клеток вокруг </summary>
        int liveAround;

        public Live(int x, int y, Rule rule)
        {
            this.x = x;
            this.y = y;
            this.rule = rule;

            Status = LiveStatusEnum.Dead;
            DyingTimes = 0;
            Pos = new Vector2(x * rule.PixelSize, y * rule.PixelSize);
        }

        /// <summary> Кэширование. </summary>
        public void PostInit(Live[,] lives)
        {
            this.lives = lives;

            live_UpLeft      = lives[WY(y - 1), WX(x - 1)];
            live_UpMid       = lives[WY(y - 1), x];
            live_UpRight     = lives[WY(y - 1), WX(x + 1)];
            live_MidLeft     = lives[y, WX(x - 1)];
            live_MidRight    = lives[y, WX(x + 1)];
            live_BottomLeft  = lives[WY(y + 1), WX(x - 1)];
            live_BottomMid   = lives[WY(y + 1), x];
            live_BottomRight = lives[WY(y + 1), WX(x + 1)];
        }

        // wrap pos X
        int WX(int x)
        {
            if (x == -1)
                return rule.MapWidth - 1;
            if (x == rule.MapWidth)
                return 0;
            return x;
        }

        // wrap pos Y
        int WY(int y)
        {
            if (y == -1)
                return rule.MapHeight - 1;
            if (y == rule.MapHeight)
                return 0;
            return y;
        }

        public void PreUpdate()
        {
            liveAround =
                live_UpLeft.isAliveGetOne      +
                live_UpMid.isAliveGetOne       +
                live_UpRight.isAliveGetOne     +
                live_MidLeft.isAliveGetOne     +
                live_MidRight.isAliveGetOne    +
                live_BottomLeft.isAliveGetOne  +
                live_BottomMid.isAliveGetOne   +
                live_BottomRight.isAliveGetOne;
        }

        /// <summary>
        /// Убить клетку.
        /// </summary>
        public void Kill()
        {
            isAliveGetOne = 0;
            Status = LiveStatusEnum.Dead;
        }

        public void PostUpdate()
        {
            // ex GOL: 23/3

            switch (Status)
            {
                case LiveStatusEnum.Alive:
                    // ex: Eсли у живой клетки есть N живые соседки, то эта клетка продолжает жить;
                    // В противном случае клетка умирает

                    if (!rule.RuleToStayLive[liveAround])
                    {
                        Status = DyingTimes == 0 ? LiveStatusEnum.Dead : LiveStatusEnum.Dying;
                        isAliveGetOne = 0;
                    }
                    break;

                case LiveStatusEnum.Dying:
                    if (--DyingTimes <= 0)
                    {
                        Status = LiveStatusEnum.Dead;
                        isAliveGetOne = 0;
                    }
                    break;

                case LiveStatusEnum.Dead:
                    // В пустой (мёртвой) клетке, рядом с которой N живые клетки, зарождается жизнь;

                    if (rule.RuleToBorn[liveAround]) // any flags
                    {
                        Alive();
                    }
                    break;
            }
        }

        public void Alive()
        {
            Status = LiveStatusEnum.Alive;
            DyingTimes = rule.DyingTimes;
            isAliveGetOne = 1;
        }

    }
}
