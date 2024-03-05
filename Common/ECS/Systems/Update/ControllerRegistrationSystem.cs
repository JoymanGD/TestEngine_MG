using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Bindings))]
    [WhenAdded(typeof(Controller))]
    
    public partial class ControllerRegistrationSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public ControllerRegistrationSystem(World world, IParallelRunner runner) : base(world, CreateEntityContainer, null, 0)
        {
            this.world = world;
            this.runner = runner;
        }

        [Update]
        private void Update(ref Controller controller, ref Bindings bindings)
        {
            controller.Init(bindings);
        }
    }
}