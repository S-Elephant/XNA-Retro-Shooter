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
    public class StraightEnemy : BaseEnemy
    {
        float Velocity;

        internal StraightEnemy() :
            base()
        {
            Animation = BaseEnemy.Animations[0];
            Shadow = BaseEnemy.Shadows[0];
            ScoreValue = 12;
            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            CollisionRects.Add(new Rectangle2(Common.InvalidVector2, new Rectangle(0, 0, Animation.Width, Animation.Height)));
            CollisionDamage = 20;
            ReappearsOnTopCnt = 1;
        }

        /// <summary>
        /// Call this after getting me from the pool.
        /// </summary>
        /// <param name="startLoc"></param>
        public void Initialize(Vector2 startLoc)
        {
            IsDisposed = false;
            Location = startLoc;

            int rndWpn;
            bool pass;
            do
            {
                pass = true;
                rndWpn = Maths.RandomNr(1, 100);
                if (rndWpn < 30)
                {
                    AutoAim aa = Level.Instance.AutoAimPool.New();
                    aa.Initialize(new Vector2(Animation.Width / 2 - 8, Animation.Height / 2 - 8), null);
                    Guns.Add(aa);
                    Velocity = 2.4f;
                }
                else if (rndWpn < 70)
                {
                    Boom1 bm = Level.Instance.BoomPool.New();
                    bm.Initialize(new Vector2(Animation.Width / 2 - 9, Animation.Height / 2), null);
                    Guns.Add(bm);
                    Velocity = 3f;
                }
                else
                {
                    if (Level.Instance.MGCnt < Level.MAX_MG_CNT)
                    {
                        Level.Instance.MGCnt++;
                        Velocity = 3f;
                        MG1 mg = Level.Instance.MGPool.New();
                        mg.Initialize(new Vector2(Animation.Width / 2 - 4 - 10, Animation.Height), new Vector2(Animation.Width / 2 - 4 + 10, Animation.Height), null);
                        Guns.Add(mg);
                    }
                    else
                        pass = false;
                }
            } while (!pass);
        }

        public static StraightEnemy PoolConstructor()
        {
            return new StraightEnemy();
        }

        public override void Update(GameTime gameTime)
        {
            Location += new Vector2(0, Velocity);

            base.Update(gameTime);
            BroadPhase.Instance.AddEntity(this, AABB);
        }
    }
}