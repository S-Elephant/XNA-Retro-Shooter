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
    public class AutoAim : BaseGun
    {
        public AutoAim()
            : base(Common.InvalidVector2, null)
        {
            throw new Exception("Use the pool.");
        }

        internal AutoAim(Vector2 relativeLoc, Player owner) :
            base(relativeLoc, owner)
        {
            MaxTier = 2;
            Initialize(relativeLoc, owner);    
        }

        public static AutoAim PoolConstructor()
        {
            return new AutoAim(Common.InvalidVector2, null) { GunType = eEnemyGunType.AutoAim };
        }

        public override void Initialize(Vector2 relativeLoc, Player owner)
        {
            base.Initialize(relativeLoc, owner);

            if (owner == null)
                ShootDelayTimer = new SimpleTimer(Maths.RandomNr(850, 1700));
            else
                ShootDelayTimer = new SimpleTimer(800);
        }

        public override void Update(GameTime gameTime, Vector2 location)
        {
            ShootDelayTimer.Update(gameTime);
            if (ShootDelayTimer.IsDone)
            {
                ShootDelayTimer.Reset();
                Vector2 absLoc = location + RelativeLocation;
                if (Owner == null)
                    Level.Instance.AddProjectile(eProjectile.Round, absLoc, Maths.GetMoveDir(absLoc, Level.Instance.GetNearestPlayer(absLoc).CenterLoc), Owner, -1, Color.White, Tier);
                else
                {
                    BaseEnemy e = Level.Instance.GetRandomEnemy();
                    if (e != null)
                        Level.Instance.AddProjectile(eProjectile.Round, absLoc, Maths.GetMoveDir(absLoc, new Vector2(e.Location.X - e.AABB.Width / 2, e.Location.Y + e.AABB.Height + 16)), Owner, -1, Color.White, Tier);
                }
            }
        }
    }
}