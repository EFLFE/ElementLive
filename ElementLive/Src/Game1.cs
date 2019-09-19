using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElementLive.Src
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D pixel;
        Render render;
        Scene scene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = UIHelper.ScreenWidth,
                PreferredBackBufferHeight = UIHelper.ScreenHeight,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = true,
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            Window.TextInput += (_, __) => EFInput.TextInput = __.Character;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = false;
            IsMouseVisible = true;

            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White.PackedValue });
            font = Content.Load<SpriteFont>("UbuntuMono-R");

            render = new Render(spriteBatch, pixel, font);
            scene = new Scene();
        }

        protected override void Update(GameTime gameTime)
        {
            EFInput.Update();

            if (IsActive)
                scene.Update();

            if (EFInput.KeyWasPressed(Keys.F4))
                Exit();

            EFInput.TextInput = '\0';
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointWrap);
            scene.Draw(render);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
