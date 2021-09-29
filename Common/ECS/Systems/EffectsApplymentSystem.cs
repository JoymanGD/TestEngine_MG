using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(ModelRenderer))]
    [WhenAdded(typeof(Effect))]
    public partial class EffectsApplymentSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public EffectsApplymentSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            Runner = _runner;
        }

        protected override void PreUpdate(GameTime state)
        {
            base.PreUpdate(state);
        }

        [Update]
        private void Update(ref ModelRenderer _modelRenderer, ref Effect _effect)
        {
            foreach (ModelMesh mesh in _modelRenderer.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                }
            }
        }

        protected override void PostUpdate(GameTime state)
        {
            base.PostUpdate(state);
        }
    }
}