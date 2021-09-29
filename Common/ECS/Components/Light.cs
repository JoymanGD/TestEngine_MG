using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public class Light
    {
        public Color AmbientColor { get; private set; }
        public float AmbientIntensity { get; private set; }
        public Color DiffuseColor { get; private set; }
        public float DiffuseIntensity { get; private set; }
        public int ID;
        public LightType Type;
        public bool IsActive;

        public Light(LightType _type, Color _diffuseColor, float _diffuseIntensity, bool _isActive = true, int _id = 0){
            Type = _type;
            DiffuseColor = _diffuseColor;
            DiffuseIntensity = _diffuseIntensity;
            AmbientColor = _diffuseColor;
            AmbientIntensity = _diffuseIntensity/2;
            ID = _id;
            IsActive = _isActive;
        }

        public Light(LightType _type, Color _ambientColor, float _ambientIntensity, Color _diffuseColor, float _diffuseIntensity, bool _isActive = true, int _id = 0){
            Type = _type;
            DiffuseColor = _diffuseColor;
            DiffuseIntensity = _diffuseIntensity;
            AmbientColor = _diffuseColor;
            AmbientIntensity = _ambientIntensity/2;
            ID = _id;
            IsActive = _isActive;
        }
    }

    public enum LightType{
        Point, Directional
    }
}