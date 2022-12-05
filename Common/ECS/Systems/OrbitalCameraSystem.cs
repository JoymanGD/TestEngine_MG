using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using System;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Common.Settings;
using System.Windows.Forms;

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
        private void Update(ref OrbitalCamera orbital, ref Transform transform, ref Controller _controller, GameTime _gameTime){
            float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 oldTargetPosition = orbital.OldTargetPosition;
            Vector3 targetPosition = orbital.Target.Position;
            Vector2 mouseDelta = GetMouseDelta();
            // Console.WriteLine($"pos: {_controller.MousePosition}; delta: {_controller.MouseDelta}");

            var lerpedPosition = Vector3.Lerp(oldTargetPosition, targetPosition, elapsedTime * orbital.FollowSpeed/SPEED_COEF);

            transform.Position = lerpedPosition;

            orbital.RotationX += -mouseDelta.X * elapsedTime * orbital.OrbitSpeed/SPEED_COEF;
            orbital.RotationY += -mouseDelta.Y * elapsedTime * orbital.OrbitSpeed/SPEED_COEF;
            orbital.RotationY = MathHelper.Clamp(orbital.RotationY, orbital.MinY, orbital.MaxY);

            transform.Rotation = Quaternion.CreateFromYawPitchRoll(orbital.RotationX, orbital.RotationY, 0);

            var endPoint = transform.Position
                                            - transform.Forward * orbital.Offset.Z
                                            + transform.WorldMatrix.Right * orbital.Offset.X
                                            + transform.WorldMatrix.Up * orbital.Offset.Y;

            transform.Position = endPoint;

            //update old target position
            orbital.OldTargetPosition = lerpedPosition;
        }

        private Vector2 GetMouseDelta()
        {
            var resetPosition = GameSettings.Instance.CenterPosition;
            var mousePosition = Cursor.Position;
            var mouseDelta = new Vector2(mousePosition.X - resetPosition.X, mousePosition.Y - resetPosition.Y);

            Cursor.Position = new System.Drawing.Point((int)resetPosition.X, (int)resetPosition.Y);

            return mouseDelta;
        }
    }
}