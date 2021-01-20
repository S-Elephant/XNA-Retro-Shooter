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
    public class Dialog
    {
        static readonly SpriteFont Font = Common.str2Font("Font03_18");
        static readonly Texture2D PanelTex = Common.str2Tex("Shop/panel");
        static readonly Rectangle TextArea = new Rectangle(256 + 32, Engine.Instance.Height - 242 + 20, 768 - 48, 242 - 40);
        static readonly Vector2 TextAreaLoc = new Vector2(256 + 32, Engine.Instance.Height - 242 + 20);

        int SelTutorialIdx = 0;
        static readonly string[] TutorialTexts = new string[]
        {
            "Hello and welcome to my shop. Press [Enter] to continue...",
            "Press [Escape] at any time to leave the shop...",
            "You can disable the tutorial permanently in the options menu...",
            "Use the Left and Right Arrow Keys to navigate through the available products and press [Enter] to buy the selected item (after you are done talking to me)...",
            "Products on a red background are either too expensive for you or you already have the maximum amount of those...",
            "Now press [Enter] once more to stop talking to me."
        };


        private bool m_IsTutorial;
        public bool IsTutorial
        {
            get { return m_IsTutorial; }
            private set { m_IsTutorial = value; }
        }
       

        string Text;

        static readonly string[] WelcomeTexts = new string[]
        {
            "Welcome pilot. Please help yourself.",
            "My prices are the lowest around. Oh wait I'm the only shop around.",
            "Have seen my sexy pilot? No? You really should!"
        };

        public Dialog(bool doTutorial)
        {
            IsTutorial = doTutorial;
            Text = WelcomeTexts[Maths.RandomNr(0, WelcomeTexts.Count() - 1)];
        }

        public void UpdateTutorial(GameTime gameTime)
        {
            if (IsTutorial && InputMgr.Instance.Keyboard.IsPressed(Keys.Enter))
            {
                SelTutorialIdx++;
                if (SelTutorialIdx >= TutorialTexts.Count())
                    IsTutorial = false;
            }
        }

        public void Draw()
        {
            // Panel
            Engine.Instance.SpriteBatch.Draw(PanelTex, new Vector2(256, Engine.Instance.Height - 242), Color.White);

            // Text
            if (IsTutorial)
                Engine.Instance.SpriteBatch.DrawString(Font, Misc.WrapText(Font, TutorialTexts[SelTutorialIdx], TextArea.Width), TextAreaLoc, Color.White);
            else
                Engine.Instance.SpriteBatch.DrawString(Font, Misc.WrapText(Font, Text, TextArea.Width), TextAreaLoc, Color.White);
        }
    }
}