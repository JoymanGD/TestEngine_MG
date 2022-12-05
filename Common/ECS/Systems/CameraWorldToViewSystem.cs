using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using System;

namespace Common.ECS.Systems
{
    [With(typeof(Camera))]
    [With(typeof(Transform))]
    public partial class CameraWorldToViewSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public CameraWorldToViewSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Camera _camera, ref Transform _transform){
            _camera.ApplyWorldMatrix(_transform.WorldMatrix);
        }
    }
}