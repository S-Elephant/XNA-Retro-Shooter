using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using XNALib.Controls;

namespace RetroShooter
{
    public class EnterNameMenu : IActiveState
    {
        TextBox TxtName;
        static readonly SpriteFont TextboxFont = Common.str2Font("Font01_18");
        int Score;
        static readonly Texture2D Texture = Common.str2Tex("Menu/spaceBG");
        const string MenuTitle = "Enter Your Name";
        int Kills;
        int ScoreMod;
        string ShipType;

        public EnterNameMenu(int score, int kills, int scoreMod, string shipType)
        {
            Score = score;
            Kills = kills;
            ScoreMod = scoreMod;
            ShipType = shipType;

            if (ControlMgr.Instance == null)
                ControlMgr.Instance = new ControlMgr(Engine.Instance.SpriteBatch);
            TxtName = new TextBox(new Rectangle(Engine.Instance.Width/2-100, 256, 200, 28), TextboxFont)
            {
                HasFocus = true,
                MaxLength = 11,
                ForeColor = Color.White,
                Caption = new StringBuilder("Pilot:"),
                CaptionColor = Color.White,
                BGColor = new Color(128,128,128,64)
            };

            // Stop level music
            MP3MusicMgr.Instance.StopMusic();

            // Victory sound
            Engine.Instance.Audio.PlaySound(AudioConstants.Victory);
        }

        public void Update(GameTime gameTime)
        {
            TxtName.Update(gameTime);

            if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultConfirmKey, InputMgr.Instance.DefaultConfirmButton))
            {
                HighScoreMenu.HighScoreMgr.AttemptScore(TxtName.Text, Score, new HighScoreColumn("dateTime", DateTime.Now.ToString("MM/dd/yyyy HH:mm")), new HighScoreColumn("kills", Kills.ToString()), new HighScoreColumn("scoreMod", ScoreMod.ToString() + "%"), new HighScoreColumn("shipType", ShipType));
                Engine.Instance.ActiveState = new HighScoreMenu(true);
            }
        }

        public void Draw()
        {
            Engine.Instance.SpriteBatch.Draw(Texture, Engine.Instance.ScreenArea, Color.White);
            TxtName.Draw();
            Engine.Instance.SpriteBatch.DrawString(MainMenu.TitleFont, MenuTitle, Common.CenterStringX(MainMenu.TitleFont, MenuTitle, Engine.Instance.Width, 70), Color.White);
        }
    }
}