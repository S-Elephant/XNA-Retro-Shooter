using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace RetroShooter
{
    public class ScrapStation : BaseShip
    {
        float VelocityX;
        float VelocityY;
        public bool IsGoingHome;
        static readonly SpriteFont Font = Common.str2Font("Font04_24");
        StringBuilder OpenSB = new StringBuilder("Shop Open!", 10);
        StringBuilder ClosedSB = new StringBuilder("Thank You!", 10);
        static readonly Vector2 TextOffset = new Vector2(-10, 64);

        public ScrapStation()
        {
            Animation = Common.str2Tex("Ships/scrapStation");
            Shadow = Common.str2Tex("Shadows/scrapStation");

            AABB = new Rectangle(0, 0, Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 39, 128, 95)));
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(54, 134, 74, 34)));
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(36, 10, 58, 29)));
        }

        public void Initialize(Vector2 location)
        {
            IsDisposed = false;
            Location = location;
            IsGoingHome = false;
            VelocityX = 0;
            VelocityY = 0.9f;
        }
        
        public static ScrapStation PoolConstructor()
        {
            return new ScrapStation();
        }

        public void Visit(PlayerShip pShip)
        {
            if(Location.X < Engine.Instance.Width/2)
                VelocityX = -1.5f;
            else
                VelocityX = 1.5f;

            VelocityY = 9f;

            Engine.Instance.ActiveState = new Shop(pShip.Owner);
            Shop.IsFirstVisit = false;
            IsGoingHome = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Location += new Vector2(VelocityX, VelocityY);
            if (Location.Y > Engine.Instance.Height)
                IsDisposed = true;
        }

        public override void Draw()
        {
            base.Draw();
            if(!IsGoingHome)
                Engine.Instance.SpriteBatch.DrawString(Font, OpenSB, Location + TextOffset, Color.White);
            else
                Engine.Instance.SpriteBatch.DrawString(Font, ClosedSB, Location + TextOffset, Color.White);
        }
    }
}
