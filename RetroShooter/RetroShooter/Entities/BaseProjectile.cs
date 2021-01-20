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
    public class BaseProjectile: IEntity
    {
        #region Members
        public Color DrawColor = Color.White;

        private Rectangle m_AABB;
        public Rectangle AABB
        {
            get { return m_AABB; }
            protected set { m_AABB = value; }
        }

        private SpriteEffects m_SpriteEffects = new SpriteEffects();
        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            protected set { m_SpriteEffects = value; }
        }

        protected List<Rectangle2> m_CollisionRects = new List<Rectangle2>();
        public List<Rectangle2> CollisionRects
        {
            get { return m_CollisionRects; }
            protected set { m_CollisionRects = value; }
        }

        Texture2D Animation;

        private Vector2 m_Location;
        public Vector2 Location
        {
            get { return m_Location; }
            set
            {
                m_Location = value;
                m_AABB.X = Location.Xi();
                m_AABB.Y = Location.Yi();
            }
        }

        Vector2 MoveIncrementer;

        eProjectile ProjectileType;
        Vector2 Direction;
        float Velocity;

        private Player m_Owner;
        public Player Owner
        {
            get { return m_Owner; }
            private set { m_Owner = value; }
        }

        private bool m_IsDisposed = false;
        public bool IsDisposed
        {
            get { return m_IsDisposed; }
            set { m_IsDisposed = value; }
        }

        private float m_Damage;
        public float Damage
        {
            get { return m_Damage; }
            private set { m_Damage = value; }
        }

        // Textures
        static readonly Texture2D RoundTexture = Common.str2Tex("projectiles/projectile01");
        static readonly Texture2D RoundTextureT2 = Common.str2Tex("projectiles/projectile01T2");
        static readonly Texture2D Boom1Texture = Common.str2Tex("projectiles/boom1_enemy");
        static readonly Texture2D Boom1TexturePlayer = Common.str2Tex("projectiles/boom1");
        static readonly Texture2D Boom1TexturePlayerT2 = Common.str2Tex("projectiles/boom1T2");
        static readonly Texture2D MissileTexture = Common.str2Tex("projectiles/missile");
        static readonly Texture2D MissileTextureT2 = Common.str2Tex("projectiles/missileT2");
        static readonly Texture2D MissileEnemyTexture = Common.str2Tex("projectiles/missileEnemy");
        static readonly Texture2D Missile45Texture = Common.str2Tex("projectiles/missile45");
        static readonly Texture2D MissileM45Texture = Common.str2Tex("projectiles/missileM45");
        static readonly Texture2D Missile45PlayerTexture = Common.str2Tex("projectiles/missile45Player");
        static readonly Texture2D MissileM45PlayerTexture = Common.str2Tex("projectiles/missileM45Player");
        static readonly Texture2D Missile45PlayerT2Texture = Common.str2Tex("projectiles/missile45PlayerT2");
        static readonly Texture2D MissileM45PlayerT2Texture = Common.str2Tex("projectiles/missileM45PlayerT2");
        static readonly Texture2D[] MG1Textures = new Texture2D[3] { Common.str2Tex("projectiles/mg1_0"), Common.str2Tex("projectiles/mg1_1"), Common.str2Tex("projectiles/mg1_2") };
        static readonly Texture2D[] MG2Textures = new Texture2D[3] { Common.str2Tex("projectiles/mg2_0"), Common.str2Tex("projectiles/mg2_1"), Common.str2Tex("projectiles/mg2_2") };
        #endregion

        /// <summary>
        /// Pool constructor
        /// </summary>
        public BaseProjectile()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectileType"></param>
        /// <param name="location"></param>
        /// <param name="direction"></param>
        /// <param name="owner"></param>
        /// <param name="texIdx"></param>
        /// <param name="tier"></param>
        /// <param name="dmgMod">1 means 100%, 1.5 means 150%, etc.</param>
        public void Initialize(eProjectile projectileType, Vector2 location, Vector2 direction, Player owner, int texIdx, int tier)
        {
            IsDisposed = false;
            ProjectileType = projectileType;
            DrawColor = Color.White;
            switch (ProjectileType)
            {
                case eProjectile.Round:
                    AABB = new Rectangle(location.Xi(),location.Yi(), 16, 16);
                    if (owner == null)
                    {
                        Animation = RoundTexture;
                        Velocity = 9f;
                        Damage = 5;
                    }
                    else
                    {
                        if (tier == 1)
                        {
                            Animation = RoundTexture;
                            Velocity = 16f;
                            Damage = 10;
                        }
                        else
                        {
                            Animation = RoundTextureT2;
                            Velocity = 20f;
                            Damage = 15;
                        }
                    }
                    break;
                case eProjectile.Boom1Enemy:
                    AABB = new Rectangle(location.Xi(), location.Yi(), 18, 24);
                    if (owner == null)
                    {
                        Animation = Boom1Texture;
                        Velocity = 6.5f;
                        Damage = 15;
                    }
                    else
                    {
                        if (tier == 1)
                        {
                            Animation = Boom1TexturePlayer;
                            Velocity = 6.5f;
                            Damage = 15;
                        }
                        else
                        {
                            Animation = Boom1TexturePlayerT2;
                            Velocity = 7f;
                            Damage = 22;
                        }
                    }
                    break;
                case eProjectile.Missile:
                    AABB = new Rectangle(location.Xi(), location.Yi(), 18, 23);
                        Velocity = 7.3f;
                    if (owner == null)
                    {
                        Animation = MissileEnemyTexture;
                        Damage = 10;
                    }
                    else
                    {
                        if (tier == 1)
                        {
                            Animation = MissileTexture;
                            Damage = 10;
                        }
                        else
                        {
                            Animation = MissileTextureT2;
                            Damage = 20;
                        }
                    }
                    break;
                case eProjectile.MG1:
                    Animation = MG1Textures[texIdx];
                    AABB = new Rectangle(location.Xi(), location.Yi(), 8, 8);
                    Velocity = 9f;
                    Damage = 2;
                    break;
                case eProjectile.MG2:
                    Animation = MG2Textures[texIdx];
                    AABB = new Rectangle(location.Xi(), location.Yi(), 8, 8);
                    Velocity = 10f;
                    Damage = 3.2f;
                    break;
                case eProjectile.MissileM45:
                    AABB = new Rectangle(location.Xi(), location.Yi(), 24, 22);
                    Velocity = 7f;
                    if (owner == null)
                    {
                        Animation = MissileM45Texture;
                        Damage = 10;
                    }
                    else
                    {
                        if (tier == 1)
                        {
                            Animation = Missile45PlayerTexture;
                            Damage = 10;
                        }
                        else
                        {
                            Animation = Missile45PlayerT2Texture;
                            Damage = 20;
                        }
                    }
                    break;
                case eProjectile.Missile45:
                    AABB = new Rectangle(location.Xi(), location.Yi(), 23, 23);
                    Velocity = 7f;
                    if (owner == null)
                    {
                        Animation = Missile45Texture;
                        Damage = 10;
                    }
                    else
                    {
                        if (tier == 1)
                        {
                            Animation = MissileM45PlayerTexture;
                            Damage = 10;
                        }
                        else
                        {
                            Animation = MissileM45PlayerT2Texture;
                            Damage = 20;
                        }
                    }
                    break;
                default:
                    throw new CaseStatementMissingException();
            }

            Location = location;
            Direction = direction;
            Direction.Normalize();
            Owner = owner;
            MoveIncrementer = Velocity * Direction;
            if (Owner != null)
                Damage *= owner.DmgMod;
        }

        public void Update(GameTime gameTime)
        {
            Location = new Vector2(Location.X + MoveIncrementer.X, Location.Y + MoveIncrementer.Y);

            if (Location.X + Animation.Width < 0 || Location.X > Engine.Instance.Width
                || Location.Y + Animation.Height < 0 || Location.Y > Engine.Instance.Height)
            {
                IsDisposed = true;                
                return;
            }
            BroadPhase.Instance.AddEntity(this, AABB);
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Animation, Location, DrawColor);
        }

        public void DebugDraw()
        {
            Engine.Instance.SpriteBatch.Draw(Common.White1px50Trans, AABB, Color.Pink);
        }

        public void DrawShadow()
        {
        }
    }
}