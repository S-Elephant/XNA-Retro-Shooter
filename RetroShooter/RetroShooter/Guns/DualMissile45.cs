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
    public class DualMissile45 : BaseGun
    {
        Vector2 ShootDir2;

        public DualMissile45()
            : base(Common.InvalidVector2, null)
        {
            throw new Exception("Use the pool.");
        }

        internal DualMissile45(Vector2 relativeLoc, Player owner) :
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
                ShootDelayTimer = new SimpleTimer(Maths.RandomNr(1500, 1700));
                ShootDir = new Vector2(-1, 1);
                ShootDir2 = new Vector2(1, 1);
            }
            else
            {
                ShootDelayTimer = new SimpleTimer(1600);
                ShootDir = new Vector2(-1, -1);
                ShootDir2 = new Vector2(1, -1);
            }
        }

        public static DualMissile45 PoolConstructor()
        {
            return new DualMissile45(Common.InvalidVector2, null) {GunType = eEnemyGunType.DualMissile45 };
        }

        public override void Update(GameTime gameTime, Vector2 location)
        {
            ShootDelayTimer.Update(gameTime);
            if (ShootDelayTimer.IsDone)
            {
                ShootDelayTimer.Reset();
                Level.Instance.AddProjectile(eProjectile.Missile45, location + RelativeLocation - new Vector2(11, 0), ShootDir, Owner, -1, Color.White, Tier);
                Level.Instance.AddProjectile(eProjectile.MissileM45, location + RelativeLocation + new Vector2(11, 0), ShootDir2, Owner, -1, Color.White, Tier);
                if (Owner == null)
                    Engine.Instance.Audio.PlaySound(AudioConstants.RLaunchEnemy);
                else
                    Engine.Instance.Audio.PlaySound(AudioConstants.RLaunch);
            }
        }
    }
}