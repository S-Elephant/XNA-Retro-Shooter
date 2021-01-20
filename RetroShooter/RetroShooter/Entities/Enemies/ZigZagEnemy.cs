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
    public class ZigZagEnemy : BaseEnemy
    {
        float VelocityX = 4f;
        float VelocityY = 1.2f;
        int MaxChange;
        int ChangeCnt = 0;
        int Direction;

        internal ZigZagEnemy() :
            base()
        {
            Animation = BaseEnemy.Animations[3];
            Shadow = BaseEnemy.Shadows[3];
            ScoreValue = 20;

            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 0, Animation.Width, Animation.Height)));
            CollisionDamage = 15;
            MaxChange = Maths.RandomNr(50, 250);
            ReappearsOnTopCnt = 0;
        }

        public void Initialize(Vector2 startLocation)
        {
            IsDisposed = false;
            Location = startLocation;

            if (Maths.Chance(50))
                Direction = 1;
            else
                Direction = -1;

            if (Maths.Chance(50) || Level.Instance.MGCnt == Level.MAX_MG_CNT)
            {
                Boom1 bm = Level.Instance.BoomPool.New();
                bm.Initialize(new Vector2(Animation.Width / 2 - 9, Animation.Height / 2), null);
                Guns.Add(bm);
            }
            else
            {
                MG1 mg = Level.Instance.MGPool.New();
                mg.Initialize(new Vector2(Animation.Width / 2 - 4 - 10, Animation.Height), new Vector2(Animation.Width / 2 - 4 + 10, Animation.Height), null);
                Guns.Add(mg);
                Level.Instance.MGCnt++;
            }
        }

        public static ZigZagEnemy PoolConstructor()
        {
            return new ZigZagEnemy();
        }

        public override void Update(GameTime gameTime)
        {
            ChangeCnt++;
            if (ChangeCnt >= MaxChange)
            {
                ChangeCnt = 0;
                Direction *= -1;
            }


            Vector2 direction = new Vector2(Direction*VelocityX, 1*VelocityY);
            Location += direction;
            if (Location.X < 0)
                Direction = 1;
            else if (Location.X + AABB.Width > Engine.Instance.Width)
                Direction = -1;

            base.Update(gameTime);
            BroadPhase.Instance.AddEntity(this, AABB);
        }
    }
}