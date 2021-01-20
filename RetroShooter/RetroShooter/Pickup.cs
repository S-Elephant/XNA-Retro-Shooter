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
    public class Pickup
    {
        #region Members
        Texture2D Texture;

        private ePickupType m_PickupType;
        public ePickupType PickupType
        {
            get { return m_PickupType; }
            private set { m_PickupType = value; }
        }
       
        public Vector2 Location;
        const float VelocityX = 0.5f;
        const float VelocityY = 1.2f;
        int MaxChange;
        int ChangeCnt = 0;
        int Direction = 1;
        public bool IsDisposed;

        static readonly Texture2D[] Textures = new Texture2D[8] { Common.str2Tex("Pickups/hp"), Common.str2Tex("Pickups/bo"), Common.str2Tex("Pickups/aim"), Common.str2Tex("Pickups/missile"), Common.str2Tex("Pickups/speed"), Common.str2Tex("Pickups/shield"), Common.str2Tex("Pickups/dualMissile"), Common.str2Tex("Pickups/shieldRegen") };
        #endregion

        /// <summary>
        /// Pool constructor
        /// </summary>
        public Pickup()
        {

        }

        public void Initialize(ePickupType type, Vector2 location)
        {
            IsDisposed = false;
            PickupType = type;
            Location = location;
            MaxChange = Maths.RandomNr(50, 250);

            switch (PickupType)
            {
                case ePickupType.HP:
                    Texture = Textures[0];
                    break;
                case ePickupType.Boom1:
                    Texture = Textures[1];
                    break;
                case ePickupType.Aim:
                    Texture = Textures[2];
                    break;
                case ePickupType.Missile:
                    Texture = Textures[3];
                    break;
                case ePickupType.Speed:
                    Texture = Textures[4];
                    break;
                case ePickupType.Shield:
                    Texture = Textures[5];
                    break;
                case ePickupType.DualMissile:
                    Texture = Textures[6];
                    break;
                case ePickupType.ShieldRegen:
                    Texture = Textures[7];
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public void Update(GameTime gameTime)
        {
            ChangeCnt++;
            if (ChangeCnt >= MaxChange)
            {
                ChangeCnt = 0;
                Direction *= -1;
            }

            Vector2 direction = new Vector2(Direction * VelocityX, 1 * VelocityY);
            Location += direction;
            if (Location.X < 0)
                Direction = 1;
            else if (Location.X + Texture.Width > Engine.Instance.Width)
                Direction = -1;
            if (Location.Y > 800)
                IsDisposed = true;
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Texture, Location, Color.White);
        }
    }
}