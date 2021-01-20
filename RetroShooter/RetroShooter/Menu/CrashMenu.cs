using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using System.IO;

namespace RetroShooter
{
    public class CrashMenu : IActiveState
    {
        StringBuilder Text;
        static readonly SpriteFont Font = Common.str2Font("Crash");
        string ExtraInfo;
        Exception Ex;
        public CrashMenu(string extraInfo, Exception ex)
        {
            ExtraInfo = extraInfo;
            Ex = ex;
            string dumpLoc = CrashDump();
            Text = Misc.WrapText(Font, "Our apologies but an error occured. Please send us the crashlog ("+dumpLoc+")."+Environment.NewLine+ ex.ToString(), Engine.Instance.Width);
        }

        private string CrashDump()
        {
            if (!Directory.Exists("CrashLogs"))
                Directory.CreateDirectory("CrashLogs");

            string path = string.Format("CrashLogs/CrashLog_{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);

            File.WriteAllText(path, ExtraInfo + Environment.NewLine + Ex.ToString());

            return path;
        }

        public void Update(GameTime gameTime)
        {
            if (InputMgr.Instance.Keyboard.IsPressed(Keys.Enter) ||
                InputMgr.Instance.Keyboard.IsPressed(Keys.Escape))
                Engine.Instance.Game.Exit();
        }

        public void Draw()
        {
            Engine.Instance.Graphics.GraphicsDevice.Clear(Color.Black);
            Engine.Instance.SpriteBatch.DrawString(Font, Text, Vector2.Zero, Color.White);
        }
    }
}