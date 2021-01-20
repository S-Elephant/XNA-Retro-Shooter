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
    public class SuiciderEnemy : BaseEnemy
    {
        float Velocity;

        internal SuiciderEnemy() :
            base()
        {
            Animation = BaseEnemy.Animations[2];
            Shadow = BaseEnemy.Shadows[2];
            ScoreValue = 30;

            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 0, Animation.Width, Animation.Height)));
            Location = Common.InvalidVector2;
            CollisionDamage = 15;
            ReappearsOnTopCnt = 0;
        }

        public void Initialize(Vector2 startLocation)
        {
            IsDisposed = false;
            Location = startLocation;

            Velocity = Maths.RandomNr(7, 9);
            AutoAim aa = Level.Instance.AutoAimPool.New();
            aa.Initialize(new Vector2(Animation.Width / 2 - 8, Animation.Height / 2 - 8), null);
            Guns.Add(aa);
        }

        public static SuiciderEnemy PoolConstructor()
        {
            return new SuiciderEnemy();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 direction = Maths.GetMoveDir(Location, Level.Instance.GetNearestPlayer(Location).CenterLoc);
            direction.Normalize();
            Location += direction * Velocity;

            base.Update(gameTime);
            BroadPhase.Instance.AddEntity(this, AABB);
        }
    }
}