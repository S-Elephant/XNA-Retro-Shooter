using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNALib;

namespace RetroShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //FPSCounter2 FPS;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Global.Content = Content;
            Engine.Instance = new Engine();
            Engine.Instance.Game = this;
            Engine.Instance.Graphics = graphics;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f); // 30 FPS

            //FPS = new FPSCounter2(this, "FPS");
            //Components.Add(FPS);
        }

       
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Engine.Instance.SpriteBatch = spriteBatch;

            // Settings
            SettingsMgr.Instance = new SettingsMgr();
            SettingsMgr.Instance.Load();

            // Achievements
            AchievementsMgr.Instance = new AchievementsMgr();
            AchievementsMgr.Instance.Load();

            // Resolution
            Resolution.Init(ref graphics);
#if WINDOWS
            Resolution.SetResolution(1280, 800, SettingsMgr.Instance.IsFullScreen);
            Resolution.SetVirtualResolution(1280, 800); // Don't change this anymore after this. Use SetResolution() instead.
#endif
#if XBOX
            Resolution.SetResolution(1280, 720, true);
            Resolution.SetVirtualResolution(1280, 720); // Don't change this anymore after this. Use SetResolution() instead.
#endif

            Engine.Instance.Audio.AddSound(60, "impact1");
            Engine.Instance.Audio.AddSound(10, "explodemini");
            Engine.Instance.Audio.AddSound(8, "rlaunch");
            Engine.Instance.Audio.AddSound(50, "rlaunchEnemy");
            Engine.Instance.Audio.AddSound(2, "victory");
            Engine.Instance.Audio.AddSound(5, "badChoice");
            Engine.Instance.Audio.AddSound(1, "coinDrop");

            Exiting += new EventHandler<EventArgs>(Game1_Exiting);

            InputMgr.Instance = new InputMgr();
            InputMgr.Instance.Mouse = new Mouse2("Mouse/mouse");

            HighScoreMenu.Init();
            MP3MusicMgr.Instance = new MP3MusicMgr() { EnableMusic = SettingsMgr.Instance.EnableMusic };

            Engine.Instance.ActiveState = new MainMenu(true);
        }

        void Game1_Exiting(object sender, EventArgs e)
        {
            Exiting -= new EventHandler<EventArgs>(Game1_Exiting);

            MP3MusicMgr.Instance.StopMusic();
            Engine.Instance.Audio.DisposeAll();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                InputMgr.Instance.Update(gameTime);
                Engine.Instance.ActiveState.Update(gameTime);
            }
            catch (Exception ex)
            {
                Engine.Instance.ActiveState = new CrashMenu("Update()", ex);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Resolution.GetScaleMatrix());
                Engine.Instance.ActiveState.Draw();
                spriteBatch.End();
            }
            catch (Exception ex)
            {
                Engine.Instance.ActiveState = new CrashMenu("Draw()", ex);
            }
            finally
            {
                try { spriteBatch.End(); }
                catch { }
            }

            base.Draw(gameTime);
        }
    }
}
