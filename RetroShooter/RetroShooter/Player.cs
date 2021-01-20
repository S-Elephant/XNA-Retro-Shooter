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
    public class Player
    {
        #region Members
        #region GUI
        static readonly Texture2D Heart = Common.str2Tex("GUI/heart");
        static readonly Texture2D BarFull = Common.str2Tex("GUI/hpbarFull");
        static readonly Texture2D BarShield = Common.str2Tex("GUI/shieldbarFull");
        static readonly Texture2D BarEmpty = Common.str2Tex("GUI/hpbarEmpty");
        Vector2 BarLocation, ShieldBarLoc, WpnLoc;
        public Bar HPBar, ShieldBar;
        Vector2 BarHeartAddition;
        #endregion

        // Misc
        public PlayerShip Ship;
        public float DmgMod = 1f; // 1f = 100%, 0.5f = 50%, 1.5f = 150%, etc.

        // Nuclear
        static readonly Texture2D NuclearIcon = Common.str2Tex("GUI/nuclear");
        Vector2 NuclearLoc;

        // Interest
        public const int MAX_INTERESTS = 10;
        public List<int> Interests = new List<int>(MAX_INTERESTS);

        // Upgrades
        public int OffensiveTier = 1;
        public int DefensiveTier = 1;
        public const int MAX_OFFENSIVE_TIER = 2;
        public const int MAX_DEFENSIVE_TIER = 2;

        #region Kills
        private int m_Kills = 0;
        public int Kills
        {
            get { return m_Kills; }
            set
            {
                m_Kills = value;
                KillsSB.Remove(0, KillsSB.Length);
                KillsSB.Append(value);
            }
        }
        StringBuilder KillsSB = new StringBuilder("0", 7);
        static readonly Texture2D KillSkull = Common.str2Tex("GUI/killSkull");
        Vector2 KillSkullloc, KillLoc;
        static readonly SpriteFont KillFont = Common.str2Font("KillCnt");
        #endregion

        public int[] AquiredWeapons = new int[5]
        {
            1, // MG
            0, // autoaim
            0, // boomer
            0, // missile
            0 // dual missile
        };

        // Wpn GUI
        static readonly Texture2D[] WpnTextures = new Texture2D[]
        {
            Common.str2Tex("GUI/Weapons/wpnRed"),
            Common.str2Tex("GUI/Weapons/wpnGreen"),
            Common.str2Tex("GUI/Weapons/wpnMG"),
            Common.str2Tex("GUI/Weapons/wpnAutoAim"),
            Common.str2Tex("GUI/Weapons/wpnBoomer"),
            Common.str2Tex("GUI/Weapons/wpnMissile"),
            Common.str2Tex("GUI/Weapons/wpnDualMissile"),
            Common.str2Tex("GUI/Weapons/wpnT2"),
        };

        #region Score
        private int m_Score = 0;
        public int Score
        {
            get { return m_Score; }
            set
            {
                m_Score = value;
                ScoreSB.Remove(0, ScoreSB.Length);
                ScoreSB.Append(DollarSign);
                ScoreSB.Append(value);
            }
        }

        const string DollarSign = "$";
        StringBuilder ScoreSB = new StringBuilder("$:0", 10);
        Vector2 ScoreLoc;
        static readonly SpriteFont ScoreFont = Common.str2Font("Font04_28");
        #endregion
        #endregion

        public Player()
        {
            BarLocation = new Vector2(10, 10);
            ShieldBarLoc = BarLocation + new Vector2(0, BarFull.Height-5);
            WpnLoc = ShieldBarLoc + new Vector2(30, BarShield.Height-3);
            ScoreLoc = new Vector2(10, 25 + WpnLoc.Y);

            HPBar = new Bar(BarFull, new Rectangle(BarLocation.Xi(), BarLocation.Yi(), BarFull.Width, BarFull.Height), Bar.eDirection.Horizontal);
            ShieldBar = new Bar(BarShield, new Rectangle(ShieldBarLoc.Xi(), ShieldBarLoc.Yi(), BarShield.Width, BarShield.Height), Bar.eDirection.Horizontal);
            BarHeartAddition = BarLocation + new Vector2(BarFull.Width + 5, 2);

            KillSkullloc = new Vector2(10, Engine.Instance.Height - KillSkull.Height-10);
            KillLoc = new Vector2(KillSkull.Width + 13, KillSkullloc.Y-7);
            NuclearLoc = KillSkullloc - new Vector2(0, NuclearIcon.Height + 5);
        }

        public void DrawGUI()
        {
            // HP bar
            Engine.Instance.SpriteBatch.Draw(BarEmpty, BarLocation, Color.White);
            HPBar.Draw(Engine.Instance.SpriteBatch);
            // Shield bar
            Engine.Instance.SpriteBatch.Draw(BarEmpty, ShieldBarLoc, Color.White);
            ShieldBar.Draw(Engine.Instance.SpriteBatch);
            // Heart
            Engine.Instance.SpriteBatch.Draw(Heart, BarHeartAddition, Color.White);

            // Weapons
            for (int i = 0; i < 5; i++)
            {
                // BG
                if (AquiredWeapons[i] == 0)
                    Engine.Instance.SpriteBatch.Draw(WpnTextures[0], new Vector2(WpnLoc.X + i * 23, WpnLoc.Y), Color.White);
                else if (AquiredWeapons[i] == 1)
                    Engine.Instance.SpriteBatch.Draw(WpnTextures[1], new Vector2(WpnLoc.X + i * 23, WpnLoc.Y), Color.White);
                else
                    Engine.Instance.SpriteBatch.Draw(WpnTextures[7], new Vector2(WpnLoc.X + i * 23, WpnLoc.Y), Color.White);
                
                Engine.Instance.SpriteBatch.Draw(WpnTextures[2 + i], new Vector2(WpnLoc.X + i * 23, WpnLoc.Y), Color.White);
            }

            // Kills
            Engine.Instance.SpriteBatch.Draw(KillSkull, KillSkullloc, Color.White);
            Engine.Instance.SpriteBatch.DrawString(KillFont, KillsSB, KillLoc, Color.White);

            // Nuclear
            for (int i = 0; i < Ship.NuclearCnt; i++)
                Engine.Instance.SpriteBatch.Draw(NuclearIcon, NuclearLoc + new Vector2(i * 31, 0), Color.White); ;

            // Score
            Engine.Instance.SpriteBatch.DrawString(ScoreFont, ScoreSB, ScoreLoc, Color.White);
        }
    }
}