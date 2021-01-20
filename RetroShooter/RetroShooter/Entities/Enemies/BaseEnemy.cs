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
    public class BaseEnemy : BaseShip
    {
        public static readonly Texture2D[] Animations = new Texture2D[8] {
            Common.str2Tex("Ships/ship01"),
            Common.str2Tex("Ships/ship02"),
            Common.str2Tex("Ships/ship03"),
            Common.str2Tex("Ships/ship04"),
            Common.str2Tex("Ships/ship05"),
            Common.str2Tex("Ships/ship06"),
            Common.str2Tex("Ships/ship07"),
            Common.str2Tex("Ships/ship08")
        };
        public static readonly Texture2D[] Shadows = new Texture2D[8] {
            Common.str2Tex("Shadows/ship01"),
            Common.str2Tex("Shadows/ship02"),
            Common.str2Tex("Shadows/ship03"),
            Common.str2Tex("Shadows/ship04"),
            Common.str2Tex("Shadows/ship05"),
            Common.str2Tex("Shadows/ship06"),
            Common.str2Tex("Shadows/ship07"),
            Common.str2Tex("Shadows/ship08")
        };

        #region Members
        const string ExplosionFX = "explosion01";
        const string ImpactFX = "impact01";

        protected List<BaseGun> Guns = new List<BaseGun>();
        protected HitPoints HP = new HitPoints(40, 40, 0);
        protected int DropChance = Level.Instance.DropRateModifier;
        protected int ScoreValue;

        SimpleTimer InitialShootDelay = new SimpleTimer(Maths.RandomNr(0, 500));

        private float m_CollisionDamage;
        public float CollisionDamage
        {
            get { return m_CollisionDamage; }
            protected set { m_CollisionDamage = value; }
        }

        public Vector2 CenterLoc { get { return new Vector2(Location.X + Animation.Width / 2, Location.Y + Animation.Height / 2); } }
        public int ReappearsOnTopCnt;
        #endregion

        public BaseEnemy()
        {
        }

        public void TakeDamage(float amount, Player damager)
        {
            if (!IsDisposed)
            {
                HP.CurrentHP -= amount;
                if (HP.CurrentHP <= 0)
                {
                    damager.Score += (int)((ScoreValue + Level.Instance.WaveNr) * Level.Instance.ScoreModifier);
                    damager.Kills++;
                    Die();
                }
                else 
                    Engine.Instance.Audio.PlaySound(AudioConstants.Impact);
            }
        }

        public void Die()
        {
            if (Maths.Chance(DropChance))
                Level.Instance.AddPickup((ePickupType)Maths.RandomNr(0, 7), Location);

            // Explosion
            Level.Instance.AddVisual(eVisual.Explosion01, CenterLoc);
            Engine.Instance.Audio.PlaySound(AudioConstants.Explode);

            Dispose();
        }

        protected void Dispose()
        {
            // Reset HP
            HP.HealFull();

            // Guns
            if (Guns.Count > 0 && Guns[0].GunType == eEnemyGunType.MG1)
                Level.Instance.MGCnt--;
            foreach (BaseGun gun in Guns)
                gun.IsDisposed = true;
            Guns.Clear();

            // Other
            InitialShootDelay = new SimpleTimer(Maths.RandomNr(0, 500));
            ReappearsOnTopCnt = 1;

            IsDisposed = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Location.Y > 800)
            {
                if (ReappearsOnTopCnt > 0)
                {
                    Location = new Vector2(Location.X, -AABB.Height);
                    ReappearsOnTopCnt--;
                }
                else
                {
                    Dispose();
                    return;
                }
            }

            if (InitialShootDelay.IsDone)
            {
                for (int i = 0; i < Guns.Count; i++)
                    Guns[i].Update(gameTime, Location);
            }
            else
                InitialShootDelay.Update(gameTime);
        }
    }
}