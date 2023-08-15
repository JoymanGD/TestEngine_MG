using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using System;

namespace Common.ECS.Systems
{
    [With(typeof(Camera))]
    [With(typeof(CameraZoom))]
    [With(typeof(Transform))]
    [With(typeof(Controller))]
    public partial class CameraZoomSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public CameraZoomSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Transform transform, ref Controller _controller, GameTime _gameTime){
            
        }
    }
}
