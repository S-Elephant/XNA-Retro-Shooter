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
    public class MainMenu : ScrollMenu
    {
        public static readonly SpriteFont TitleFont = Common.str2Font("MenuTitle");

        public MainMenu(bool replayMusic) :
            base(Engine.Instance.SpriteBatch, "MenuChoice", Engine.Instance.Width, Engine.Instance.Height, 250, 160)
        {
            if(replayMusic)
                MP3MusicMgr.Instance.PlayMusic("mainTheme");
            Texture = Common.str2Tex("Menu/spaceBG");
            AddChoice("play", "Play");
            AddChoice("highScores", "HighScores");
            AddChoice("options", "Options");
            AddChoice("credits", "Credits");
            AddChoice("exit", "Exit");
            SelectChoice += new OnSelectChoice(MainMenu_SelectChoice);
        }

        ~MainMenu()
        {
            SelectChoice -= new OnSelectChoice(MainMenu_SelectChoice);
        }

        void MainMenu_SelectChoice(ScrollChoice choice)
        {
            switch (choice.Name)
            {
                case "play":
                    Engine.Instance.ActiveState = new GameOptions();
                    break;
                case "highScores":
                    Engine.Instance.ActiveState = new HighScoreMenu(false);
                    break;
                case "options":
                    Engine.Instance.ActiveState = new OptionsMenu();
                    break;
                case "credits":
                    Engine.Instance.ActiveState = new Credits();
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultCancelKey, InputMgr.Instance.DefaultCancelButton))
                Exit();
        }

        void Exit()
        {
            Engine.Instance.Game.Exit();
        }

        public override void Draw()
        {
            base.Draw();
            Engine.Instance.SpriteBatch.DrawString(TitleFont, "Retro Shooter I", Common.CenterStringX(TitleFont, "Retro Shooter I", Engine.Instance.Width, 70), Color.White);

            // Achievements
            int offsetX = 25;
            int locY = Engine.Instance.Height - 64-10;
            if (AchievementsMgr.Instance.BeatedLvl30)
            {
                Engine.Instance.SpriteBatch.Draw(AchievementsMgr.Lvl30Tex, new Vector2(offsetX, locY), Color.White);
                offsetX += 80;
            }
            if (AchievementsMgr.Instance.BeatedLvl60)
            {
                Engine.Instance.SpriteBatch.Draw(AchievementsMgr.Lvl60Tex, new Vector2(offsetX, locY), Color.White);
                offsetX += 80;
            }
            if (AchievementsMgr.Instance.Killed100)
            {
                Engine.Instance.SpriteBatch.Draw(AchievementsMgr.Kill100Tex, new Vector2(offsetX, locY), Color.White);
                offsetX += 80;
            }
            if (AchievementsMgr.Instance.Killed500)
            {
                Engine.Instance.SpriteBatch.Draw(AchievementsMgr.Kill500Tex, new Vector2(offsetX, locY), Color.White);
                offsetX += 80;
            }
            if (AchievementsMgr.Instance.Killed1000)
            {
                Engine.Instance.SpriteBatch.Draw(AchievementsMgr.Kill1000Tex, new Vector2(offsetX, locY), Color.White);
                offsetX += 80;
            }
        }
    }
}