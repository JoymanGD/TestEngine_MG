// using DefaultEcs;
// using DefaultEcs.System;
// using DefaultEcs.Threading;
// using Microsoft.Xna.Framework;
// using Common.ECS.Components;
// using tainicom.Aether.Animation;

// namespace Common.ECS.Systems
// {
//     [With(typeof(ModelRenderer))]
//     [With(typeof(Animation))]
//     [With(typeof(Transform))]
//     public partial class AnimationPlayingSystem : AEntitySetSystem<GameTime>
//     {
//         private IParallelRunner Runner;
        
//         public AnimationPlayingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
//             Runner = _runner;
//         }

//         protected override void PreUpdate(GameTime state)
//         {
//             base.PreUpdate(state);
//         }

//         [Update]
//         private void Update(ref ModelRenderer _modelRenderer, in Animation _animation, in Transform _transform, GameTime _gameTime)
//         {
//             var model = _modelRenderer.Model;
//             var anims = model.GetAnimations();
//             var elapsedTime = _gameTime.ElapsedGameTime;
            
//             if(anims.CurrentClip != null){
//                 anims.Update(elapsedTime, true, Matrix.Identity);

//                 foreach (var mesh in model.Meshes)
//                 {
//                     foreach (var part in mesh.MeshParts)
//                     {
//                         part.UpdateVertices(anims.AnimationTransforms);
//                     }
//                 }
//             }
//         }

//         protected override void PostUpdate(GameTime state)
//         {
//             base.PostUpdate(state);
//         }
//     }
// }