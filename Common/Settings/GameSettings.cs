using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Common.Helpers.Patterns;
using MonoGame.Extended.Screens;
using DefaultEcs;
using DefaultEcs.Threading;
using FontStashSharp;

namespace Common.Settings
{
    public class GameSettings : Singleton<GameSettings>
    {
        public GraphicsDevice GraphicsDevice;
        public GraphicsDeviceManager Graphics;
        public ContentManager ContentManager;
        public Game Game;
        public World World;
        public IParallelRunner MainRunner;
        public ScreenManager ScreenManager;
        public SpriteBatch SpriteBatch;
        public int DirectionalLightsCount = 0, PointLightsCount = 0;
        public Vector2 CenterPosition
        {
            get
            {
                if(centerPosition == Vector2.Zero)
                {
                    centerPosition = new Vector2(Graphics.PreferredBackBufferWidth/2, Graphics.PreferredBackBufferHeight/2);
                }

                return centerPosition;
            }
        }

        private Vector2 centerPosition;
    }
}