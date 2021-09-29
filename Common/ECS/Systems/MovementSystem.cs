using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [With(typeof(Movement))]
    
    public partial class MovementSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public MovementSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Transform _transform, ref Movement _movement){
            _transform.Translate(_movement.Direction * _movement.Speed);
        }
    }
}