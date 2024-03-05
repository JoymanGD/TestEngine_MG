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
    [With(typeof(Zoom))]
    public partial class OrbitalCameraSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        private const float SPEED_COEF = 20;
        
        public OrbitalCameraSystem(World world, IParallelRunner runner) : base(world, CreateEntityContainer, null, 0)
        {
            this.world = world;
            this.runner = runner;
        }

        [Update]
        private void Update(ref OrbitalCamera orbital, ref Transform transform, ref Controller controller, GameTime gameTime, Zoom zoom)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 targetPosition = orbital.Target.Position;
            Vector2 mouseDelta = InputSystem.MouseDelta;

            transform.Position = targetPosition + orbital.Offset;

            orbital.RotationX += -mouseDelta.X * elapsedTime * orbital.OrbitSpeed/SPEED_COEF;
            orbital.RotationY += -mouseDelta.Y * elapsedTime * orbital.OrbitSpeed/SPEED_COEF;
            orbital.RotationY = MathHelper.Clamp(orbital.RotationY, orbital.MinY, orbital.MaxY);

            transform.Rotation = Quaternion.CreateFromYawPitchRoll(orbital.RotationX, orbital.RotationY, 0);

            transform.Position -= transform.Forward * zoom.Value;
        }
    }
}