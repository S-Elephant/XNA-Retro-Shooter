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
    public class ItemEnemy : BaseEnemy
    {
        const float Velocity = 1.75f;
        /// <summary>
        /// On what wave number it was last spawned.
        /// </summary>
        public static int LastWaveSpawn;

        internal ItemEnemy() :
            base()
        {
            Animation = BaseEnemy.Animations[4];
            Shadow = BaseEnemy.Shadows[4];
            ScoreValue = 120;

            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 0, Animation.Width, Animation.Height)));
            Location = Common.InvalidVector2;
            CollisionDamage = 80;
            
            DropChance = 100;
            HP.MaxHP = 120;
            HP.CurrentHP = 120;
            ReappearsOnTopCnt = 0;
        }

        public void Initialize(Vector2 startLoc)
        {
            IsDisposed = false;
            Location = startLoc;
        }

        public static ItemEnemy PoolConstructor()
        {
            return new ItemEnemy();
        }

        public override void Update(GameTime gameTime)
        {
            Location += new Vector2(0, Velocity);

            base.Update(gameTime);
            BroadPhase.Instance.AddEntity(this, AABB);
        }
    }
}