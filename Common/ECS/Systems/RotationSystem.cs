using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [With(typeof(Rotation))]
    
    public partial class RotationSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public RotationSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Transform _transform, ref Rotation _rotation){
            _transform.Rotate(_rotation.Value);
        }
    }
}