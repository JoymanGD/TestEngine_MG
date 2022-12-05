using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [With(typeof(LookAt))]
    
    public partial class LookAtSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public LookAtSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Transform _transform, ref LookAt _lookAt, in Entity _entity, GameTime _gameTime)
        {
            var elapsedSeconds = _gameTime.ElapsedGameTime.TotalSeconds;
            var direction = _lookAt.Forward + _transform.Position;

            if(_lookAt.Smooth)
            {
                _transform.LookAtSmooth(direction, _transform.RotationSpeed * (float)elapsedSeconds);
            }
            else
            {
                _transform.LookAt(direction);
            }
        }
    }
}