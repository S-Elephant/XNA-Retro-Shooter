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
    public class Missile : BaseGun
    {
        public Missile():base(Common.InvalidVector2,null)
        {
            throw new Exception("Use the pool.");
        }

        internal Missile(Vector2 relativeLoc, Player owner) :
            base(relativeLoc, owner)
        {
            MaxTier = 2;
            Initialize(relativeLoc, owner);
        }

        public override void Initialize(Vector2 relativeLoc, Player owner)
        {
            base.Initialize(relativeLoc, owner);

            ShootDelayTimer = new SimpleTimer(1600);
            if (owner == null)
                ShootDir = new Vector2(0, 1);
            else
                ShootDir = new Vector2(0, -1);
        }

        public static Missile PoolConstructor()
        {
            return new Missile(Common.InvalidVector2, null) { GunType = eEnemyGunType.Missile };
        }

        public override void Update(GameTime gameTime, Vector2 location)
        {
            ShootDelayTimer.Update(gameTime);
            if (ShootDelayTimer.IsDone)
            {
                ShootDelayTimer.Reset();
                Level.Instance.AddProjectile(eProjectile.Missile, location + RelativeLocation - new Vector2(11, 0), ShootDir, Owner, -1, Color.White, Tier);
                Level.Instance.AddProjectile(eProjectile.Missile, location + RelativeLocation + new Vector2(11, 0), ShootDir, Owner, -1, Color.White, Tier);
                if (Owner != null)
                    Engine.Instance.Audio.PlaySound(AudioConstants.RLaunch);
            }
        }
    }
}