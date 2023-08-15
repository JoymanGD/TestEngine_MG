using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using System;

namespace Common.ECS.Systems
{
    [With(typeof(Transform))]
    [With(typeof(Follower))]
    public partial class FollowingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        
        public FollowingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Transform transform, ref Follower follower, GameTime _gameTime){
            float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 targetPosition = follower.FollowTarget.Position;
            
            var followingPosition = targetPosition + follower.Offset;
            transform.Position = Vector3.Lerp(transform.Position, followingPosition, elapsedTime * follower.FollowSpeed);
        }
    }
}