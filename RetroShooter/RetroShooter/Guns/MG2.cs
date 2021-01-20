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
    public class MG2 : BaseGun
    {
        public static Color MGDrawColor;
        Vector2 RelativeLocation2;
        int MG2Idx = 0;

        // For creating enemy pauses
        SimpleTimer EnemyDelayTimer = new SimpleTimer(1600);
        bool EnemyIsReloading = false;

        public MG2():base(Common.InvalidVector2,null)
        {
            throw new Exception("Use the pool.");
        }

        internal MG2(Vector2 relativeLoc1, Vector2 relativeLoc2, Player owner) :
            base(relativeLoc1, owner)
        {
            Tier = 2;
            Initialize(relativeLoc1,relativeLoc2, owner);
        }

        public static MG2 PoolConstructor()
        {
            return new MG2(Common.InvalidVector2, Common.InvalidVector2, null) { GunType = eEnemyGunType.MG2 };
        }

        public void Initialize(Vector2 relativeLoc1, Vector2 relativeLoc2, Player owner)
        {
            IsDisposed = false;
            RelativeLocation = relativeLoc1;
            RelativeLocation2 = relativeLoc2;
            Owner = owner;
            
            ShootDelayTimer = new SimpleTimer(140);
            if (owner == null)
                ShootDir = new Vector2(0, 1);
            else
                ShootDir = new Vector2(0, -1);
        }

        public override void Update(GameTime gameTime, Vector2 location)
        {
            if (Owner == null)
            {
                EnemyDelayTimer.Update(gameTime);
                if (EnemyDelayTimer.IsDone)
                {
                    EnemyDelayTimer.Reset();
                    EnemyIsReloading = !EnemyIsReloading;
                }
            }

            ShootDelayTimer.Update(gameTime);
            if (ShootDelayTimer.IsDone && !EnemyIsReloading)
            {
                ShootDelayTimer.Reset();
                Level.Instance.AddProjectile(eProjectile.MG2, location + RelativeLocation, ShootDir, Owner, MG2Idx, MGDrawColor, Tier);
                Level.Instance.AddProjectile(eProjectile.MG2, location + RelativeLocation2, ShootDir, Owner, MG2Idx, MGDrawColor, Tier);
                MG2Idx++;
                if (MG2Idx > 2)
                    MG2Idx = 0;
            }
        }
    }
}