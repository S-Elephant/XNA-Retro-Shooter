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
    public class Boom1 : BaseGun
    {
        public Boom1()
            : base(Common.InvalidVector2, null)
        {
            throw new Exception("Use the pool.");
        }

        internal Boom1(Vector2 relativeLoc, Player owner):
            base(relativeLoc, owner)
        {
            MaxTier = 2;
            Initialize(relativeLoc, owner);
        }

        public override void Initialize(Vector2 relativeLoc, Player owner)
        {
            base.Initialize(relativeLoc, owner);
                
            if (owner == null)
            {
                ShootDelayTimer = new SimpleTimer(1450);
                ShootDir = new Vector2(0, 1);
            }
            else
            {
                ShootDir = new Vector2(0, -1);
                ShootDelayTimer = new SimpleTimer(1200);
            }
        }

        public static Boom1 PoolConstructor()
        {
            return new Boom1(Common.InvalidVector2, null) { GunType = eEnemyGunType.Boom1Enemy };
        }

        public override void Update(GameTime gameTime, Vector2 location)
        {
            ShootDelayTimer.Update(gameTime);
            if (ShootDelayTimer.IsDone)
            {
                ShootDelayTimer.Reset();
                Level.Instance.AddProjectile(eProjectile.Boom1Enemy, location + RelativeLocation, ShootDir, Owner, -1, Color.White, Tier);
            }
        }
    }
}