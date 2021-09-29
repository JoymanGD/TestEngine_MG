using MonoGame.Extended.Screens;
using Microsoft.Xna.Framework;
using DefaultEcs.System;
using DefaultEcs;
using Common.Settings;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.SceneManagement
{
    public abstract class ECSScreen : GameScreen
    {
        protected ISystem<GameTime> UpdateSystems { get; private set; }
        protected ISystem<GameTime> DrawSystems { get; private set; }
        protected World World = GameSettings.Instance.World;
        protected IParallelRunner MainRunner = GameSettings.Instance.MainRunner;
        protected bool EntitiesInitialized = false;

        public ECSScreen(byBullet game) : base(game) { }

        public override void LoadContent()
        {
            base.LoadContent();
            SetSystems();
        }

        public void SetSystems(){
            UpdateSystems = InitializeUpdateSystems();
            DrawSystems = InitializeDrawSystems();
        }

        public override void Update(GameTime gameTime) {
            if(!EntitiesInitialized){
                SetStartEntities();
                EntitiesInitialized = true;
            }
            
            UpdateSystems.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            DrawSystems.Update(gameTime);
        }

        public abstract ISystem<GameTime> InitializeUpdateSystems();

        public abstract ISystem<GameTime> InitializeDrawSystems();

        public abstract void SetStartEntities();
    }
}