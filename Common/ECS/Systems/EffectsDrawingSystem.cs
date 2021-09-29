using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Systems
{
    [With(typeof(ModelRenderer))]
    [With(typeof(Transform))]
    [With(typeof(Effect))]
    public partial class EffectsDrawingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public EffectsDrawingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            Runner = _runner;
        }

        protected override void PreUpdate(GameTime state)
        {
            base.PreUpdate(state);
        }

        [Update]
        private void Update(ref Transform _transform, ref Effect _effect)
        {
            var cameraEntities = World.GetEntities().With<Camera>().With<Transform>().AsEnumerable();

            foreach (var cameraEntity in cameraEntities)
            {
                var camera = cameraEntity.Get<Camera>();

                _effect.Parameters["WorldMatrix"].SetValue(_transform.WorldMatrix);
                _effect.Parameters["ViewMatrix"].SetValue(camera.ViewMatrix);
                _effect.Parameters["ProjectionMatrix"].SetValue(camera.ProjectionMatrix);
                Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(_transform.WorldMatrix));
                _effect.Parameters["WorldMatrix3x3"].SetValue(_transform.WorldMatrix);
            }
        }

        protected override void PostUpdate(GameTime state)
        {
            base.PostUpdate(state);
        }
    }
}