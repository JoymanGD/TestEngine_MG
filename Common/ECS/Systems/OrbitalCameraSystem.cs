using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using System;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Common.Settings;

namespace Common.ECS.Systems
{
    [With(typeof(Camera))]
    [With(typeof(OrbitalCamera))]
    [With(typeof(Transform))]
    [With(typeof(Controller))]
    public partial class OrbitalCameraSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        private const float SPEED_COEF = 10;
        
        public OrbitalCameraSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref OrbitalCamera orbital, ref Transform transform, ref Controller controller, GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 targetPosition = orbital.Target.Position;
            Vector2 mouseDelta = InputSystem.MouseDelta;

            transform.Position = targetPosition;

            orbital.RotationX += -mouseDelta.X * elapsedTime * orbital.OrbitSpeed/SPEED_COEF;
            orbital.RotationY += -mouseDelta.Y * elapsedTime * orbital.OrbitSpeed/SPEED_COEF;
            orbital.RotationY = MathHelper.Clamp(orbital.RotationY, orbital.MinY, orbital.MaxY);

            transform.Rotation = Quaternion.CreateFromYawPitchRoll(orbital.RotationX, orbital.RotationY, 0);

            var endPoint = transform.Position
                                            - transform.Forward * orbital.Offset.Z
                                            + transform.WorldMatrix.Right * orbital.Offset.X
                                            + transform.WorldMatrix.Up * orbital.Offset.Y;

            transform.Position = endPoint;
        }
    }
}