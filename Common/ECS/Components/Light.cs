using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public class Light
    {
        public Color DiffuseColor { get; private set; }
        public float Intensity { get; private set; }
        public float Range { get; private set; }
        public int ID;
        public LightType Type;
        public bool IsActive;

        public Light(LightType _type, Color _diffuseColor, float _intensity, bool _isActive = true, int _id = 0){
            Type = _type;
            DiffuseColor = _diffuseColor;
            Intensity = _intensity;
            ID = _id;
            IsActive = _isActive;
            Range = 1;
        }

        public Light(LightType _type, Color _diffuseColor, float _intensity, float _range, bool _isActive = true, int _id = 0){
            Type = _type;
            DiffuseColor = _diffuseColor;
            Intensity = _intensity;
            ID = _id;
            IsActive = _isActive;
            Range = _range;
        }
    }

    public enum LightType{
        Point, Directional
    }
}