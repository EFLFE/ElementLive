using Microsoft.Xna.Framework;

namespace ElementLive.Src
{
    /// <summary>
    /// UI config.
    /// </summary>
    sealed class Config
    {
        IScene scene;
        Rule rule;

        UINumeric
            sceneUpdateTick,
            ruleToAlive,
            ruleToBorn,
            dyingTimes;

        public Config(IScene scene, Rule rule)
        {
            this.scene = scene;
            this.rule = rule;

            // задержка обновления сцены
            sceneUpdateTick = new UINumeric(new Vector2(1f, UIHelper.ScreenHeight - 22f), "Delay: ", 0, 9);
            sceneUpdateTick.Toggle(scene.UpdateCounterTarget, false);
            sceneUpdateTick.SingleValue = true;
            sceneUpdateTick.OnToggle += (index, val) =>
            {
                scene.UpdateCounterTarget = index;
            };

            // правило клетке не умирать
            ruleToAlive = new UINumeric(Vector2.One, "Rule live:", 0, 8);
            ruleToAlive.LoadArray(rule.RuleToStayLive);
            ruleToAlive.OnToggle += (index, val) =>
            {
                rule.RuleToStayLive[index] = val;
            };

            // правило рождения клетки
            ruleToBorn = new UINumeric(new Vector2(328f, 1f), "Rule born:", 1, 8);
            ruleToBorn.LoadArray(rule.RuleToBorn);
            ruleToBorn.OnToggle += (index, val) =>
            {
                rule.RuleToBorn[index] = val;
            };

            // время смерти клетки
            dyingTimes = new UINumeric(new Vector2(328f, UIHelper.ScreenHeight - 22f), "Die time:", 0, 9);
            dyingTimes.Toggle(rule.DyingTimes, false);
            dyingTimes.SingleValue = true;
            dyingTimes.OnToggle += (index, val) =>
            {
                rule.DyingTimes = index;
            };
        }

        public void SyncRule()
        {
            sceneUpdateTick.AllToFalse();
            sceneUpdateTick.Toggle(scene.UpdateCounterTarget, false);

            dyingTimes.AllToFalse();
            dyingTimes.Toggle(rule.DyingTimes, false);

            ruleToAlive.LoadArray(rule.RuleToStayLive);
            ruleToBorn.LoadArray(rule.RuleToBorn);
        }

        public void Update()
        {
            sceneUpdateTick.Update();
            ruleToAlive.Update();
            ruleToBorn.Update();
            dyingTimes.Update();
        }

        public void Draw(Render render)
        {
            sceneUpdateTick.Draw(render);
            ruleToAlive.Draw(render);
            ruleToBorn.Draw(render);
            dyingTimes.Draw(render);
        }
    }
}
