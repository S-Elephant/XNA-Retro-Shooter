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
    public class Level : IActiveState
    {
        #region Members
        // Other
        public static Level Instance;
        public int DropRateModifier;
        ScrollBG ScrollBG;
        public int MGCnt = 0; // Used to limit the max. number of MG enemies because they drain a lot of cpu.
        public const int MAX_MG_CNT = 10;
        const int SPAWN_DELAY_INCREASE = 300;
        ScrapStation ActiveScrapStation = null;
        bool NBombWasUsedThisWave = false;
        AlphaBlendHelper ABH = new AlphaBlendHelper(0, 230, 230, -2) { IsDone = true, Loop = false };
        const float DOUBLE_PICKUP_HEAL_AMOUNT = 15;
        string Music;

        private float m_ScoreModifier;
        public float ScoreModifier
        {
            get { return m_ScoreModifier; }
            private set { m_ScoreModifier = value; }
        }       

        #region SpawnClearBonus
        const string SpawnClrBonusText = "Wave Cleared!";
        const string SpawnClrBonusPrefix = "+$";
        const string SpawnClrBonusSuffix = "!";
        StringBuilder SpawnClrBonusSB = new StringBuilder(10);
        bool SpawnClrBonusIsShowing = false;
        float SpawnClrBonusX;
        static readonly SpriteFont SpawnClrBonusFont = Common.str2Font("MenuTitle");
        #endregion

        // Lists and pools and such
        const int ENTITITY_MAX = 512, PROJECTILE_POOL_MAX = 1024, PICKUP_MAX=50, ENEMY_MAX=160, VISUALS_MAX=50;
        public List<IEntity> Entities = new List<IEntity>(ENTITITY_MAX);
        public List<PlayerShip> PlayerShips = new List<PlayerShip>(4);
        public List<Player> Players = new List<Player>();
        List<BaseProjectile> ProjectilePool = new List<BaseProjectile>(PROJECTILE_POOL_MAX);
        List<Pickup> PickupPool = new List<Pickup>(PICKUP_MAX);
        List<Pickup> Pickups = new List<Pickup>(PICKUP_MAX);
        List<Visual> Visuals = new List<Visual>(VISUALS_MAX);
        #region Generic pools
        // Guns
        public Pool<MG1> MGPool;
        public Pool<MG2> MG2Pool;
        public Pool<AutoAim> AutoAimPool;
        public Pool<Boom1> BoomPool;
        public Pool<Missile> MissilePool;
        public Pool<DualMissile45> DualMissile45Pool;
        // Enemies
        public Pool<StraightEnemy> StraightEnemyPool;
        public Pool<SuiciderEnemy> SuiciderEnemyPool;
        public Pool<ItemEnemy> ItemEnemyPool;
        public Pool<ZigZagEnemy> ZigZagEnemyPool;
        public Pool<BombardEnemy> BombardEnemyPool;
        public Pool<Dual45Enemy> Dual45EnemyPool;
        public Pool<SideEnemy> SideEnemyPool;
        // Explosions
        public Pool<Visual> ExplosionPool;
        // Scrap station
        public Pool<ScrapStation> ScrapStationPool;
        #endregion

        // Spawning
        private int m_WaveNr;
        public int WaveNr
        {
            get { return m_WaveNr; }
            private set { m_WaveNr = value; }
        }
        SimpleTimer SpawnDelayTimer;
        static readonly SpriteFont WaveFont = Common.str2Font("Font03_18");
        StringBuilder WaveNrSB;
        readonly Vector2 WaveNrLoc = new Vector2(Engine.Instance.Width/2-10, 5);
        bool IsFirstSpawn = true;
        static readonly Vector2 SpawnTimerLoc = new Vector2(Engine.Instance.Width - 50, 5);
        StringBuilder SpawnTimerSB = new StringBuilder(2);

        // Paused
        bool IsPaused = false;
        static readonly SpriteFont PausedFont = Common.str2Font("MenuTitle");
        const string PAUSE_TEXT = "Game [P]aused";
        Vector2 PauseLocation;
        #endregion

        public Level(int startingWave, int dropRateModifier, int waveDelayInMS, int area, float scoreModifier, string music, ePlaneType planeType, string shipName)
        {
            // Music
            Music = music;
            MP3MusicMgr.Instance.PlayMusic(music);

            ScoreModifier = scoreModifier;
            WaveNr = startingWave;
            ItemEnemy.LastWaveSpawn = WaveNr;
            DropRateModifier = dropRateModifier;
            SpawnDelayTimer = new SimpleTimer(waveDelayInMS);
            ScrollBG = new ScrollBG("bg0" + area.ToString());
            Shop.IsFirstVisit = true;
            if (area == 3 || area == 4)
            {
                MG1.MGDrawColor = Color.DarkRed;
                MG2.MGDrawColor = Color.DarkBlue;
            }
            else
                MG1.MGDrawColor = MG2.MGDrawColor = Color.White;
            WaveNrSB = new StringBuilder(startingWave.ToString(), 2);

            BroadPhase.Instance = new BroadPhase(128);
            BroadPhase.Instance.Init();

            Level.Instance = this;
            #region Generic Pools
            MGPool = new Pool<MG1>(25, true, g => !g.IsDisposed, () => MG1.PoolConstructor());
            MG2Pool = new Pool<MG2>(1, true, g => !g.IsDisposed, () => MG2.PoolConstructor());
            AutoAimPool = new Pool<AutoAim>(100, true, g => !g.IsDisposed, () => AutoAim.PoolConstructor());
            BoomPool = new Pool<Boom1>(100, true, g => !g.IsDisposed, () => Boom1.PoolConstructor());
            MissilePool = new Pool<Missile>(80, true, g => !g.IsDisposed, () => Missile.PoolConstructor());
            DualMissile45Pool = new Pool<DualMissile45>(80, true, g => !g.IsDisposed, () => DualMissile45.PoolConstructor());
            // Enemies
            StraightEnemyPool = new Pool<StraightEnemy>(200, true, e => !e.IsDisposed, () => StraightEnemy.PoolConstructor());
            SuiciderEnemyPool = new Pool<SuiciderEnemy>(60, true, e => !e.IsDisposed, () => SuiciderEnemy.PoolConstructor());
            ItemEnemyPool = new Pool<ItemEnemy>(10, true, e => !e.IsDisposed, () => ItemEnemy.PoolConstructor());
            ZigZagEnemyPool = new Pool<ZigZagEnemy>(130, true, e => !e.IsDisposed, () => ZigZagEnemy.PoolConstructor());
            BombardEnemyPool = new Pool<BombardEnemy>(60, true, e => !e.IsDisposed, () => BombardEnemy.PoolConstructor());
            Dual45EnemyPool = new Pool<Dual45Enemy>(60, true, e => !e.IsDisposed, () => Dual45Enemy.PoolConstructor());
            SideEnemyPool = new Pool<SideEnemy>(10, true, e => !e.IsDisposed, () => SideEnemy.PoolConstructor());
            // Explosions
            ExplosionPool = new Pool<Visual>(100, true, v => !v.IsDisposed, () => Visual.PoolConstructor());
            // Scrap station
            ScrapStationPool = new Pool<ScrapStation>(2, true, s => !s.IsDisposed, () => ScrapStation.PoolConstructor());
            #endregion

            Players.Add(new Player());

            // Projectile pool
            for (int i = 0; i < PROJECTILE_POOL_MAX; i++)
                ProjectilePool.Add(new BaseProjectile());

            // Pickup pool
            for (int i = 0; i < PICKUP_MAX; i++)
                PickupPool.Add(new Pickup());

            // Increase the spawn delay every 5 waves
            int spawnDelayIncs = WaveNr / 5;
            for (int i = 0; i < spawnDelayIncs; i++)
                SpawnDelayTimer.TimeInMS += SPAWN_DELAY_INCREASE;

            // Add starting resources
            for (int i = 1; i <= startingWave; i++)
            {
                if (i < 5)
                {
                    for (int j = 0; j < Players.Count; j++)
                        Players[j].Score += 150;
                }
                else
                {
                    for (int j = 0; j < Players.Count; j++)
                        Players[j].Score += 500;
                }
                // Limit score to ~15000
                if (Players[0].Score >= 15000)
                    break;
            }

            // Spawn scrap station if starting wave # >=20
            if (startingWave >= 20)
                SpawnScrapStation();

            #region Player
            PlayerShip ps = new PlayerShip(PlayerIndex.One, new Vector2(500, 400), Players[0]);
            ps.SetPlaneType(planeType, shipName);

            // Bonus speed & shield regen
            int bonusSpeedUpgrades = startingWave / 10;
            for (int i = 0; i < bonusSpeedUpgrades; i++)
            {
                ps.UpgradeSpeed();
                ps.UpgradeShieldRegen();
            }
            // Add mg
            MG1 mg = MGPool.New();
            mg.Initialize(new Vector2(5, ps.Height - 20), new Vector2(ps.Width - 5 - 8, ps.Height - 20), ps.Owner);
            ps.Guns.Add(mg);

            Entities.Add(ps);
            PlayerShips.Add(ps);
            #endregion

            // Pause
            PauseLocation = Common.CenterString(PausedFont, PAUSE_TEXT, Engine.Instance.Width, Engine.Instance.Height);

            // Collect Garbage
            System.GC.Collect();
        }

        public void PlayMusic()
        {
            MP3MusicMgr.Instance.PlayMusic(Music);
        }

        public void AddProjectile(eProjectile projectileType, Vector2 location, Vector2 direction, Player owner, int texIdx, Color drawColor, int tier)
        {
            BaseProjectile p = ProjectilePool[ProjectilePool.Count - 1];
            p.Initialize(projectileType, location, direction, owner, texIdx, tier);
            p.DrawColor = drawColor;
            Entities.Add(p);
            ProjectilePool.Remove(p);
        }

        public void LaunchNuclearBomb(PlayerShip ship)
        {
            NBombWasUsedThisWave = true;

            for (int i = 0; i < Entities.Count; i++)
            {
                BaseEnemy e = Entities[i] as BaseEnemy;
                if (e != null)
                {
                    if (e.Location.Y + e.AABB.Height > 0)
                        e.TakeDamage(999, ship.Owner);
                }
            }
            ABH.Reset();
        }

        public void AddPickup(ePickupType type, Vector2 location)
        {
            PickupPool[0].Initialize(type, location);
            Pickups.Add(PickupPool[0]);
            PickupPool.RemoveAt(0);
        }

        public void AddVisual(eVisual type, Vector2 centerLoc)
        {
            switch (type)
            {
                case eVisual.Explosion01:
                    Visual v = ExplosionPool.New();
                    v.Initialize(type,centerLoc);
                    Visuals.Add(v);
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public PlayerShip GetNearestPlayer(Vector2 location)
        {
            return PlayerShips[0];
        }

        public BaseEnemy GetRandomEnemy()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                BaseEnemy result = Entities[i] as BaseEnemy;
                if (result != null)
                    return result;
            }
            return null;
        }

        void SpawnClearBonus()
        {
            // Checks for not allowing the bonus
            if (NBombWasUsedThisWave)
                return;
            for (int i = 0; i < Entities.Count; i++)
            {
                if (Entities[i] is BaseEnemy)
                    return;
            }

            SpawnClrBonusIsShowing = true;
            SpawnClrBonusX = Engine.Instance.Width;
            SpawnClrBonusSB.Remove(0, SpawnClrBonusSB.Length);
            int bonus = (int)(WaveNr * 200*ScoreModifier);
            SpawnClrBonusSB.Append(SpawnClrBonusPrefix);
            SpawnClrBonusSB.Append(bonus);
            SpawnClrBonusSB.Append(SpawnClrBonusSuffix);

            // Add bonus
            for (int i = 0; i < Players.Count; i++)
                Players[i].Score += bonus;

            // Victory sound
            Engine.Instance.Audio.PlaySound(AudioConstants.Victory);
        }

        void SpawnScrapStation()
        {
            ScrapStation ss = ScrapStationPool.New();
            ss.Initialize(new Vector2(Maths.RandomNr(0, Engine.Instance.Width - 128), Maths.RandomNr(-256, -174)));
            ActiveScrapStation = ss;
        }

        void Spawn()
        {
            if (SpawnDelayTimer.IsDone || IsFirstSpawn)
            {
                // Increase the spawn delay every 5 waves
                if (WaveNr % 5 == 0)
                    SpawnDelayTimer.TimeInMS += SPAWN_DELAY_INCREASE;
                // Reset the spawn delay timer
                SpawnDelayTimer.Reset();

                if (WaveNr < 60 && !IsFirstSpawn)
                {
                    WaveNr++;
                    WaveNrSB.Remove(0, WaveNrSB.Length);
                    WaveNrSB.Append(WaveNr);
                    SpawnClearBonus();
                    NBombWasUsedThisWave = false;
                }
                IsFirstSpawn = false;

                #region Clean Gun and Enemy Pools
                CleanGunPools();
                StraightEnemyPool.CleanUp();
                SuiciderEnemyPool.CleanUp();
                ZigZagEnemyPool.CleanUp();
                ItemEnemyPool.CleanUp();
                DualMissile45Pool.CleanUp();
                BombardEnemyPool.CleanUp();
                #endregion

                #region Interest
                bool InterestWasReturned = false;
                foreach (Player p in Players)
                {
                    for (int i = 0; i < p.Interests.Count; i++)
                    {
                        p.Interests[i]--;
                        if (p.Interests[i] == 0)
                        {
                            p.Score += (int)(10000 * Maths.RandomNr(115, 130) / (float)100);
                            p.Interests.RemoveAt(i);
                            i--;
                            InterestWasReturned = true;
                        }
                    }
                }
                if (InterestWasReturned)
                    Engine.Instance.Audio.PlaySound(AudioConstants.CoinDrop);
                #endregion

                int spawnNr = Maths.RandomNr(1, 7 + WaveNr / 2);
                // Regular
                if (WaveNr <= 5 && spawnNr < 5)
                    spawnNr = 5;
                for (int i = 0; i < spawnNr; i++)
                {
                    StraightEnemy se = StraightEnemyPool.New();
                    se.Initialize(new Vector2(Maths.RandomNr(0, Engine.Instance.Width - 64), Maths.RandomNr(-512, -57)));
                    Entities.Add(se);
                }

                // Scrapstation
                if (ActiveScrapStation == null && WaveNr >= 12 && (Maths.Chance(10) || (ItemEnemy.LastWaveSpawn <= (WaveNr - 10))))
                    SpawnScrapStation();

                // Side
                if (WaveNr >= 10)
                {
                    spawnNr = Maths.RandomNr(0, (WaveNr / 10) + 1);
                    for (int i = 0; i < spawnNr; i++)
                    {
                        SideEnemy se = SideEnemyPool.New();
                        se.Initialize();
                        Entities.Add(se);
                    }
                }

                // Suicide
                if (WaveNr >= 20)
                {
                    spawnNr = Maths.RandomNr(0, (WaveNr / 3) - 5);
                    if (spawnNr > 7)
                        spawnNr = 7;
                    for (int i = 0; i < spawnNr; i++)
                    {
                        SuiciderEnemy se = SuiciderEnemyPool.New();
                        se.Initialize(new Vector2(Maths.RandomNr(0, Engine.Instance.Width - 35), Maths.RandomNr(-512, -64)));
                        Entities.Add(se);
                    }
                }

                // Bombard
                if (WaveNr / 3 > 5)
                {
                    spawnNr = Maths.RandomNr(0, (WaveNr / 3) - 5);
                    for (int i = 0; i < spawnNr; i++)
                    {
                        BombardEnemy be = BombardEnemyPool.New();
                        be.Initialize(new Vector2(Maths.RandomNr(0, Engine.Instance.Width - 60), Maths.RandomNr(-512, -64)));
                        Entities.Add(be);
                    }
                }

                // 45/M45 Missile
                if (WaveNr / 2 > 3)
                {
                    spawnNr = Maths.RandomNr(0, -3 + WaveNr / 2);
                    for (int i = 0; i < spawnNr; i++)
                    {
                        Dual45Enemy de45 = Dual45EnemyPool.New();
                        de45.Initialize(new Vector2(Maths.RandomNr(0, Engine.Instance.Width - 43), Maths.RandomNr(-512, -64)));
                        Entities.Add(de45);
                    }
                }

                // Item enemy
                for (int i = 0; i < 3; i++)
                {
                    if (Maths.Chance(5))
                    {
                        ItemEnemy.LastWaveSpawn = WaveNr;
                        ItemEnemy ie = ItemEnemyPool.New();
                        ie.Initialize(new Vector2(Maths.RandomNr(0, Engine.Instance.Width - 48), Maths.RandomNr(-512, -112)));
                        Entities.Add(ie);
                    }
                }

                // Zigzag
                if (WaveNr / 2 > 3)
                {
                    spawnNr = Maths.RandomNr(0, -3 + WaveNr / 2);
                    for (int i = 0; i < spawnNr; i++)
                    {
                        ZigZagEnemy zze = ZigZagEnemyPool.New();
                        zze.Initialize(new Vector2(Maths.RandomNr(0, Engine.Instance.Width - 57), Maths.RandomNr(-256, -64)));
                        Entities.Add(zze);
                    }
                }
            }
        }

        public void CleanGunPools()
        {
            MGPool.CleanUp();
            AutoAimPool.CleanUp();
            MissilePool.CleanUp();
            DualMissile45Pool.CleanUp();
            BoomPool.CleanUp();
        }

        public void Update(GameTime gameTime)
        {
            Engine.Instance.Audio.Update(gameTime);

            if (!IsPaused)
            {
                // Alow exit to main menu
                if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultCancelKey, InputMgr.Instance.DefaultCancelButton))
                {
                    Engine.Instance.ActiveState = new MainMenu(true);
                    return;
                }

                // ABH
                if(!ABH.IsDone)
                    ABH.Update();

                // Spawn
                SpawnDelayTimer.Update(gameTime);
                Spawn();

                // Spawn bonus
                if (SpawnClrBonusIsShowing)
                {
                    SpawnClrBonusX -= 6f;
                    if (SpawnClrBonusX < -400)
                        SpawnClrBonusIsShowing = false;
                }

                // Clear broadphase
                BroadPhase.Instance.ClearEntities();

                // Update entities
                for (int i = 0; i < Entities.Count; i++)
                    Entities[i].Update(gameTime);

                // Pools
                ExplosionPool.CleanUp();

                // Scrapstation
                if (ActiveScrapStation != null)
                {
                    ActiveScrapStation.Update(gameTime);
                    if (ActiveScrapStation.IsDisposed)
                        ActiveScrapStation = null;
                }

                // update pickups
                for (int i = 0; i < Pickups.Count; i++)
                    Pickups[i].Update(gameTime);

                for (int i = 0; i < Entities.Count; i++)
                {
                    if (Entities[i].IsDisposed)
                    {
                        // Projectile pool
                        BaseProjectile p = Entities[i] as BaseProjectile;
                        if (p != null)
                            ProjectilePool.Add(p);

                        Entities.RemoveAt(i);
                        i--;
                    }
                }

                // Visuals
                for (int i = 0; i < Visuals.Count; i++)
                {
                    Visuals[i].Update(gameTime);
                    if (Visuals[i].IsDisposed)
                    {
                        Visuals.RemoveAt(i);
                        i--;
                    }
                }

                // Pickup pool
                for (int i = 0; i < Pickups.Count; i++)
                {
                    if (Pickups[i].IsDisposed)
                    {
                        PickupPool.Add(Pickups[i]);
                        Pickups.RemoveAt(i);
                        i--;
                    }
                }

                // Collision
                HandleCollision();
                // Background scroller
                ScrollBG.Update(gameTime);
            }
            if (InputMgr.Instance.IsPressed(null, Keys.P, Buttons.Y))
                IsPaused = !IsPaused;
        }

        void PickupWeapon(PlayerShip pShip, eEnemyGunType gunType)
        {
            BaseGun wpn = pShip.GetGun(gunType);
            if (wpn != null)
            {
                if (wpn.CanUpgradeTier)
                {
                    wpn.UpgradeTier();
                    pShip.UpdateAquiredGuns(); // This is needed to set the background box from green to purple.
                }
                else
                    pShip.Heal(DOUBLE_PICKUP_HEAL_AMOUNT);
            }
            else
                pShip.AddGun(gunType);
        }

        void HandleCollision()
        {
            #region Pickups
            foreach (PlayerShip pShip in PlayerShips)
            {
                for (int i = 0; i < Pickups.Count; i++)
                {
                    if (pShip.AABB.Intersects(new Rectangle(Pickups[i].Location.Xi(), Pickups[i].Location.Yi(), 32, 32)))
                    {
                        switch (Pickups[i].PickupType)
                        {
                            case ePickupType.HP:
                                pShip.Heal(65);
                                break;
                            case ePickupType.Boom1:
                                PickupWeapon(pShip, eEnemyGunType.Boom1Enemy);
                                break;
                            case ePickupType.Aim:
                                PickupWeapon(pShip, eEnemyGunType.AutoAim);
                                break;
                            case ePickupType.Missile:
                                PickupWeapon(pShip, eEnemyGunType.Missile);
                                break;
                            case ePickupType.Speed:
                                if(!pShip.UpgradeSpeed())
                                    pShip.Heal(DOUBLE_PICKUP_HEAL_AMOUNT);
                                break;
                            case ePickupType.Shield:
                                if (!pShip.UpgradeShield())
                                    pShip.Heal(DOUBLE_PICKUP_HEAL_AMOUNT);
                                pShip.Shield.CurrentHP += 7;
                                break;
                            case ePickupType.DualMissile:
                                PickupWeapon(pShip, eEnemyGunType.DualMissile45);
                                break;
                            case ePickupType.ShieldRegen:
                                if (!pShip.UpgradeShieldRegen())
                                    pShip.Heal(DOUBLE_PICKUP_HEAL_AMOUNT);
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                        Pickups[i].IsDisposed = true;
                        pShip.Owner.Score += (int)((5 + WaveNr) * Level.Instance.ScoreModifier);
                    }
                }
            }
            #endregion

            #region Scrapstation
            if(ActiveScrapStation != null && !ActiveScrapStation.IsGoingHome)
                if (PlayerShips[0].AABB.Intersects(ActiveScrapStation.AABB))
                {
                    foreach (Rectangle2 rect in PlayerShips[0].CollisionRects)
                    {
                        foreach (Rectangle2 rect2 in ActiveScrapStation.CollisionRects)
                        {
                            if (rect.Absolute.Intersects(rect2.Absolute))
                            {
                                ActiveScrapStation.Visit(PlayerShips[0]);
                                return;
                            }
                        }
                    }
                }
            #endregion

            #region Other Collisions
            // A note about the 'is' operator performance: http://stackoverflow.com/questions/686412/c-is-operator-performance
            for (int y = 0; y < BroadPhase.GRID_CNT; y++)
            {
                for (int x = 0; x < BroadPhase.GRID_CNT; x++)
                {
                    if (BroadPhase.Instance.Blocks[x, y].Entities.Count > 0)
                    {
                        for (int i = 0; i < BroadPhase.Instance.Blocks[x, y].Entities.Count; i++)
                        {
                            PlayerShip pShip = BroadPhase.Instance.Blocks[x, y].Entities[i] as PlayerShip;
                            if (pShip != null)
                            {
                                for (int j = 0; j < BroadPhase.Instance.Blocks[x, y].Entities.Count; j++)
                                {
                                    // Player <--> enemy projectile
                                    BaseProjectile p = BroadPhase.Instance.Blocks[x, y].Entities[j] as BaseProjectile;
                                    if (p != null)
                                    {
                                        if (p.Owner == null && !p.IsDisposed && pShip.AABB.Intersects(BroadPhase.Instance.Blocks[x, y].Entities[j].AABB))
                                        {
                                            foreach (Rectangle2 rect in pShip.CollisionRects)
                                            {
                                                if (p.AABB.Intersects(rect.Absolute))
                                                {
                                                    pShip.TakeDamage(p.Damage);
                                                    p.IsDisposed = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    // Player <--> enemy collision
                                    else
                                    {
                                        BaseEnemy e = BroadPhase.Instance.Blocks[x, y].Entities[j] as BaseEnemy;
                                        if (e != null)
                                        {
                                            if (!e.IsDisposed && pShip.AABB.Intersects(e.AABB))
                                            {
                                                foreach (Rectangle2 rect in pShip.CollisionRects)
                                                {
                                                    if (e.AABB.Intersects(rect.Absolute))
                                                    {
                                                        pShip.TakeDamage(e.CollisionDamage);
                                                        pShip.Owner.Kills++;
                                                        e.Die();
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            // enemy <--> player projectile
                            else if (BroadPhase.Instance.Blocks[x, y].Entities[i] is BaseEnemy)
                            {
                                BaseEnemy e = (BaseEnemy)BroadPhase.Instance.Blocks[x, y].Entities[i];

                                for (int j = 0; j < BroadPhase.Instance.Blocks[x, y].Entities.Count; j++)
                                {
                                    BaseProjectile p = BroadPhase.Instance.Blocks[x, y].Entities[j] as BaseProjectile;
                                    if (p != null)
                                    {
                                        if (p.Owner != null && e.AABB.Intersects(p.AABB) && !p.IsDisposed)
                                        {
                                            e.TakeDamage(p.Damage, p.Owner);
                                            p.IsDisposed = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        public void Draw()
        {
            ScrollBG.Draw();
            //BroadPhase.Instance.DebugDraw();

            for (int i = 0; i < Entities.Count; i++)
                Entities[i].DrawShadow();

            if (ActiveScrapStation != null)
            {
                ActiveScrapStation.DrawShadow();
                ActiveScrapStation.Draw();
            }

            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Draw();
                //Entities[i].DebugDraw();
            }

            for (int i = 0; i < Pickups.Count; i++)
                Pickups[i].Draw();

            for (int i = 0; i < Visuals.Count; i++)
                Visuals[i].Draw();

            if (IsPaused)
            {
                Engine.Instance.SpriteBatch.Draw(Common.White1px50Trans, Engine.Instance.ScreenArea, Color.Black);
                Engine.Instance.SpriteBatch.DrawString(PausedFont, PAUSE_TEXT, PauseLocation, Color.White);
            }

            // ABH
            if (!ABH.IsDone)
                Engine.Instance.SpriteBatch.Draw(Common.White1px, Engine.Instance.ScreenArea, new Color(255, 255, 255,ABH.AlphaValue));

            #region GUI
            // Wave timer
            Engine.Instance.SpriteBatch.DrawString(WaveFont, WaveNrSB, WaveNrLoc, Color.Goldenrod);
            
            // Spawn Timer
            SpawnTimerSB.Remove(0, SpawnTimerSB.Length);
            SpawnTimerSB.Append(SpawnDelayTimer.TimeLeftInSecRoundedUp);
            if (SettingsMgr.Instance.ShowSpawnTimer)
                Engine.Instance.SpriteBatch.DrawString(WaveFont, SpawnTimerSB, SpawnTimerLoc, Color.LightBlue);

            // Player GUI
            for (int i = 0; i < Players.Count; i++)
                Players[i].DrawGUI();

            // Mouse if applicable
            if (SettingsMgr.Instance.ControlType1 == eControlType.Mouse)
                InputMgr.Instance.Mouse.Draw(Engine.Instance.SpriteBatch);

             // Spawn bonus
            if (SpawnClrBonusIsShowing)
            {
                Engine.Instance.SpriteBatch.DrawString(SpawnClrBonusFont, SpawnClrBonusText, new Vector2(SpawnClrBonusX, 180), Color.White);
                Engine.Instance.SpriteBatch.DrawString(SpawnClrBonusFont, SpawnClrBonusSB, new Vector2(SpawnClrBonusX + 90, 245), Color.White);
            }
            #endregion
        }
    }
}