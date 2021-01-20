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
    public class Ability
    {
        public float MaxValue;
        public float CurrentValue;
        public float UpgAmount;

        public bool CanUpgrade { get { return CurrentValue < MaxValue; } }

        public Ability(float maxValue, float currentValue, float upgAmount)
        {
            MaxValue = maxValue;
            CurrentValue = currentValue;
            UpgAmount = upgAmount;
        }

        public bool Upgrade()
        {
            if (CurrentValue == MaxValue)
                return false;
            CurrentValue += UpgAmount;
            if (CurrentValue > MaxValue)
                CurrentValue = MaxValue;
            return true;
        }
    }

    public class PlayerShip : BaseShip, IEntity
    {
        #region Members
        #region Misc
        const string ImpactFX = "impact01";
        PlayerIndex ControllerIdx;
        float Velocity = 6.9f;
        float MaxVelocity = 11.5f;
        const float SpeedUpgradeAmount = 0.9f;
        public Vector2 CenterLoc { get { return new Vector2(Location.X + AABB.Width / 2, Location.Y + AABB.Height / 2); } }
        static readonly Texture2D AfterBurner = Common.str2Tex("Player/afterBurner");
        static Vector2 AfterBurnerRelLoc;
        public int NuclearCnt = 0;
        public int MaxNuclearCnt = 3;
        static readonly Texture2D RoundGUILeft = Common.str2Tex("Sprites/roundBarLeft130px_11");
        static readonly Vector2 RoundGUIOffsetLeft = new Vector2(-36.5f,-33);
        static readonly Vector2 RoundGUIOffsetRight = new Vector2(-36.5f+130/2-64, -33);
        static Color RoundGUIHPColorLow = new Color(255, 0, 0, 64);
        static Color RoundGUIHPColorMid = new Color(255, 66, 0, 64);
        static Color RoundGUIHPColorHigh = new Color(0, 255, 0, 64);
        static Color RoundGUIShieldColor = new Color(0, 240, 220, 64);
        ePlaneType PlaneType;
        StringBuilder ShipName;
        #endregion
        
        #region HP & Shield
        public HitPoints HP = new HitPoints(300, 300, 0);
        SimpleTimer HPRegenTimer = new SimpleTimer(30);
        public Ability HPRegen = new Ability(0.05f, 0, 0.05f);

        /// <summary>
        /// Only read this one public
        /// </summary>
        public float ShieldRegen = 0.25f;
        const float ShieldRegenUpgIncrease = 0.15f;
        float MaxShield = 60;
        public HitPoints Shield = new HitPoints(30, 30, 0);
        SimpleTimer ShieldRegenTimer = new SimpleTimer(60);
        float MaxShieldRegen = 0.8f;
        SimpleASprite ShieldSprite = new SimpleASprite(Common.InvalidVector2, "Sprites/shield128px_4", 128, 128, 4, 1, 96) { ExtraDrawOffset = new Vector2(-36, -36) };
        SimpleASprite ShieldSpriteT2 = new SimpleASprite(Common.InvalidVector2, "Sprites/shieldT2128px_4", 128, 128, 4, 1, 96) { ExtraDrawOffset = new Vector2(-36, -36) };
        const float MIN_SHIELD_HP_FOR_SPRITE = 3;
        #endregion

        public List<BaseGun> Guns = new List<BaseGun>(5);
        public Player Owner;

        public int Width { get { return Animation.Width; } }
        public int Height { get { return Animation.Height; } }

        #region CanUpgrade
        public bool CanUpgShieldHP { get { return Shield.MaxHP < MaxShield; } }
        public bool CanUpgShieldRegen { get { return ShieldRegen < MaxShieldRegen; } }
        public bool CanUpgSpeed { get { return Velocity < MaxVelocity; } }
        #endregion

        /// <summary>
        /// The amount of damage taken. This amount is used to determine when to randomly destroy a player's gun.
        /// </summary>
        float GunLoseDamTakenCnt = 0;
        float GunLoseDamTakenLimit = 35;
        #endregion

        public PlayerShip(PlayerIndex controllerIdx, Vector2 startLocation, Player owner)
        {
            Animation = Common.str2Tex("Ships/player"); // default
            Shadow = Common.str2Tex("Shadows/player"); // default
            AfterBurnerRelLoc = new Vector2(Animation.Width / 2 - AfterBurner.Width / 2+1, Animation.Height-8);

            ControllerIdx = controllerIdx;
            AABB = new Rectangle(Location.Xi(), Location.Yi(), Animation.Width, Animation.Height);
            Location = startLocation;
            Owner = owner;
            Owner.Ship = this;
            Owner.HPBar.Percentage = HP.LifeLeftPercentage;
            Owner.ShieldBar.Percentage = Shield.LifeLeftPercentage;

            RoundGUIHPColorHigh.A = RoundGUIHPColorMid.A = RoundGUIHPColorLow.A = RoundGUIShieldColor.A = SettingsMgr.Instance.RoundGUIAlpha;
        }

        public void SetPlaneType(ePlaneType planeType, string shipName)
        {
            PlaneType = planeType;
            ShipName = new StringBuilder(shipName);
            switch (planeType)
            {
                case ePlaneType.Normal:
                    Animation = Common.str2Tex("Ships/player");
                    Shadow = Common.str2Tex("Shadows/player");
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(23, 0, 11, 20)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(8, 19, 41, 16)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(0, 35, 57, 9)));
                    Owner.Score += 500;
                    break;
                case ePlaneType.Dmg:
                    Animation = Common.str2Tex("Ships/playerDmg");
                    Shadow = Common.str2Tex("Shadows/playerDmg");
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(5, 0, 47, 35)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(0, 17, 57, 18)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(14, 35, 30, 9)));

                    Owner.DmgMod += 0.41f;
                    MaxVelocity -= 4f;
                    Velocity -= 0.7f;
                    break;
                case ePlaneType.Speed:
                    Animation = Common.str2Tex("Ships/playerVelocity");
                    Shadow = Common.str2Tex("Shadows/player");
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(23, 0, 11, 20)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(8, 19, 41, 16)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(0, 35, 57, 9)));

                    Owner.DmgMod -= 0.05f;
                    MaxVelocity += 3.0f;
                    Velocity += 2.0f;
                    
                    HP.CurrentHP = HP.MaxHP -= 50;

                    MaxShield += 15;
                    Shield.MaxHP = MaxShield;
                    Shield.HealFull();
                    Owner.Score += 1500;
                    break;
                case ePlaneType.Regen:
                    Animation = Common.str2Tex("Ships/playerRegen");
                    Shadow = Common.str2Tex("Shadows/player");
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(23, 0, 11, 20)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(8, 19, 41, 16)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(0, 35, 57, 9)));

                    MaxShieldRegen += 0.4f;
                    ShieldRegen += 0.3f;
                    MaxShield += 15;
                    Shield.MaxHP = MaxShield;
                    Shield.HealFull();
                    HPRegen.MaxValue += 0.4f;
                    HPRegen.CurrentValue += 0.25f;
                    break;
                case ePlaneType.Tough:
                    Animation = Common.str2Tex("Ships/playerDmg");
                    Shadow = Common.str2Tex("Shadows/playerDmg");
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(5, 0, 47, 35)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(0, 17, 57, 18)));
                    CollisionRects.Add(new Rectangle2(Location, new Rectangle(14, 35, 30, 9)));

                    Owner.DmgMod -= 0.1f;
                    HP.MaxHP = 500;
                    HP.HealFull();
                    MaxShield += 35;
                    Shield.MaxHP += 10;
                    Shield.HealFull();
                    MaxVelocity -= 3.5f;
                    Velocity -= 0.6f;
                    HPRegen.MaxValue += 0.15f;
                    HPRegen.CurrentValue += 0.05f;
                    GunLoseDamTakenLimit *= 3;
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        #region Upgrades
        public void UpgradeOffensiveTier()
        {
            if (Owner.OffensiveTier < Player.MAX_OFFENSIVE_TIER)
            {
                Owner.OffensiveTier++;
                Guns[0].IsDisposed = true;
                Guns[0] = Level.Instance.MG2Pool.New();
                ((MG2)Guns[0]).Initialize(new Vector2(5, Animation.Height - 20), new Vector2(Animation.Width - 5 - 8, Animation.Height - 20), Owner);
                Owner.AquiredWeapons[0] = 2;
            }
        }

        public void UpgradeDefensiveTier()
        {
            if (Owner.DefensiveTier < Player.MAX_DEFENSIVE_TIER)
            {
                Owner.DefensiveTier++;
                MaxNuclearCnt++;
                MaxShield += 25;
                MaxShieldRegen += 0.1f;
                ShieldSprite = ShieldSpriteT2;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>true when an upgrade was done.</returns>
        public bool UpgradeSpeed()
        {
            if (Velocity == MaxVelocity)
                return false;
            Velocity += SpeedUpgradeAmount;
            if (Velocity > MaxVelocity)
                Velocity = MaxVelocity;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true when an upgade was done.</returns>
        public bool UpgradeShield()
        {
            if (Shield.MaxHP == MaxShield)
                return false;
            Shield.MaxHP += 7f;
            if (Shield.MaxHP > MaxShield)
                Shield.MaxHP = MaxShield;
            Owner.ShieldBar.Percentage = Shield.LifeLeftPercentage;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true when an upgade was done.</returns>
        public bool UpgradeShieldRegen()
        {
            if (ShieldRegen == MaxShieldRegen)
                return false;
            ShieldRegen += ShieldRegenUpgIncrease;
            if (ShieldRegen > MaxShieldRegen)
                ShieldRegen = MaxShieldRegen;
            return true;
        }
        #endregion

        #region Damage & Heal
        public void HealShieldFull()
        {
            Shield.HealFull();
            Owner.ShieldBar.Percentage = Shield.LifeLeftPercentage;
        }

        public void HealHPFull()
        {
            HP.HealFull();
            Owner.HPBar.Percentage = HP.LifeLeftPercentage;
        }

        public void Heal(float amount)
        {
            HP.CurrentHP += amount;
            GunLoseDamTakenCnt -= amount / 2;
            if (GunLoseDamTakenCnt < 0)
                GunLoseDamTakenCnt = 0;
            Owner.HPBar.Percentage = HP.LifeLeftPercentage;
        }

        public void TakeDamage(float amount)
        {
            if (Shield.CurrentHP > amount)
            {
                Shield.CurrentHP -= amount;
                Owner.ShieldBar.Percentage = Shield.LifeLeftPercentage;
                //Engine.Instance.Audio.PlaySound(AudioConstants.ShieldHit);
            }
            else
            {
                Shield.CurrentHP -= amount;
                Owner.ShieldBar.Percentage = Shield.LifeLeftPercentage;
                amount -= Shield.CurrentHP;
                HP.CurrentHP -= amount;
                GunLoseDamTakenCnt += amount; // Only add when the hp took damage.
                Owner.HPBar.Percentage = HP.LifeLeftPercentage;

                Engine.Instance.Audio.PlaySound(AudioConstants.Impact);
            }
                
            if (HP.CurrentHP <= 0)
                Die();
            else
            {
                if (GunLoseDamTakenCnt >= GunLoseDamTakenLimit)
                {
                    GunLoseDamTakenCnt = 0;
                    if (Guns.Count > 2 && Maths.Chance(50))
                    {
                        Guns.RemoveAt(Maths.RandomNr(2, Guns.Count - 1));
                        UpdateAquiredGuns();
                        Level.Instance.CleanGunPools();
                        Level.Instance.AddVisual(eVisual.Explosion01, CenterLoc);
                    }
                }
            }
        }
        #endregion

        #region Guns
        public BaseGun GetGun(eEnemyGunType gunType)
        {
            return Guns.FirstOrDefault(g => g.GunType == gunType);
        }

        public void AddGun(eEnemyGunType gunType)
        {
            switch (gunType)
            {
                case eEnemyGunType.AutoAim:
                    AutoAim aa = Level.Instance.AutoAimPool.New();
                    aa.Initialize(new Vector2(Animation.Width / 2 - 8, Animation.Height / 2 - 8), Owner);
                    Guns.Add(aa);
                    break;
                case eEnemyGunType.Boom1Enemy:
                    Boom1 b = Level.Instance.BoomPool.New();
                    b.Initialize(new Vector2(Animation.Width / 2 - 9, 0), Owner);
                    Guns.Add(b);
                    break;
                case eEnemyGunType.MG1:
                    throw new Exception("MG Can not be added");
                case eEnemyGunType.Missile:
                    Missile m = Level.Instance.MissilePool.New();
                    m.Initialize(new Vector2(Animation.Width / 2 - 9, Animation.Height / 2 - 11), Owner);
                    Guns.Add(m);
                    break;
                case eEnemyGunType.DualMissile45:
                    DualMissile45 dm = Level.Instance.DualMissile45Pool.New();
                    dm.Initialize(new Vector2(Animation.Width / 2 - 11, 5), Owner);
                    Guns.Add(dm);
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
            UpdateAquiredGuns();
        }

        public void UpdateAquiredGuns()
        {
            for (int i = 1; i < 5; i++)
                Owner.AquiredWeapons[i] = 0;
            for (int i = 1; i < Guns.Count; i++)
            {
                switch (Guns[i].GunType)
                {
                    case eEnemyGunType.AutoAim:
                        Owner.AquiredWeapons[1] = Guns[i].Tier;
                        break;
                    case eEnemyGunType.Boom1Enemy:
                        Owner.AquiredWeapons[2] = Guns[i].Tier;
                        break;
                    case eEnemyGunType.MG1:
                        throw new Exception("Should never occur.");
                    case eEnemyGunType.Missile:
                        Owner.AquiredWeapons[3] = Guns[i].Tier;
                        break;
                    case eEnemyGunType.DualMissile45:
                        Owner.AquiredWeapons[4] = Guns[i].Tier;
                        break;
                    default:
                        throw new CaseStatementMissingException();
                }
            }
        }
        #endregion

        void Die()
        {
            // Add Kill Score
            Owner.Score += (int)((Owner.Kills * 12) * Level.Instance.ScoreModifier);

            // Achievements
            if (!AchievementsMgr.Instance.BeatedLvl30 && Level.Instance.WaveNr > 30)
                AchievementsMgr.Instance.BeatedLvl30 = true;
            if (!AchievementsMgr.Instance.BeatedLvl60 && Level.Instance.WaveNr >= 60) // Also unlock at lvl 59...
                AchievementsMgr.Instance.BeatedLvl60 = true;

            if (!AchievementsMgr.Instance.Killed100 && Owner.Kills > 100)
                AchievementsMgr.Instance.Killed100 = true;
            if (!AchievementsMgr.Instance.Killed500 && Owner.Kills > 500)
                AchievementsMgr.Instance.Killed500 = true;
            if (!AchievementsMgr.Instance.Killed1000 && Owner.Kills > 1000)
                AchievementsMgr.Instance.Killed1000 = true;
            AchievementsMgr.Instance.Save();

            if (HighScoreMenu.HighScoreMgr.IsHighScore(Owner.Score))
                Engine.Instance.ActiveState = new EnterNameMenu(Owner.Score, Owner.Kills, (int)(Level.Instance.ScoreModifier * 100), ShipName.ToString());
            else
                Engine.Instance.ActiveState = new MainMenu(true);
        }

        public override void Update(GameTime gameTime)
        {
            #region Movement
            Vector2 newLoc = Location;
            Vector2 md = Vector2.Zero;
            if (SettingsMgr.Instance.ControlType1 == eControlType.Keyboard)
            {
                if (InputMgr.Instance.IsDown(ControllerIdx, Keys.Up, Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp))
                    md += new Vector2(0, -1);
                if (InputMgr.Instance.IsDown(ControllerIdx, Keys.Right, Buttons.DPadRight, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight))
                    md += new Vector2(1, 0);
                if (InputMgr.Instance.IsDown(ControllerIdx, Keys.Down, Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown))
                    md += new Vector2(0, 1);
                if (InputMgr.Instance.IsDown(ControllerIdx, Keys.Left, Buttons.DPadLeft, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft))
                    md += new Vector2(-1, 0);
            }
            else
            {
                if (Vector2.Distance(CenterLoc, InputMgr.Instance.Mouse.Location) > Velocity)
                    md = Maths.GetMoveDir(CenterLoc, InputMgr.Instance.Mouse.Location);
            }

            newLoc += md * Velocity;

            // Clamp
            if (newLoc.X < 0)
                newLoc.X = 0;
            if (newLoc.X + AABB.Width > Engine.Instance.Width)
                newLoc.X = Engine.Instance.Width - AABB.Width;
            if (newLoc.Y < 0)
                newLoc.Y = 0;
            if (newLoc.Y + AABB.Height > Engine.Instance.Height)
                newLoc.Y = Engine.Instance.Height - AABB.Height;

            Location = newLoc;

            BroadPhase.Instance.AddEntity(this, AABB);
            #endregion

            // Reset weapons
            if (InputMgr.Instance.Keyboard.IsPressed(Keys.X))
            {
                for (int i = 0; i < Guns.Count; i++)
                    Guns[i].ResetDelay();
            }

            // Shoot
            for (int i = 0; i < Guns.Count; i++)
                Guns[i].Update(gameTime, Location);

            // Shield
            ShieldRegenTimer.Update(gameTime);
            if (ShieldRegenTimer.IsDone)
            {
                ShieldRegenTimer.Reset();
                Shield.CurrentHP += ShieldRegen;
                Owner.ShieldBar.Percentage = Shield.LifeLeftPercentage;
            }

            // HP regen
            if (HPRegen.CurrentValue > 0)
            {
                HPRegenTimer.Update(gameTime);
                if (HPRegenTimer.IsDone)
                {
                    HP.CurrentHP += HPRegen.CurrentValue;
                    Owner.HPBar.Percentage = HP.LifeLeftPercentage;
                }
            }

            // Bomb
            if (NuclearCnt > 0)
            {
                if ((SettingsMgr.Instance.ControlType1 == eControlType.Keyboard && InputMgr.Instance.IsPressed(null, Keys.Space, Buttons.A)) ||
                   ((SettingsMgr.Instance.ControlType1 == eControlType.Mouse && InputMgr.Instance.Mouse.RightButtonIsPressed))
                  )
                {
                    NuclearCnt--;
                    Level.Instance.LaunchNuclearBomb(this);
                }
            }

            // Update shield sprite
            if (Shield.CurrentHP >= MIN_SHIELD_HP_FOR_SPRITE)
            {
                ShieldSprite.Update(gameTime);
                ShieldSprite.Location = Location;
            }
        }

        public override void Draw()
        {
            if (Shield.CurrentHP >= MIN_SHIELD_HP_FOR_SPRITE)
                ShieldSprite.Draw(Engine.Instance.SpriteBatch);
            
            // Round GUI
            if (SettingsMgr.Instance.ShowRoundGUI)
            {
                float hpLifeLeftPerc = HP.LifeLeftPercentage;
                
                Color hpColor = RoundGUIHPColorHigh;
                if (hpLifeLeftPerc < 25)
                    hpColor = RoundGUIHPColorLow;
                else if (hpLifeLeftPerc < 50)
                    hpColor = RoundGUIHPColorMid;
                
                Engine.Instance.SpriteBatch.Draw(RoundGUILeft, Location + RoundGUIOffsetLeft, new Rectangle(0 + (int)(hpLifeLeftPerc / 10) * 130, 0, 130, 130), hpColor);
                Engine.Instance.SpriteBatch.Draw(RoundGUILeft, Location + RoundGUIOffsetRight, new Rectangle(0 + (int)(Shield.LifeLeftPercentage / 10) * 130, 0, 130, 130), RoundGUIShieldColor, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
            }

            base.Draw();
        }

        public override void DrawShadow()
        {
            base.DrawShadow();
            Engine.Instance.SpriteBatch.Draw(AfterBurner, Location + AfterBurnerRelLoc, Color.White);
        }
    }
}