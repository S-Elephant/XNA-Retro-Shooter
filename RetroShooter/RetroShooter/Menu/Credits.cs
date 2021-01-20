using XNALib;
using Microsoft.Xna.Framework;

namespace RetroShooter
{
    public class Credits : CreditsMenu
    {
        public Credits() :
            base(new MainMenu(false), Engine.Instance, "Menu/spaceBG", "Font01_20", "Font02_28")
        {
            AddCreditTitle("Retro Shooter I");
            AddCredit("Created in 2 days");
            AddCredit("Released September 29 2011");
            AddCredit("Released under the CC-BY-SA 3.0 license");
            AddCredit("Be aware that the Audio & Graphics might have their own licenses!");
            AddCreditTitle("");
            AddCreditTitle("");

            AddCreditTitle("Programming and Design");
            AddCredit("Napoleon");
            AddCredit("Napoleons XNA Library");
            AddCreditTitle("");
            AddCreditTitle("");

            AddCreditTitle("Graphics");
            AddCredit("All from Open Game Art (www.opengameart.org) - keyboard layout");
            AddCredit("Mumu & Redshrike - Most of the Ships and weapons");
            AddCredit("I forgot the guy who did the scrolling background images but it was from OGA...");
            AddCredit("LokiF - HB Bar");
            AddCredit("ac3raven - Space background");
            AddCredit("HorrorPen - Icons/Pickups");
            AddCredit("Skorpio - spaceships");
            AddCredit("gecko666 - GUI buttons background");
            AddCredit("OceansDream - Machinegun icon");
            //AddCredit("Includes graphics from JS WARS by Jonas Wagner - http://29a.ch/ - Mine texture"); // mine is not used
            AddCredit("Napoleon - Nuclear icon & the orange bullet");
            AddCredit("Clint Bellanger - Shield sprite");
            AddCredit("Lokif - Invalid selection sound (badChoice)");
            AddCredit("qubodup - Mouse texture");
            AddCredit("Justin Nichol - ShopKeeper portrait");
            AddCredit("Flatlander - Dialog panel in the shop");
            AddCreditTitle("");
            AddCreditTitle("");

            AddCreditTitle("Audio FX (opengameart)");
            AddCredit("Michel Baradari - Explosion");
            AddCredit("qubodup - Impact");
            AddCredit("Michel Baradari - Rocket Launch");
            AddCredit("Secretlondon - Coin Drop");
            AddCredit("yewbic - Shop Ambience");
            AddCreditTitle("");
            AddCreditTitle("");

            AddCreditTitle("Audio Music (www.opengameart.org)");
            AddCredit("Pheonton - Main Theme");
            AddCredit("Gobusto - Level Music (Techno one)");
            AddCredit("Bart - Level Music (Airship)");
            AddCredit("Remaxim - Victory tune");
            AddCredit("p0ss - p0ss Music");
            AddCreditTitle("");
            AddCreditTitle("");

            AddCreditTitle("Testing");
            AddCredit("Napoleon");
            AddCredit("DeamonBoy");
            AddCreditTitle("");
            AddCreditTitle("");

            AddCreditTitle("Special Thanks & Misc");
            AddCredit("AudaCity");
            AddCredit("Microsoft");
            AddCredit("The Gimp");
            AddCredit("OpenGameArt.org");
            AddCredit("Iron Star Media LTD. (Fancy Bitmap Generator)");
            AddCreditTitle("");
            AddCreditTitle("");

            SetLocations();
        }
    }
}