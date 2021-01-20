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
    public class BaseShip : IEntity
    {
        #region Members
        protected Texture2D Animation;
        protected Texture2D Shadow;

        private bool m_IsDisposed = false;
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        private SpriteEffects m_SpriteEffects = new SpriteEffects();
        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            protected set { m_SpriteEffects = value; }
        }

        protected Rectangle m_AABB;
        public Rectangle AABB
        {
            get { return m_AABB; }
            protected set { m_AABB = value; }
        }

        protected List<Rectangle2> m_CollisionRects = new List<Rectangle2>();
        public List<Rectangle2> CollisionRects
        {
            get { return m_CollisionRects; }
            protected set { m_CollisionRects = value; }
        }

        protected Vector2 m_Location;
        public Vector2 Location
        {
            get { return m_Location; }
            protected set
            {
                m_Location = value;
                Point newPoint = new Point(value.Xi(), value.Yi());
                m_AABB.Location = newPoint;
                for (int i = 0; i < CollisionRects.Count; i++)
                    CollisionRects[i].SetNewLocation(newPoint);
            }
        }
        static readonly Vector2 ShadowOffset = new Vector2(0, 24);
        #endregion

        public BaseShip()
        {

        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Animation, Location, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects, 0);
        }

        public virtual void DrawShadow()
        {
            Engine.Instance.SpriteBatch.Draw(Shadow, Location + ShadowOffset, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects, 0);
        }

        public virtual void DebugDraw()
        {
            Engine.Instance.SpriteBatch.Draw(Common.White1px50Trans, AABB, Color.Pink);
            foreach (Rectangle2 cr in CollisionRects)
            {
                Engine.Instance.SpriteBatch.Draw(Common.White1px50Trans, cr.Absolute, Color.Red);
            }
        }
    }
}