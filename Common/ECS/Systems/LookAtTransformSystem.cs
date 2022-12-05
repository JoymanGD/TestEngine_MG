using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [With(typeof(LookAtTransform))]
    
    public partial class LookAtTransformSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public LookAtTransformSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Transform _transform, ref LookAtTransform _lookAt, in Entity _entity, GameTime _gameTime)
        {
            var elapsedSeconds = _gameTime.ElapsedGameTime.TotalSeconds;

            if(_lookAt.Smooth)
            {
                _transform.LookAtSmooth(_lookAt.Target.Position, _transform.RotationSpeed * (float)elapsedSeconds);
            }
            else
            {
                _transform.LookAt(_lookAt.Target.Position);
            }
        }
    }
}