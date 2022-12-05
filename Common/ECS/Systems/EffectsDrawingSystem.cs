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
    public partial class EffectsDrawingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public EffectsDrawingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            Runner = _runner;
        }

        [Update]
        private void Update(ref Transform _transform, ref ModelRenderer _modelRenderer)
        {
            var cameras = World.Get<Camera>();
            var model = _modelRenderer.Model;
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var camera in cameras)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (Effect effect in mesh.Effects)
                    {
                        effect.Parameters["WorldMatrix"].SetValue(transforms[mesh.ParentBone.Index] * _transform.WorldMatrix);
                        effect.Parameters["ViewMatrix"].SetValue((Matrix)camera.ViewMatrix);
                        effect.Parameters["ProjectionMatrix"].SetValue((Matrix)camera.ProjectionMatrix);
                    }
                }
            }
        }
    }
}