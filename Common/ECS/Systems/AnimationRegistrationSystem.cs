using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using tainicom.Aether.Animation;

namespace Common.ECS.Systems
{
    [With(typeof(ModelRenderer))]
    [WhenAdded(typeof(Animation))]
    public partial class AnimationRegistrationSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner Runner;
        
        public AnimationRegistrationSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            Runner = _runner;
        }

        protected override void PreUpdate(GameTime state)
        {
            base.PreUpdate(state);
        }

        [Update]
        private void Update(ref ModelRenderer _modelRenderer, in Animation _animation)
        {
            var model = _modelRenderer.Model;
            var anims = model.GetAnimations();
            var clip = anims.Clips[_animation.Name];
            if(clip != null)
                anims.SetClip(clip);
        }

        protected override void PostUpdate(GameTime state)
        {
            base.PostUpdate(state);
        }
    }
}