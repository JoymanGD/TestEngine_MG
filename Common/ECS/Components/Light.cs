using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public struct Light
    {
        public LightType Type;
        public Color Color;
        public float Radius;
        public float Intensity;
        public bool IsActive;

        public Light(LightType type, Color color, float intensity = 1, float radius = 1, bool isActive = true)
        {
            IsActive = isActive;
            Type = type;
            Color = color;
            Intensity = intensity;
            Radius = radius;
        }
    }

    public enum LightType
    {
        Directional = 0,
        Point = 1
    }
}