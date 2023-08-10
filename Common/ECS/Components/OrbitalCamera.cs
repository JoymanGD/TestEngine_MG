using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public struct OrbitalCamera
    {
        public Transform Target { get; private set; }
        public float OrbitSpeed { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }
        public Vector3 Offset { get; private set; }
        public float RotationX;
        public float RotationY;

        public OrbitalCamera(Transform target, float orbitSpeed, Vector3 offset)
        {
            Target = target;
            MinY = 0;
            MaxY = 90;
            OrbitSpeed = orbitSpeed;
            RotationX = 0;
            RotationY = 0;
            Offset = offset;
        }

        public OrbitalCamera(Transform target, float minY, float maxY, float orbitSpeed, Vector3 offset)
        {
            Target = target;
            MinY = minY;
            MaxY = maxY;
            OrbitSpeed = orbitSpeed;
            RotationX = 0;
            RotationY = 0;
            Offset = offset;
        }
    }
}