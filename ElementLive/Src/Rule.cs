using Microsoft.Xna.Framework;

namespace ElementLive.Src
{
    /// <summary>
    /// Правило жизни клеток.
    /// </summary>
    sealed class Rule
    {
        const int RULE_ARRAY_LEN = 9;

        /// <summary> Размер клетки в пикселях. </summary>
        public int PixelSize { get; private set; }

        public int MapWidth { get; private set; }

        public int MapHeight { get; private set; }

        /// <summary> Цвет живой клетки. </summary>
        public Color LiveColor;

        /// <summary> Цвет умирающей клетки. </summary>
        public Color DyingColor;

        /// <summary> Правило выживания клетки(stay). </summary>
        public bool[] RuleToStayLive;

        /// <summary> Правило рождения клетки (born). </summary>
        public bool[] RuleToBorn;

        bool[] ruleToStayLiveUndo, ruleToBornUndo;

        /// <summary> Время смерти клетки. Умирающая клетка не считает живой. </summary>
        public int DyingTimes;

        public Rule(int pixelSize)
        {
            PixelSize = pixelSize;
            MapWidth = UIHelper.ScreenWidth / PixelSize;
            MapHeight = UIHelper.ScreenHeight / PixelSize;
            LiveColor = Color.Lime;
            DyingColor = Color.Green;
            DyingTimes = 0;
            SetupGOL();
        }

        /// <summary>
        /// Правило GOL (S:23/B:3).
        /// </summary>
        void SetupGOL()
        {
            RuleToStayLive = new bool[RULE_ARRAY_LEN];
            RuleToStayLive[2] = true;
            RuleToStayLive[3] = true;

            RuleToBorn = new bool[RULE_ARRAY_LEN];
            RuleToBorn[3] = true;
        }

        public void CreateUndo()
        {
            if (ruleToStayLiveUndo == null)
            {
                ruleToStayLiveUndo = new bool[RULE_ARRAY_LEN];
                ruleToBornUndo     = new bool[RULE_ARRAY_LEN];
            }

            for (int i = 0; i < RULE_ARRAY_LEN; i++)
            {
                ruleToBornUndo[i] = RuleToStayLive[i];
                ruleToBornUndo[i] = RuleToBorn[i];
            }
        }

        public void DoUndo()
        {
            if (ruleToStayLiveUndo != null)
            {
                for (int i = 0; i < RULE_ARRAY_LEN; i++)
                {
                    RuleToStayLive[i] = ruleToBornUndo[i];
                    RuleToBorn[i] = ruleToBornUndo[i];
                }
            }
        }

    }
}
