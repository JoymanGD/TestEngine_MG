using System;
using Microsoft.Xna.Framework;
using DefaultEcs;
using DefaultEcs.Threading;
using Common.Settings;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Common.Core.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class byBullet : Game
    {
        private GraphicsDeviceManager Graphics;
        private ScreenManager ScreenManager;

        public byBullet()
        {
            SetBasicConfiguration();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            InitializeScreenManager();
            InitizalizeGameSettings();
            StartGame();
        }

        void SetBasicConfiguration(){
            //Graphics settings
            Graphics = new GraphicsDeviceManager(this);
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            Graphics.IsFullScreen = false;
            // Graphics.PreferredBackBufferWidth = 1920;
            // Graphics.PreferredBackBufferHeight = 1080;
            // Graphics.IsFullScreen = true;

            //Other
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        void InitializeScreenManager(){
            ScreenManager = new ScreenManager();
            Components.Add(ScreenManager);
        }

        void InitizalizeGameSettings(){
            //initialize GameSetting class
            var settings = GameSettings.Instance;
            
            settings.GraphicsDevice = GraphicsDevice;
            settings.Graphics = Graphics;
            settings.ContentManager = Content;
            settings.Game = this;
            settings.World = new World();
            settings.MainRunner = new DefaultParallelRunner(Environment.ProcessorCount);
            settings.ScreenManager = ScreenManager;
        }

        void StartGame(){
            ScreenManager.LoadScreen(new MenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
    }
}
