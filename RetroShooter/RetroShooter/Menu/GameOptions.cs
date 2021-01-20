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
    public class GameOptions : ScrollMenu
    {
        private float ScoreModifier;
        private static readonly SpriteFont ScoreModFont = Common.str2Font("Font03_18");

        public GameOptions():
            base(Engine.Instance.SpriteBatch, "MenuChoice", Engine.Instance.Width, Engine.Instance.Height, 250, 160,400)
        {
            Texture = Common.str2Tex("Menu/spaceBG");
            AddChoice("play", "Play");
            AddChoice("playerShip", "Ship:", "Normal", "Destroyer", "Regeneration", "Cruiser", "Tanker");
            AddChoice("startWave", "Starting Wave:", 1, 3, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50);
            AddChoice("area", "Area:", "Ireland", "Spain", "Siberia", "Artic Region");
            AddChoice("dropRate", "DropRate:", eDropRateMod.None, eDropRateMod.Low, eDropRateMod.Normal, eDropRateMod.High);
            Choices[Choices.Count - 1].SetSelectedIdx(2);
            AddChoice("waveDelay", "Wave Delay (s):", 10, 13, 16, 18, 20);
            Choices[Choices.Count - 1].SetSelectedIdx(3);
            AddChoice("music", "Music:", "Random", "Techno1", "Airship", "Poss");
            AddChoice("back", "Back");
            SelectChoice += new OnSelectChoice(GameOptions_SelectChoice);

            ScoreModifier = GetScoreMod();
            SelectionChanged += new OnSelectionChanged(GameOptions_SelectionChanged);
            ValueChanged += new OnValueChanged(GameOptions_ValueChanged);
        }

        void GameOptions_ValueChanged(ScrollChoice choice, string oldValue, string newValue)
        {
            ScoreModifier = GetScoreMod();
        }

        void GameOptions_SelectionChanged(ScrollChoice oldChoice, ScrollChoice newChoice)
        {
            ScoreModifier = GetScoreMod();
        }

        ~GameOptions()
        {
            SelectChoice -= new OnSelectChoice(GameOptions_SelectChoice);
            SelectionChanged -= new OnSelectionChanged(GameOptions_SelectionChanged);
            ValueChanged -= new OnValueChanged(GameOptions_ValueChanged);
        }

        private float GetScoreMod()
        {
            float scoreMod = 1f;
            int wDelayIdx = Choices.Find(c => c.Name == "waveDelay").SelectedValueIdx;
            switch (wDelayIdx)
            {
                case 0:
                    scoreMod += 0.4f;
                    break;
                case 1:
                    scoreMod += 0.2f;
                    break;
                case 2:
                    // Do nothing
                    break;
                case 3:
                    scoreMod -= 0.2f;
                    break;
                case 4:
                    scoreMod -= 0.4f;
                    break;
                default:
                    throw new CaseStatementMissingException();
            }

            ePlaneType planeType = (ePlaneType)GetChoiceByName("playerShip").SelectedValueIdx;
            switch (planeType)
            {
                case ePlaneType.Normal:
                    // Do nothing
                    scoreMod += 0.05f;
                    break;
                case ePlaneType.Dmg:
                    scoreMod -= 0.1f;
                    break;
                case ePlaneType.Regen:
                    scoreMod -= 0.1f;
                    break;
                case ePlaneType.Speed:
                    scoreMod -= 0.05f;
                    break;
                case ePlaneType.Tough:
                    scoreMod -= 0.1f;
                    break;
                default:
                    throw new CaseStatementMissingException();
            }

            eDropRateMod dropRateMod = (eDropRateMod)Enum.Parse(typeof(eDropRateMod), GetChoiceValue("dropRate"));
            switch (dropRateMod)
            {
                case eDropRateMod.None:
                    scoreMod += 0.4f;
                    break;
                case eDropRateMod.Low:
                    scoreMod += 0.2f;
                    break;
                case eDropRateMod.Normal:
                    // Do nothing
                    break;
                case eDropRateMod.High:
                    scoreMod -= 0.2f;
                    break;
                default:
                    throw new CaseStatementMissingException();
            }

            if (scoreMod < 0.1f)
                scoreMod = 0.1f;

            return scoreMod;
        }

        void GameOptions_SelectChoice(ScrollChoice choice)
        {
            switch (choice.Name)
            {
                case "play":
                    // Music
                    string levelMusic;
                    if (GetChoiceValue("music") == "Random")
                        levelMusic = "level0" + Maths.RandomNr(1, 3).ToString();
                    else
                    {
                        switch (GetChoiceValue("music"))
                        {
                            case "Techno1":
                                levelMusic = "level01";
                                break;
                            case "Airship":
                                levelMusic = "level02";
                                break;
                            case "Poss":
                                levelMusic = "level03";
                                break;
                            default:
                                throw new CaseStatementMissingException();
                        }
                    }

                    eDropRateMod dropRateMod = (eDropRateMod)Enum.Parse(typeof(eDropRateMod), GetChoiceValue("dropRate"));                  

                    // Create level
                    new Level(int.Parse(GetChoiceValue("startWave")), (int)dropRateMod, int.Parse(GetChoiceValue("waveDelay")) * 1000, GetChoiceByName("area").SelectedValueIdx + 1, ScoreModifier, levelMusic, (ePlaneType)GetChoiceByName("playerShip").SelectedValueIdx, GetChoiceByName("playerShip").SelectedValue);
                   
                    // set state
                    Engine.Instance.ActiveState = Level.Instance;
                    break;
                case "playerShip":
                    // Do nothing
                    break;
                case "startWave":
                    // Do nothing
                    break;
                case "area":
                    // Do nothing
                    break;
                case "waveDelay":
                    // Do nothing
                    break;
                case "music":
                    // Do nothing
                    break;
                case "back":
                    Engine.Instance.ActiveState = new MainMenu(false);
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultCancelKey, InputMgr.Instance.DefaultCancelButton))
                Engine.Instance.ActiveState = new MainMenu(false);
        }

        public override void Draw()
        {
            base.Draw();
            
            // Title
            Engine.Instance.SpriteBatch.DrawString(MainMenu.TitleFont, "Game Options", Common.CenterStringX(MainMenu.TitleFont, "Game Options", Engine.Instance.Width, 70), Color.White);

            // Score modifier
            Engine.Instance.SpriteBatch.DrawString(ScoreModFont, "Score Modifier: " + (ScoreModifier*100)+"%", new Vector2(25, Engine.Instance.Height-50), Color.Goldenrod);
        }
    }
   
}