using Microsoft.Xna.Framework;

namespace Common.ECS.Components{
    public class SimplePointLight : Light{
        public float Radius { get; private set;}
        public SimplePointLight(Color _diffuseColor, float _diffuseIntensity, float _radius, bool IsActive, int _id = 0) : base(LightType.Point, _diffuseColor, _diffuseIntensity, IsActive = true, _id){
            Radius = _radius;
        }
    }
}