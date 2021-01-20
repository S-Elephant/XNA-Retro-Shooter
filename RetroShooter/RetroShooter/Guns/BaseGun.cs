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
    public class BaseGun : IXNADispose
    {
        #region
        protected Vector2 RelativeLocation;

        protected eEnemyGunType m_GunType;
        public eEnemyGunType GunType
        {
            get { return m_GunType; }
            protected set { m_GunType = value; }
        }
       
        protected SimpleTimer ShootDelayTimer;
        protected Player Owner;

        private bool m_IsDisposed = false;
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        protected Vector2 ShootDir;

        // Upgrading

        private int m_Tier = 1;
        public int Tier
        {
            get { return m_Tier; }
            protected set { m_Tier = value; }
        }
       
        protected int MaxTier = 1;
        public bool CanUpgradeTier
        {
            get
            {
                if (Owner == null)
                    return Tier < MaxTier;
                else
                    return Tier < MaxTier && Owner.OffensiveTier > Tier;
            }
        }
        #endregion

        public BaseGun(Vector2 relativeLocation, Player owner)
        {
            RelativeLocation = relativeLocation;
            Owner = owner;
        }

        public virtual void Initialize(Vector2 relativeLoc, Player owner)
        {
            IsDisposed = false;
            RelativeLocation = relativeLoc;
            Owner = owner;
            Tier = 1;
        }

        public void ResetDelay()
        {
            ShootDelayTimer.Reset();
        }

        public virtual void Update(GameTime gameTime, Vector2 location)
        {

        }

        public void UpgradeTier()
        {
            if (Tier < MaxTier)
                Tier++;
        }
    }
}