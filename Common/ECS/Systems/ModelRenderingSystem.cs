using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(ModelRenderer))]
    public partial class ModelRenderingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public ModelRenderingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            Runner = _runner;
        }

        [Update]
        private void Update(ref ModelRenderer _renderer)
        {
            foreach (var mesh in _renderer.Model.Meshes)
            {
                mesh.Draw();
            }
        }
    }
}