using Microsoft.Xna.Framework;

namespace Common.ECS.Components{
    public class SimpleDirectionalLight : Light{
        public SimpleDirectionalLight(Color _diffuseColor, float _diffuseIntensity, bool IsActive = true, int _id = 0) : base(LightType.Directional, _diffuseColor, _diffuseIntensity, IsActive, _id){}
    }
}