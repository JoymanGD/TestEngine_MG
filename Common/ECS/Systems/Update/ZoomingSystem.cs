using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using DefaultEcs.Command;
using System;

namespace Common.ECS.Systems
{
    [With(typeof(Zoom))]
    [With(typeof(Controller))]
    
    public partial class ZoomingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        private EntityCommandRecorder EntityCommandRecorder = new EntityCommandRecorder();
        
        public ZoomingSystem(World world, IParallelRunner runner) : base(world, CreateEntityContainer, null, 0)
        {
            this.world = world;
            this.runner = runner;
        }

        [Update]
        private void Update(ref Controller controller, ref Zoom zoom, GameTime gameTime)
        {
            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            var scrollWheelValue = InputSystem.ScrollWheelValue;

            if(scrollWheelValue != 0)
            {
                zoom.AddValue(-zoom.Speed * elapsedSeconds * scrollWheelValue);
            }
        }
    }
}