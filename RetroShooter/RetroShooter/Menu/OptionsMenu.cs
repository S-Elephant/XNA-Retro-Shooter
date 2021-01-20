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
    public class OptionsMenu : ScrollMenu
    {
        public OptionsMenu() :
            base(Engine.Instance.SpriteBatch, "MenuChoice", Engine.Instance.Width, Engine.Instance.Height, 250, 160)
        {
            Texture = Common.str2Tex("Menu/spaceBG");

            AddChoice("music", "Enable Music", "Yes", "No");
            if (!SettingsMgr.Instance.EnableMusic)
                Choices[Choices.Count - 1].SetSelectedIdx(1);

            AddChoice("fullscreen", "FullScreen", "Yes", "No");
            if (!SettingsMgr.Instance.IsFullScreen)
                Choices[Choices.Count - 1].SetSelectedIdx(1);

            AddChoice("ctrlType", "Control Type:", "Keyboard", "Mouse");
            if (SettingsMgr.Instance.ControlType1 == eControlType.Mouse)
                Choices[Choices.Count - 1].SetSelectedIdx(1);

            AddChoice("showSpawnTimer", "Show Spawn Timer:", "Yes", "No");
            if (!SettingsMgr.Instance.ShowSpawnTimer)
                Choices[Choices.Count - 1].SetSelectedIdx(1);

            AddChoice("skipTutorial", "Disable Tutorial:", "No", "Yes");
            if (SettingsMgr.Instance.SkipTutorial)
                Choices[Choices.Count - 1].SetSelectedIdx(1);

            AddChoice("showRoundGUI", "Enable Round GUI:", "No", "Yes");
            if (SettingsMgr.Instance.ShowRoundGUI)
                Choices[Choices.Count - 1].SetSelectedIdx(1);

            AddChoice("roundGUIAlpha", "Round GUI Alpha:", "32", "64", "96", "128", "160", "192", "224", "255");
            Choices[Choices.Count - 1].SetSelectedIdx((int)Math.Ceiling(SettingsMgr.Instance.RoundGUIAlpha / (float)32) - 1);

            AddChoice("back", "Back");
            SelectChoice += new OnSelectChoice(OptionsMenu_SelectChoice);
            SelectionChanged += new OnSelectionChanged(OptionsMenu_SelectionChanged);
        }

        void OptionsMenu_SelectionChanged(ScrollChoice oldChoice, ScrollChoice newChoice)
        {
            switch (oldChoice.Name)
            {
                case "music":
                    SettingsMgr.Instance.EnableMusic = oldChoice.SelectedValue == "Yes";
                    MP3MusicMgr.Instance.EnableMusic = SettingsMgr.Instance.EnableMusic;
                    break;
                case "fullscreen":
                    if (Engine.Instance.Graphics.IsFullScreen != (oldChoice.SelectedValue == "Yes"))
                    {
                        SettingsMgr.Instance.IsFullScreen = oldChoice.SelectedValue == "Yes";
                        Resolution.SetResolution(1280, 800, SettingsMgr.Instance.IsFullScreen);
                        Engine.Instance.Graphics.ApplyChanges();
                    }
                    break;
                case "showSpawnTimer":
                    SettingsMgr.Instance.ShowSpawnTimer = oldChoice.SelectedValue == "Yes";
                    break;
                case "ctrlType":
                    SettingsMgr.Instance.ControlType1 = (eControlType)oldChoice.SelectedValueIdx;
                    break;
                case "skipTutorial":
                    SettingsMgr.Instance.SkipTutorial = oldChoice.SelectedValue == "Yes";
                    break;
                case "showRoundGUI":
                    SettingsMgr.Instance.ShowRoundGUI = oldChoice.SelectedValue == "Yes";
                    break;
                case "roundGUIAlpha":
                    SettingsMgr.Instance.RoundGUIAlpha = byte.Parse(oldChoice.SelectedValue);
                    break;
                case "back":
                    // Do nothing
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        void OptionsMenu_SelectChoice(ScrollChoice choice)
        {
            switch (choice.Name)
            {
                case "back":
                    SettingsMgr.Instance.Save();
                    Engine.Instance.ActiveState = new MainMenu(false);
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        ~OptionsMenu()
        {
            SelectChoice -= new OnSelectChoice(OptionsMenu_SelectChoice);
            SelectionChanged -= new OnSelectionChanged(OptionsMenu_SelectionChanged);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputMgr.Instance.Keyboard.IsPressed(Keys.Escape))
                Engine.Instance.ActiveState = new MainMenu(false);
        }

        public override void Draw()
        {
            base.Draw();
            Engine.Instance.SpriteBatch.DrawString(MainMenu.TitleFont, "Options", Common.CenterStringX(MainMenu.TitleFont, "Options", Engine.Instance.Width, 70), Color.White);
        }
    }
}