using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct CameraZoom
    {
        public float ZoomSpeed { get; private set; }

        public CameraZoom(float zoomSpeed)
        {
            ZoomSpeed = zoomSpeed;
        }
    }
}