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
    public class BombardEnemy : BaseEnemy
    {
        const float Velocity = 2.2f;

        internal BombardEnemy()
        {
            Animation = BaseEnemy.Animations[5];
            Shadow = BaseEnemy.Shadows[5];
            ScoreValue = 60;

            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 0, Animation.Width, Animation.Height)));
            Location = Common.InvalidVector2;
            CollisionDamage = 30;

            HP.MaxHP = 60;
            HP.CurrentHP = 60;
            
            ReappearsOnTopCnt = 0;
        }

        public void Initialize(Vector2 startLocation)
        {
            IsDisposed = false;
            Location = startLocation;

            DualMissile45 d45 = Level.Instance.DualMissile45Pool.New();
            d45.Initialize(new Vector2(20, 30), null);
            Guns.Add(d45);

            Missile ms = Level.Instance.MissilePool.New();
            ms.Initialize(new Vector2(21, 40), null);
            Guns.Add(ms);
        }

        public static BombardEnemy PoolConstructor()
        {
            return new BombardEnemy();
        }

        public override void Update(GameTime gameTime)
        {
            Location += new Vector2(0, Velocity);

            base.Update(gameTime);
            BroadPhase.Instance.AddEntity(this, AABB);
        }
    }
}