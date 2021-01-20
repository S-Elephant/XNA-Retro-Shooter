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
    public class MG1 : BaseGun
    {
        public static Color MGDrawColor;
        Vector2 RelativeLocation2;
        int MG1Idx = 0;

        // For creating enemy pauses
        SimpleTimer EnemyDelayTimer = new SimpleTimer(1600);
        bool EnemyIsReloading = false;

        public MG1():base(Common.InvalidVector2,null)
        {
            throw new Exception("Use the pool.");
        }

        internal MG1(Vector2 relativeLoc1, Vector2 relativeLoc2, Player owner) :
            base(relativeLoc1, owner)
        {
            Initialize(relativeLoc1,relativeLoc2, owner);
        }

        public static MG1 PoolConstructor()
        {
            return new MG1(Common.InvalidVector2, Common.InvalidVector2, null) { GunType = eEnemyGunType.MG1};
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
                Level.Instance.AddProjectile(eProjectile.MG1, location + RelativeLocation, ShootDir, Owner, MG1Idx, MGDrawColor, Tier);
                Level.Instance.AddProjectile(eProjectile.MG1, location + RelativeLocation2, ShootDir, Owner, MG1Idx, MGDrawColor, Tier);
                MG1Idx++;
                if (MG1Idx > 2)
                    MG1Idx = 0;
            }
        }
    }
}