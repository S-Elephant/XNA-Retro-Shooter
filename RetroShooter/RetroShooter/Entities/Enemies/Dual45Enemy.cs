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
    public class Dual45Enemy : BaseEnemy
    {
        const float Velocity = 2.4f;

        internal Dual45Enemy() :
            base()
        {
            Animation = BaseEnemy.Animations[6];
            Shadow = BaseEnemy.Shadows[6];
            ScoreValue = 45;

            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 0, Animation.Width, Animation.Height)));
            Location = Common.InvalidVector2;
            CollisionDamage = 15;
            ReappearsOnTopCnt = 1;
        }

        public void Initialize(Vector2 startLoc)
        {
            IsDisposed = false;
            Location = startLoc;

            DualMissile45 d45 = Level.Instance.DualMissile45Pool.New();
            d45.Initialize(new Vector2(10,31), null);
            Guns.Add(d45);
        }

        public static Dual45Enemy PoolConstructor()
        {
            return new Dual45Enemy();
        }

        public override void Update(GameTime gameTime)
        {
            Location += new Vector2(0, Velocity);

            base.Update(gameTime);
            BroadPhase.Instance.AddEntity(this, AABB);
        }
    }
}