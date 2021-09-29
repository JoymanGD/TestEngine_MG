using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Controller))]
    [With(typeof(Player))]
    
    public partial class DebugSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public DebugSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Controller _controller){
            if(_controller.IsHolding("MoveLeft")){
                var lights = World.Get<Light>();

                foreach (var light in lights)
                {
                    light.IsActive = false;
                }
            }

            
            if(_controller.IsHolding("MoveRight")){
                var lights = World.Get<Light>();

                foreach (var light in lights)
                {
                    light.IsActive = true;
                }
            }
        }
    }
}