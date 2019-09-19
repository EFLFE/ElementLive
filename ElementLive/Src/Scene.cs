using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;

namespace ElementLive.Src
{
    interface IScene
    {
        int UpdateCounterTarget { get; set; }
    }

    sealed class Scene : IScene
    {
        Rule rule;
        Live[,] lives, livesToUndo;
        bool pause;
        Counter updateCounter;
        Config config;
        Stopwatch sw;
        bool showHelp = true;

        public int UpdateCounterTarget
        {
            get => updateCounter.TargetTick;
            set => updateCounter.TargetTick = value;
        }

        public Scene()
        {
            updateCounter = new Counter(4); // cap: 9
            pause = true;
            rule = new Rule(pixelSize: 12);
            lives = CreateLiveArray();
            config = new Config(this, rule);
            sw = new Stopwatch();
        }

        Live[,] CreateLiveArray()
        {
            var liveArray = new Live[rule.MapHeight, rule.MapWidth];
            // create
            for (int y = 0; y < rule.MapHeight; y++)
                for (int x = 0; x < rule.MapWidth; x++)
                    liveArray[y, x] = new Live(x, y, rule);

            // post init
            for (int y = 0; y < rule.MapHeight; y++)
                for (int x = 0; x < rule.MapWidth; x++)
                    liveArray[y, x].PostInit(liveArray);

            return liveArray;
        }

        public void Update()
        {
            if (showHelp && (EFInput.KeyWasPressed() || EFInput.MouseWasPressed(MouseButtonEnum.LeftButton)))
            {
                showHelp = false;
            }

            // create undo
            if (EFInput.MouseWasPressed(MouseButtonEnum.LeftButton) || EFInput.MouseWasPressed(MouseButtonEnum.MiddleButton))
            {
                CreateUndo();
            }

            if (EFInput.KeyIsPressed(Keys.LeftControl) && EFInput.KeyWasPressed(Keys.Z))
                DoUndo();

            Point mp = EFInput.GetMousePoint;

            if (rule.PixelSize > 1)
                mp /= new Point(rule.PixelSize);

            if (mp.X >= 0 && mp.Y >= 0 && mp.X < rule.MapWidth && mp.Y < rule.MapHeight)
            {
                // paint
                if (EFInput.MouseIsPressed(MouseButtonEnum.LeftButton))
                {
                    lives[mp.Y, mp.X].Alive();
                }
                else if (EFInput.MouseIsPressed(MouseButtonEnum.MiddleButton))
                {
                    if (mp.X > 0 && mp.Y > 0 && mp.X < rule.MapWidth - 1 && mp.Y < rule.MapHeight - 1)
                    {
                        lives[mp.Y - 1, mp.X].Alive();
                        lives[mp.Y, mp.X - 1].Alive();
                        lives[mp.Y, mp.X].Alive();
                        lives[mp.Y + 1, mp.X].Alive();
                        lives[mp.Y, mp.X + 1].Alive();
                    }
                }
            }

            config.Update();

            if (EFInput.KeyWasPressed(Keys.Space))
                pause = !pause;

            // step
            if (EFInput.KeyWasPressed(Keys.S))
            {
                UpdateLive();
                pause = true;
            }

            if (!pause && updateCounter.Tick())
            {
                UpdateLive();
            }

            if (EFInput.KeyWasPressed(Keys.C))
            {
                // clear
                for (int y = 0; y < rule.MapHeight; y++)
                    for (int x = 0; x < rule.MapWidth; x++)
                        lives[y, x].Kill();
            }
        }

        // сохранить жизнь, что бы потом можно было отменить
        void CreateUndo()
        {
            // live data
            if (livesToUndo == null)
                livesToUndo = CreateLiveArray();

            for (int y = 0; y < rule.MapHeight; y++)
            {
                for (int x = 0; x < rule.MapWidth; x++)
                {
                    lives[y, x].CopyTo(livesToUndo[y, x]);
                }
            }

            // rule
            rule.CreateUndo();
        }

        void DoUndo()
        {
            if (livesToUndo == null)
                return;

            // live data
            for (int y = 0; y < rule.MapHeight; y++)
            {
                for (int x = 0; x < rule.MapWidth; x++)
                {
                    livesToUndo[y, x].CopyTo(lives[y, x]);
                }
            }

            // rule
            rule.DoUndo();
        }

        void UpdateLive()
        {
            sw.Restart();

            for (int y = 0; y < rule.MapHeight; y++)
                for (int x = 0; x < rule.MapWidth; x++)
                    lives[y, x].PreUpdate();

            for (int y = 0; y < rule.MapHeight; y++)
                for (int x = 0; x < rule.MapWidth; x++)
                    lives[y, x].PostUpdate();

            sw.Stop();
        }

        public void Draw(Render render)
        {
            // draw live
            for (int y = 0; y < rule.MapHeight; y++)
            {
                for (int x = 0; x < rule.MapWidth; x++)
                {
                    var live = lives[y, x];
                    switch (live.Status)
                    {
                        case LiveStatusEnum.Alive:
                            render.DrawPixel(live.Pos, rule.LiveColor, rule.PixelSize);
                            break;

                        case LiveStatusEnum.Dying:
                            render.DrawPixel(live.Pos, rule.DyingColor, rule.PixelSize);
                            break;
                    }
                }
            }

            config.Draw(render);

            int elapsedTime = (int)(sw.ElapsedTicks * 0.01f);
            render.DrawText(
                elapsedTime.ToString(),
                new Vector2(UIHelper.ScreenWidth - 96f, UIHelper.ScreenHeight - 20f),
                elapsedTime > 99 ? Color.Red : Color.Gray);

            if (showHelp)
            {
                render.DrawText(
                    "ЛКМ - точка\nСКМ - 5 точек\nS - шаг\nScape - пауза\nC - очистить\nПКМ - клик по упр. цифрам\nCtrl+Z - Undo",
                    new Vector2(96f), Color.White);
            }

            if (pause)
            {
                render.DrawText("||", new Vector2(1f, UIHelper.ScreenHeight - 48f), Color.White);
            }

        }

    }
}
