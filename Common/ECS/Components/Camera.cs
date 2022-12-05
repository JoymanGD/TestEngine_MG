using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public struct Camera
    {
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        public Camera(GraphicsDeviceManager _graphics, Matrix _worldMatrix){
            ViewMatrix = Matrix.CreateLookAt(_worldMatrix.Translation, _worldMatrix.Forward + _worldMatrix.Translation, _worldMatrix.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _graphics.PreferredBackBufferWidth / (float)_graphics.PreferredBackBufferHeight, 1, 100);
        }

        public void ApplyWorldMatrix(Matrix _worldMatrix){
            ViewMatrix = Matrix.Invert(_worldMatrix);
            // var target = _worldMatrix.Translation + _worldMatrix.Forward;
            // ViewMatrix = Matrix.CreateLookAt(_worldMatrix.Translation, target, _worldMatrix.Up);
        }
    }
}