using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Common.Helpers.Patterns;
using MonoGame.Extended.Screens;
using DefaultEcs;
using DefaultEcs.Threading;

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
        public int DirectionalLightsCount = 0, PointLightsCount = 0;
    }
}