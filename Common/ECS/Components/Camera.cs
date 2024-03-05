using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public struct Camera
    {
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        public Camera(GraphicsDeviceManager graphics, Matrix worldMatrix)
        {
            ViewMatrix = Matrix.CreateLookAt(worldMatrix.Translation, worldMatrix.Forward + worldMatrix.Translation, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight, 1f, 1000);
            // ProjectionMatrix = Matrix.CreatePerspective(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, 1, 100);
        }

        public void ApplyWorldMatrix(Matrix _worldMatrix)
        {
            ViewMatrix = Matrix.Invert(_worldMatrix);
            // var target = _worldMatrix.Translation + _worldMatrix.Forward;
            // ViewMatrix = Matrix.CreateLookAt(_worldMatrix.Translation, target, _worldMatrix.Up);
        }
    }
}