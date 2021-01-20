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
    public class Visual
    {
        eVisual VisualType;
        
        private bool m_IsDisposed = false;
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        SimpleASprite Animation;
        const string Explosion01Tex = "Effects/explosion01_64px_16";

        /// <summary>
        /// Pool constructor
        /// </summary>
        internal Visual()
        {
            Animation = new SimpleASprite(Common.InvalidVector2, Explosion01Tex, 64, 64, 16, 1, 64) { LoopOnce = true };
        }

        public static Visual PoolConstructor()
        {
            return new Visual();
        }

        public void Initialize(eVisual type, Vector2 centerLoc)
        {
            VisualType = type;

            switch (VisualType)
            {
                case eVisual.Explosion01:
                    IsDisposed = false;
                    Animation.IsDisposed = false;
                    Animation.Location = centerLoc - new Vector2(32, 32);
                    Animation.CurrentFrame = Point.Zero;
                    Animation.IsDisposed = false;
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
            if (Animation.IsDisposed)
                IsDisposed = true;
        }

        public void Draw()
        {
            Animation.Draw(Engine.Instance.SpriteBatch);
        }
    }
}