using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ElementLive.Src
{
    sealed class Render
    {
        SpriteBatch sb;
        Texture2D pixel;
        SpriteFont font;

        public Render(SpriteBatch sb, Texture2D pixel, SpriteFont font)
        {
            this.sb = sb;
            this.pixel = pixel;
            this.font = font;
        }

        public Vector2 TextSize(string text) => font.MeasureString(text);

        public void DrawPixel(Vector2 pos, Color clr, float size = 1f)
        {
            sb.Draw(pixel, pos, null, clr, 0f, Vector2.Zero, new Vector2(size), 0, 0f);
        }

        public void DrawText(string text, Vector2 pos, Color clr, float scale = 1f)
        {
            sb.DrawString(font, text, pos, clr, 0f, Vector2.Zero, new Vector2(scale), 0, 0f);
        }
    }
}
