using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(Camera))]
    [With(typeof(Transform))]
    [With(typeof(Controller))]
    // [With(typeof(Movement))]
    public partial class CameraControllingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public CameraControllingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0)
        {
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Camera _camera, ref Transform _transform, ref Controller _controller)
        {
            if(_controller.IsHolding("CameraMoveLeft"))
            {
                _transform.Rotate(Vector3.UnitX * .1f);
            }
            if(_controller.IsHolding("CameraMoveRight"))
            {
                // _transform.Translate(Vector3.UnitX * 1/10);
                _transform.Rotate(-Vector3.UnitX * .1f);
            }
            if(_controller.IsHolding("CameraMoveUp"))
            {
                _transform.Translate(Vector3.UnitY * 1/10);
            }
            if(_controller.IsHolding("CameraMoveDown"))
            {
                _transform.Translate(-Vector3.UnitY * 1/10);
            }
            if(_controller.IsHolding("CameraZoomIn"))
            {
                _transform.Translate(-Vector3.UnitZ * 1/10);
            }
            if(_controller.IsHolding("CameraZoomOut"))
            {
                _transform.Translate(Vector3.UnitZ * 1/10);
            }
        }
    }
}