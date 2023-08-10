using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;
using Myra.Graphics2D;
using Common.Settings;
using FontStashSharp;
using System.IO;
using System;

namespace Common.Core.Scenes
{
    public class MenuScreen : GameScreen
    {
        private new byBullet Game => (byBullet) base.Game;
        Desktop Desktop;

        public MenuScreen(byBullet game) : base(game) { }

        public override void LoadContent()
        {
            Game.IsMouseVisible = true;

            MyraEnvironment.Game = Game;

            var grid = new Grid();

            //grid.ShowGridLines = true;
            var fontSys = FontSystemFactory.Create(GameSettings.Instance.Game.GraphicsDevice, 400, 400);
            fontSys.AddFont(File.ReadAllBytes(@"Content/Fonts/Default.otf"));

            var title = new Label
            {
            GridRow = 0,
            Id = "label",
            Text = "byBullet",
            Font = fontSys.GetFont(80),
            TextColor = Color.Cyan,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom
            };
            grid.Widgets.Add(title);

            // Button
            var button = new TextButton
            {
            GridRow = 1,
            Text = "PLAY",
            Height = 40,
            Width = 130,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0,20,0,0),
            Background = null
            };
            
            button.Click += (s, a) =>
            {
                ScreenManager.LoadScreen(new TestScreen(Game), new FadeTransition(GraphicsDevice, Color.Black));
            };

            grid.Widgets.Add(button);

            Desktop = new Desktop();
            Desktop.Root = grid;

            // Add it to the desktop
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Desktop.UpdateLayout();
        }

        public override void Draw(GameTime gameTime)
        {
            GameSettings.Instance.GraphicsDevice.Clear(Color.Black);
            Desktop.Render();
        }
    }
}