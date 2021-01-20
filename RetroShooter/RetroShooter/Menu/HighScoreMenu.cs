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
    public class HighScoreMenu : IActiveState
    {
        public static HighScoreMgr2 HighScoreMgr;

        static readonly SpriteFont Font = Common.str2Font("Font01_18");
        static readonly SpriteFont TitleFont = MainMenu.TitleFont;
        float FontHeight;
        public Texture2D Texture = null;
        public Color FontColor = Color.White;
        public Color ColumnFontColor = Color.White;
        static readonly Vector2 Offset = new Vector2(110, 150);

        public static void Init()
        {
            HighScoreMgr = new HighScoreMgr2("HighScores.xml", 10);
        }

        public HighScoreMenu(bool replayMusic)
        {
            if (replayMusic)
                MP3MusicMgr.Instance.PlayMusic("mainTheme");
            FontHeight = Font.MeasureString(Common.MeasureString).Y;
            Texture = Common.str2Tex("Menu/spaceBG");
        }

        public void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.IsPressed(null,InputMgr.Instance.DefaultCancelKey,InputMgr.Instance.DefaultCancelButton) ||
                InputMgr.Instance.IsPressed(null,InputMgr.Instance.DefaultConfirmKey,InputMgr.Instance.DefaultConfirmButton))
                Engine.Instance.ActiveState = new MainMenu(false);
        }

        public void Draw()
        {
            // BG
            if (Texture != null)
                Engine.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);

            // Campaign Title
            Engine.Instance.SpriteBatch.DrawString(TitleFont, "Highscores", Common.CenterStringX(TitleFont, "Highscores", Engine.Instance.Width, 60), ColumnFontColor);

            // Titles
            Engine.Instance.SpriteBatch.DrawString(Font, "Rank", Offset + new Vector2(0, 0), ColumnFontColor);
            Engine.Instance.SpriteBatch.DrawString(Font, "Name", Offset + new Vector2(100, 0), ColumnFontColor);
            Engine.Instance.SpriteBatch.DrawString(Font, "Score", Offset + new Vector2(275, 0), ColumnFontColor);
            Engine.Instance.SpriteBatch.DrawString(Font, "Date & Time", Offset + new Vector2(425, 0), ColumnFontColor);
            Engine.Instance.SpriteBatch.DrawString(Font, "Kills", Offset + new Vector2(700, 0), ColumnFontColor);
            Engine.Instance.SpriteBatch.DrawString(Font, "Mod", Offset + new Vector2(825, 0), ColumnFontColor);
            Engine.Instance.SpriteBatch.DrawString(Font, "Ship", Offset + new Vector2(912, 0), ColumnFontColor);

            // Column Values
            try
            {
                for (int i = 0; i < HighScoreMgr.HighScores.Count; i++)
                {
                    Engine.Instance.SpriteBatch.DrawString(Font, (i + 1).ToString(), Offset + new Vector2(0, i * FontHeight + FontHeight + 5), FontColor);
                    Engine.Instance.SpriteBatch.DrawString(Font, HighScoreMgr.HighScores[i].Name, Offset + new Vector2(100, i * FontHeight + FontHeight + 5), FontColor);
                    Engine.Instance.SpriteBatch.DrawString(Font, HighScoreMgr.HighScores[i].Score.ToString(), Offset + new Vector2(275, i * FontHeight + FontHeight + 5), FontColor);
                    Engine.Instance.SpriteBatch.DrawString(Font, HighScoreMgr.HighScores[i].Values[0].Value, Offset + new Vector2(425, i * FontHeight + FontHeight + 5), FontColor);
                    Engine.Instance.SpriteBatch.DrawString(Font, HighScoreMgr.HighScores[i].Values[1].Value, Offset + new Vector2(700, i * FontHeight + FontHeight + 5), FontColor);
                    Engine.Instance.SpriteBatch.DrawString(Font, HighScoreMgr.HighScores[i].Values[2].Value, Offset + new Vector2(825, i * FontHeight + FontHeight + 5), FontColor);
                    Engine.Instance.SpriteBatch.DrawString(Font, HighScoreMgr.HighScores[i].Values[3].Value, Offset + new Vector2(912, i * FontHeight + FontHeight + 5), FontColor);
                }
            }
            catch
            {
                // If something goes wrong then delete the highscores and show an emtpy one.
                HighScoreMgr.DeleteAllHighScores();
            }
        }
    }
}
