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
    public class ShopItem
    {
        #region Members
        Rectangle AABB;
        Rectangle SelRect;
        Vector2 Location;

        static readonly Texture2D BoxGreen = Common.str2Tex("Shop/boxGreen");
        static readonly Texture2D BoxRed = Common.str2Tex("Shop/boxRed");
        static readonly Dictionary<eShopItem, Texture2D> ShopIcons = new Dictionary<eShopItem, Texture2D>()
        {
            {eShopItem.HPHeal,Common.str2Tex("Shop/hpHeal")},
            {eShopItem.ShieldHeal,Common.str2Tex("Shop/shieldHeal")},
            {eShopItem.ShieldRegen,Common.str2Tex("Shop/shieldRegen")},
            {eShopItem.ShieldUpg,Common.str2Tex("Shop/shieldUpg")},
            {eShopItem.SpeedUpg,Common.str2Tex("Shop/speedUpg")},
            {eShopItem.Nuclear,Common.str2Tex("Shop/nuclear")},
            {eShopItem.UpgOffensiveTier,Common.str2Tex("Shop/upgOffTier")},
            {eShopItem.UpgDefensiveTier,Common.str2Tex("Shop/upgDefTier")},
            {eShopItem.Interest,Common.str2Tex("Shop/dollarSign")},
            {eShopItem.HPRegen,Common.str2Tex("Shop/hpRegen")}
        };

        static readonly Dictionary<eShopItem, StringBuilder> ItemDescs = new Dictionary<eShopItem, StringBuilder>()
        {
            {eShopItem.HPHeal,new StringBuilder("Fully repair your ship.",40)},
            {eShopItem.ShieldHeal,new StringBuilder("Fully recharge your shield.",40)},
            {eShopItem.ShieldRegen,new StringBuilder("Increase your shield regeneration rate.",60)},
            {eShopItem.ShieldUpg,new StringBuilder("Upgrade your max. shield capacity.",40)},
            {eShopItem.SpeedUpg,new StringBuilder("Upgrade your engine.",30)},
            {eShopItem.Nuclear,new StringBuilder("Buy a Nuclear Bomb.", 30)},
            {eShopItem.UpgOffensiveTier,new StringBuilder("Upgrade Offensive Tier.", 40)},
            {eShopItem.UpgDefensiveTier,new StringBuilder("Upgrade Defensive Tier.", 40)},
            {eShopItem.Interest,new StringBuilder("Deposit 10.000$ and receive 15-30% interest in 8 waves.", 60)},
            {eShopItem.HPRegen,new StringBuilder("Upgrade HP Regeneration.", 45)}
        };

        public StringBuilder GetDescription { get { return ItemDescs[ItemType]; } }

        public int Price;

        private eShopItem m_ItemType;
        public eShopItem ItemType
        {
            get { return m_ItemType; }
            private set { m_ItemType = value; }
        }

        private bool m_CanAffordOrBuy = true;
        public bool CanAffordAndBuy
        {
            get { return m_CanAffordOrBuy; }
            private set { m_CanAffordOrBuy = value; }
        }
        #endregion

        public ShopItem(eShopItem shopItem, Vector2 location)
        {
            ItemType = shopItem;
            Location = location;
            AABB = new Rectangle(location.Xi(), location.Yi(), 96, 96);
            SelRect = new Rectangle(AABB.X-8,AABB.Y-8, 96+16, 96+16);

            switch (shopItem)
            {
                case eShopItem.HPHeal:
                    Price = 2500;
                    break;
                case eShopItem.ShieldHeal:
                    Price = 250;
                    break;
                case eShopItem.ShieldUpg:
                    Price = 2000;
                    break;
                case eShopItem.ShieldRegen:
                    Price = 2000;
                    break;
                case eShopItem.SpeedUpg:
                    Price = 2000;
                    break;
                case eShopItem.Nuclear:
                    Price = 5000;
                    break;
                case eShopItem.UpgOffensiveTier:
                    Price = 8000;
                    break;
                case eShopItem.UpgDefensiveTier:
                    Price = 4000;
                    break;
                case eShopItem.Interest:
                    Price = 10000;
                    break;
                case eShopItem.HPRegen:
                    Price = 1750;
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public void Buy(Player visitor)
        {
            switch (ItemType)
            {
                case eShopItem.HPHeal:
                    visitor.Ship.HealHPFull();
                    break;
                case eShopItem.ShieldHeal:
                    visitor.Ship.HealShieldFull();
                    break;
                case eShopItem.ShieldUpg:
                    visitor.Ship.UpgradeShield();
                    break;
                case eShopItem.ShieldRegen:
                    visitor.Ship.UpgradeShieldRegen();
                    break;
                case eShopItem.SpeedUpg:
                    visitor.Ship.UpgradeSpeed();
                    break;
                case eShopItem.Nuclear:
                    visitor.Ship.NuclearCnt++;
                    break;
                case eShopItem.UpgOffensiveTier:
                    visitor.Ship.UpgradeOffensiveTier();
                    break;
                case eShopItem.UpgDefensiveTier:
                    visitor.Ship.UpgradeDefensiveTier();
                    break;
                case eShopItem.Interest:
                    if (visitor.Interests.Count < Player.MAX_INTERESTS)
                        visitor.Interests.Add(8);
                    break;
                case eShopItem.HPRegen:
                    visitor.Ship.HPRegen.Upgrade();
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
            
        }

        void UpdateBuyableStatus(Player visitor)
        {
            CanAffordAndBuy = Price <= visitor.Score;
            switch (ItemType)
            {
                case eShopItem.HPHeal:
                    if(visitor.Ship.HP.CurrentHP == visitor.Ship.HP.MaxHP)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.ShieldHeal:
                    if (visitor.Ship.Shield.CurrentHP == visitor.Ship.Shield.MaxHP)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.ShieldUpg:
                    if (!visitor.Ship.CanUpgShieldHP)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.ShieldRegen:
                    if (!visitor.Ship.CanUpgShieldRegen)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.SpeedUpg:
                    if (!visitor.Ship.CanUpgSpeed)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.Nuclear:
                    if(visitor.Ship.NuclearCnt == visitor.Ship.MaxNuclearCnt)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.UpgOffensiveTier:
                    if (visitor.OffensiveTier >= Player.MAX_OFFENSIVE_TIER)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.UpgDefensiveTier:
                    if (visitor.DefensiveTier >= Player.MAX_DEFENSIVE_TIER)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.Interest:
                    if (visitor.Interests.Count >= Player.MAX_INTERESTS)
                        CanAffordAndBuy = false;
                    break;
                case eShopItem.HPRegen:
                    if (!visitor.Ship.HPRegen.CanUpgrade)
                        CanAffordAndBuy = false;
                    break;
                default:
                    throw new CaseStatementMissingException();
            }
        }

        public void Update(Player visitor)
        {
            UpdateBuyableStatus(visitor);
        }

        public void DrawAsSelected()
        {
            Engine.Instance.SpriteBatch.Draw(Common.White1px, SelRect, Color.Blue);
            Draw();
        }

        public void Draw()
        {
            if (CanAffordAndBuy)
                Engine.Instance.SpriteBatch.Draw(BoxGreen, AABB, Color.White);
            else
                Engine.Instance.SpriteBatch.Draw(BoxRed, AABB, Color.White);

            Engine.Instance.SpriteBatch.Draw(ShopIcons[ItemType], Location, Color.White);
        }
    }
}