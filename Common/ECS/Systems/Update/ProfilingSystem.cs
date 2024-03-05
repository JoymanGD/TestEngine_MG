using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Common.Settings;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Common.ECS.Systems
{
    [With(typeof(GameInfo))]
    [With(typeof(SpriteFont))]
    
    public partial class ProfilingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        private SpriteBatch SpriteBatch;
        
        public static bool ShowShadowMap;
        
        public ProfilingSystem(SpriteBatch _spriteBatch, World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
            SpriteBatch = _spriteBatch;
        }

        [Update]
        private void Update(in GameInfo _info, in SpriteFont _font, GameTime _gameTime)
        {
            var playerEntites = World.GetEntities().With<Player>().With<Transform>().AsSet().GetEntities();
            var playerTransform = playerEntites[0].Get<Transform>();
            var currentSpeed = playerTransform.DeltaPosition.Length();
            var elapsedMilliseconds = _gameTime.ElapsedGameTime.TotalMilliseconds;
            var elapsedSeconds = _gameTime.ElapsedGameTime.TotalSeconds;
            var fps = 1/elapsedSeconds;
            
            SpriteBatch.Begin(0, BlendState.Opaque, SamplerState.AnisotropicClamp);
            if(ShowShadowMap)
            {
                SpriteBatch.Draw(ShadowMapGenerationSystem.ShadowMap, new Rectangle(0, 0, 512, 512), Color.White);
            }
            SpriteBatch.DrawString(_font, $"FPS: {fps}\nElapsed time(ms): {elapsedMilliseconds}\nPlayer speed: {currentSpeed}", Vector2.One * 20, Color.White);
            SpriteBatch.End();
        }
    }
}