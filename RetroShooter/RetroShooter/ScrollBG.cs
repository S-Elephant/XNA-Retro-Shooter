using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace RetroShooter
{
    public struct ScrollBG
    {
        Texture2D Texture;
        Vector2 Location1;
        Vector2 Location2;
        public const float ScrollSpeed = 15;

        public ScrollBG(string texture)
        {
            Texture = Common.str2Tex("BG/"+texture);
            Location1 = new Vector2(0, -Texture.Height);
            Location2 = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            float increment = ScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Location1.Y += increment;
            if (Location1.Y >= Engine.Instance.Height)
            {
                Location1.Y = Location2.Y - Texture.Height;
                Location1.Y += increment;
            }

            Location2.Y += increment;
            if (Location2.Y >= Engine.Instance.Height)
            {
                Location2.Y = Location1.Y - Texture.Height;
                Location2.Y += increment;
            }
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Texture, Location1, Color.White);
            Engine.Instance.SpriteBatch.Draw(Texture, Location2, Color.White);
        }
    }
}
