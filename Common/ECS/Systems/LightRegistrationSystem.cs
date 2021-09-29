using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Common.Settings;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [WhenAdded(typeof(Light))]
    public partial class LightRegistrationSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public LightRegistrationSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            Runner = _runner;
        }

        protected override void PreUpdate(GameTime state)
        {
            base.PreUpdate(state);
        }

        [Update]
        private void Update(ref Light _light)
        {
            if(_light.Type == LightType.Directional){
                _light.ID = GameSettings.Instance.DirectionalLightsCount++;

                var effects = World.Get<Effect>();

                foreach (var effect in effects)
                {
                    effect.Parameters["ActiveDirectionalLightsCount"].SetValue(GameSettings.Instance.DirectionalLightsCount);
                }
            }
            else if(_light.Type == LightType.Point){
                _light.ID = GameSettings.Instance.PointLightsCount++;

                var effects = World.Get<Effect>();

                foreach (var effect in effects)
                {
                    effect.Parameters["ActivePointLightsCount"].SetValue(GameSettings.Instance.PointLightsCount);
                }
            }
        }

        protected override void PostUpdate(GameTime state)
        {
            base.PostUpdate(state);
        }
    }
}