using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using DefaultEcs.Command;

namespace Common.ECS.Systems
{
    [With(typeof(Controller))]
    [With(typeof(Player))]
    
    public partial class PlayerControllingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        private EntityCommandRecorder EntityCommandRecorder = new EntityCommandRecorder();
        
        public PlayerControllingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Controller _controller, in Entity _entity)
        {
            CalculateCameraVectors(out Vector3 cameraRight, out Vector3 cameraForward);

            if(EntityCommandRecorder.Size > 0)
            {
                EntityCommandRecorder.Clear();
            }

            Vector2 inputVector = _controller.GetInputVector("MoveLeft", "MoveRight", "MoveForward", "MoveBack");
            Vector3 moveDirection = cameraForward * inputVector.Y + cameraRight * inputVector.X;

            if(moveDirection != Vector3.Zero)
            {
                moveDirection.Normalize();
                EntityCommandRecorder.Record(_entity).Set(new LookAt(moveDirection, Vector3.Up, true));
            }

            EntityCommandRecorder.Record(_entity).Set(new Movement(moveDirection, .2f));

            if(EntityCommandRecorder.Size > 0)
            {
                EntityCommandRecorder.Execute();
            }
        }

        private void CalculateCameraVectors(out Vector3 right, out Vector3 forward)
        {
            var camera = World.Get<Camera>()[0];
            var cameraWorld = Matrix.Invert(camera.ViewMatrix);

            right = cameraWorld.Right;
            forward = cameraWorld.Forward;

            right.Y = 0;
            forward.Y = 0;

            right.Normalize();
            forward.Normalize();
        }
    }
}