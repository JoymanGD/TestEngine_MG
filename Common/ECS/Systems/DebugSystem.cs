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
        private bool lightsOn = true;
        
        public DebugSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Controller _controller){
            if(_controller.WasPressed("Debug_LightsSwitch")){
                if(lightsOn)
                {
                    lightsOn = false;
                }
                else
                {
                    lightsOn = true;
                }

                var lights = World.Get<Light>();

                foreach (var item in lights)
                {
                    item.IsActive = lightsOn;
                }
            }
        }
    }
}