using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [With(typeof(Light))]
    public partial class LightingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public LightingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            Runner = _runner;
        }

        protected override void PreUpdate(GameTime state)
        {
            base.PreUpdate(state);
        }

        [Update]
        private void Update(ref Transform _transform, ref Light _light)
        {            
            if(_light.IsActive)
            {
                var effects = World.Get<Effect>();

                if(_light.Type == LightType.Directional){
                    foreach (var effect in effects)
                    {
                        // effect.Parameters["DirectionalLightsAmbientColors"].Elements[_light.ID].SetValue(_light.AmbientColor.ToVector4());
                        // effect.Parameters["DirectionalLightsAmbientIntensities"].Elements[_light.ID].SetValue(_light.AmbientIntensity);
                        effect.Parameters["DirectionalLightsDiffuseColors"].Elements[_light.ID].SetValue(_light.DiffuseColor.ToVector4());
                        effect.Parameters["DirectionalLightsDiffuseIntensities"].Elements[_light.ID].SetValue(_light.Intensity);
                        effect.Parameters["DirectionalLightsDirections"].Elements[_light.ID].SetValue(_transform.WorldMatrix.Forward);
                    }
                }
                else if(_light.Type == LightType.Point){
                    foreach (var effect in effects)
                    {
                        // effect.Parameters["PointLightsAmbientColors"].Elements[_light.ID].SetValue(_light.AmbientColor.ToVector4());
                        // effect.Parameters["PointLightsAmbientIntensities"].Elements[_light.ID].SetValue(_light.AmbientIntensity);
                        effect.Parameters["PointLightsDiffuseColors"].Elements[_light.ID].SetValue(_light.DiffuseColor.ToVector4());
                        effect.Parameters["PointLightsIntensities"].Elements[_light.ID].SetValue(_light.Intensity);
                        effect.Parameters["PointLightsPositions"].Elements[_light.ID].SetValue(_transform.Position);
                        effect.Parameters["PointLightsRadii"].Elements[_light.ID].SetValue(_light.Range);
                    }
                }
            }
        }

        protected override void PostUpdate(GameTime state)
        {
            base.PostUpdate(state);
        }
    }
}