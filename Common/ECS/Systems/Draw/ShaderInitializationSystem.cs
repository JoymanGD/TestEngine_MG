using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common.ECS.Components;

namespace Common.ECS.Systems
{
    [With(typeof(ModelRenderer))]
    [WhenAdded(typeof(Material))]
    public partial class ShaderInitializationSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public ShaderInitializationSystem(World world, IParallelRunner runner) : base(world, CreateEntityContainer, null, 0)
        {
            Runner = runner;
        }

        [Update]
        private void Update(ref ModelRenderer modelRenderer, ref Material material)
        {
            foreach (ModelMesh mesh in modelRenderer.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = material.Shader;
                }
            }
        }
    }
}