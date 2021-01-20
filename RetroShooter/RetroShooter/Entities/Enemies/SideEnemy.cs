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
    public class SideEnemy : BaseEnemy
    {
        const float Velocity = 2.5f;
        int side;

        internal SideEnemy()
        {
            Animation = BaseEnemy.Animations[7];
            Shadow = BaseEnemy.Shadows[7];
            ScoreValue = 45;

            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 0, Animation.Width, Animation.Height)));
            Location = Common.InvalidVector2;
            CollisionDamage = 40;

            HP.MaxHP = 45;
            HP.CurrentHP = 45;
            ReappearsOnTopCnt = 0;
        }

        public void Initialize()
        {
            IsDisposed = false;

            if (Maths.Chance(50))
            {
                side = 1; // going right
                Location = new Vector2(Maths.RandomNr(-200, -130), Maths.RandomNr(32, 650));
            }
            else
            {
                side = -1; // going left
                Location = new Vector2(Maths.RandomNr(1280, 1400), Maths.RandomNr(32, 650));
                SpriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public static SideEnemy PoolConstructor()
        {
            return new SideEnemy();
        }

        public override void Update(GameTime gameTime)
        {
            Location += new Vector2(Velocity * side, ScrollBG.ScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if ((side == 1 && Location.X > 1280) || (side == -1 && Location.X < -AABB.Width))
                Dispose();

            base.Update(gameTime);
            BroadPhase.Instance.AddEntity(this, AABB);
        }
    }
}