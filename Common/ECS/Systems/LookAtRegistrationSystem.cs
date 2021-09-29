using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [WhenAdded(typeof(LookAt))]
    
    public partial class LookAtRegistrationSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public LookAtRegistrationSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Transform _transform, ref LookAt _lookAt, in Entity _entity){
            _transform.LookAt(_lookAt.Forward, _lookAt.Up);
            _entity.Remove<LookAt>();
        }
    }
}