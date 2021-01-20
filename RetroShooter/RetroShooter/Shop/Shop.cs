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
    public class Shop : IActiveState
    {
        List<ShopItem> ShopItems = new List<ShopItem>();
        const int TEMP = 128;
        int SelectedIdx = 0;
        static readonly SpriteFont Font = Common.str2Font("Font03_18");
        static readonly Color FontColor = Color.Goldenrod;
        static readonly Texture2D ActorTex = Common.str2Tex("Shop/shopKeeper0");
        
        StringBuilder PriceSB = new StringBuilder(7);
        const string PRICE_PREFIX = "Costs: $";
        Player Visitor;
        public static bool IsFirstVisit;
        Dialog Dialog = new Dialog((!SettingsMgr.Instance.SkipTutorial) && IsFirstVisit);

        public Shop(Player visitor)
        {
            MP3MusicMgr.Instance.PlayMusic("shopAmbience");
            Visitor = visitor;
            Vector2 nextShopItmLoc = new Vector2(100, 100);
            ShopItems.Add(new ShopItem(eShopItem.HPHeal, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.ShieldHeal, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.ShieldRegen, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.ShieldUpg, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.SpeedUpg, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.Nuclear, nextShopItmLoc));
            nextShopItmLoc = new Vector2(100, 196 + TEMP-96);

            ShopItems.Add(new ShopItem(eShopItem.UpgOffensiveTier, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.UpgDefensiveTier, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.Interest, nextShopItmLoc));
            nextShopItmLoc += new Vector2(TEMP, 0);
            ShopItems.Add(new ShopItem(eShopItem.HPRegen, nextShopItmLoc));

            foreach (ShopItem si in ShopItems)
                si.Update(Visitor);
        }

        public void Update(GameTime gameTime)
        {
            if (!Dialog.IsTutorial)
            {
                if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultConfirmKey, InputMgr.Instance.DefaultConfirmButton))
                    AttemptBuy();
                else if (InputMgr.Instance.IsPressed(null, InputMgr.Instance.DefaultCancelKey, InputMgr.Instance.DefaultCancelButton))
                    ReturnToLevel();

                if (InputMgr.Instance.IsPressed(null, Keys.Left, Buttons.DPadLeft) && SelectedIdx > 0)
                    SelectedIdx--;
                else if (InputMgr.Instance.IsPressed(null, Keys.Right, Buttons.DPadRight) && SelectedIdx < ShopItems.Count - 1)
                    SelectedIdx++;

                foreach (ShopItem si in ShopItems)
                    si.Update(Visitor);
            }
            else
                Dialog.UpdateTutorial(gameTime);
        }

        void AttemptBuy()
        {
            ShopItem SelectedItem = ShopItems[SelectedIdx];
            if (SelectedItem.CanAffordAndBuy)
            {
                Visitor.Score -= SelectedItem.Price;
                SelectedItem.Buy(Visitor);
            }
            else
            {
                Engine.Instance.Audio.PlaySound(AudioConstants.BadChoice);
            }
        }

        void ReturnToLevel()
        {
            Level.Instance.PlayMusic();
            GC.Collect();
            Engine.Instance.ActiveState = Level.Instance;
        }

        public void Draw()
        {
            // Actor
            Engine.Instance.SpriteBatch.Draw(ActorTex,new Vector2(Engine.Instance.Width-ActorTex.Width,Engine.Instance.Height-ActorTex.Height), Color.White);

            // Dialog
            Dialog.Draw();

            // Draw items
            int i = 0;
            foreach (ShopItem si in ShopItems)
            {
                if (i == SelectedIdx)
                    si.DrawAsSelected();
                else
                    si.Draw();
                i++;
            }

            // Draw texts
            PriceSB.Remove(0, PriceSB.Length);
            PriceSB.Append(PRICE_PREFIX);
            PriceSB.Append(ShopItems[SelectedIdx].Price);
            Engine.Instance.SpriteBatch.DrawString(Font, PriceSB, new Vector2(100, 400), FontColor);
            Engine.Instance.SpriteBatch.DrawString(Font, ShopItems[SelectedIdx].GetDescription, new Vector2(100, 500), FontColor);

            // Score
            Visitor.DrawGUI();
        }
    }
}