using System;
using Microsoft.Xna.Framework;
using DefaultEcs;
using Microsoft.Xna.Framework.Input;
using DefaultEcs.Threading;
using Common.Settings;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Common.Core.Scenes;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
using System.IO;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D;

namespace Common
{
    public class byBullet : Game
    {
        private GraphicsDeviceManager Graphics;
        private ScreenManager ScreenManager;
        private SpriteBatch spriteBatch;

        public byBullet()
        {
            SetBasicConfiguration();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            InitializeScreenManager();
            InitializeProperties();
            InitizalizeGameSettings();
            InitializeUI();
            StartGame();
        }

        private void InitializeUI()
        {
        }

        private void InitializeProperties()
        {
        }

        private void SetBasicConfiguration()
        {
            //Graphics settings
            Graphics = new GraphicsDeviceManager(this);
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            
            Graphics.PreferredBackBufferWidth = 1600;
            Graphics.PreferredBackBufferHeight = 900;
            Graphics.IsFullScreen = false;

            //Other
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void InitializeScreenManager()
        {
            ScreenManager = new ScreenManager();
        }

        private void InitizalizeGameSettings()
        {
            //initialize GameSetting class
            var settings = GameSettings.Instance;
            
            settings.GraphicsDevice = GraphicsDevice;
            settings.Graphics = Graphics;
            settings.ContentManager = Content;
            settings.Game = this;
            settings.World = new World();
            settings.MainRunner = new DefaultParallelRunner(Environment.ProcessorCount);
            settings.ScreenManager = ScreenManager;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            settings.SpriteBatch = spriteBatch;
        }

        private void StartGame()
        {
            ScreenManager.LoadScreen(new MenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        protected override void Draw(GameTime gameTime)
        {
            // GraphicsDevice.SetRenderTarget(engineScreen);
            //     GraphicsDevice.Clear(Color.Coral);

            //     guiRenderer.BeginLayout(gameTime);

            //         ImGui.BeginMainMenuBar();
            //             ImGui.Button("File");
            //             ImGui.Button("Edit");
            //             ImGui.Button("About");
            //         ImGui.EndMainMenuBar();

            //         gameScreenDrawing = ImGui.Begin("Game");
            //             var windowSize = ImGui.GetContentRegionAvail();
            //             var correctPos = ImGui.GetWindowPos() + ImGui.GetWindowContentRegionMin();
            //             var pos = new Vector2(correctPos.X, correctPos.Y).ToPoint();
            //             var size = new Vector2(windowSize.X, windowSize.Y).ToPoint();
            //             gameViewportRectangle = new Rectangle(pos, size);
            //         ImGui.End();

            //     guiRenderer.EndLayout();
            // GraphicsDevice.SetRenderTarget(null);

            // GraphicsDevice.SetRenderTarget(gameScreen);
            //     GraphicsDevice.Clear(Color.Coral);
            //     if(gameScreenDrawing)
            //     {
            //         ScreenManager.Draw(gameTime);
            //     }
            // GraphicsDevice.SetRenderTarget(null);
            
            // spriteBatch.Begin();
            // spriteBatch.Draw(gameScreen, Vector2.Zero, Color.White);
            // // spriteBatch.Draw(gameScreen, gameViewportRectangle, Color.White);
            
            // spriteBatch.End();

            ScreenManager.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Update(gameTime);
        }
    }
}