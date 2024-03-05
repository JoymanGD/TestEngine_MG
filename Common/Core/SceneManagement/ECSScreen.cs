using Common.Helpers;
using Microsoft.Xna.Framework;
using DefaultEcs.System;
using DefaultEcs;
using Common.Settings;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace Common.ECS.SceneManagement
{
    public abstract class ECSScreen : GameScreen
    {
        protected ISystem<GameTime> UpdateSystems { get; private set; }
        protected ISystem<GameTime> DrawSystems { get; private set; }
        protected World World = GameSettings.Instance.World;
        protected IParallelRunner MainRunner = GameSettings.Instance.MainRunner;
        protected SpriteBatch SpriteBatch = GameSettings.Instance.SpriteBatch;
        protected bool EntitiesInitialized = false;

        public ECSScreen(byBullet game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            SetSystems();
        }

        public void SetSystems()
        {
            UpdateSystems = InitializeUpdateSystems();
            DrawSystems = InitializeDrawSystems();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateSystems.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if(!EntitiesInitialized)
            {
                SetStartEntities();
                EntitiesInitialized = true;
            }
            
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DrawSystems.Update(gameTime);
        }

        public abstract ISystem<GameTime> InitializeUpdateSystems();

        public abstract ISystem<GameTime> InitializeDrawSystems();

        public abstract void SetStartEntities();
    }
}