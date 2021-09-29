using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using DefaultEcs.Command;

namespace Common.ECS.Systems
{
    [With(typeof(Controller))]
    [With(typeof(Player))]
    
    public partial class PlayerControllingSystem : AEntitySetSystem<GameTime>
    {
        private IParallelRunner runner;
        private World world;
        private EntityCommandRecorder EntityCommandRecorder = new EntityCommandRecorder();
        
        public PlayerControllingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Controller _controller, in Entity _entity){
            if(EntityCommandRecorder.Size > 0) EntityCommandRecorder.Clear();
//MOVEMENT AND ANIMATION/////////////////////////////////////////////////////////////////////////////////////////////////////
            //pressing
            if(_controller.WasPressed("MoveLeft") || _controller.WasPressed("MoveRight")){
                //animation
                if(_entity.Has<Animation>()){
                    var currentAnimation = _entity.Get<Animation>();
                    if(currentAnimation.Name != "Armature|Run"){
                        EntityCommandRecorder.Record(_entity).Remove<Animation>();
                    }
                }
                EntityCommandRecorder.Record(_entity).Set(new Animation("Armature|Run"));

                //movement and looking at
                if(_controller.WasPressed("MoveLeft")){
                    if(_entity.Has<Movement>()) EntityCommandRecorder.Record(_entity).Remove<Movement>();
                    EntityCommandRecorder.Record(_entity).Set(new Movement(Vector3.Left, .2f));
                    
                    if(_entity.Has<LookAt>()) EntityCommandRecorder.Record(_entity).Remove<LookAt>();
                    EntityCommandRecorder.Record(_entity).Set(new LookAt(Vector3.Left, Vector3.Up));
                }
                if(_controller.WasPressed("MoveRight")){
                    if(_entity.Has<Movement>()) EntityCommandRecorder.Record(_entity).Remove<Movement>();
                    EntityCommandRecorder.Record(_entity).Set(new Movement(Vector3.Right, .2f));
                    
                    if(_entity.Has<LookAt>()) EntityCommandRecorder.Record(_entity).Remove<LookAt>();
                    EntityCommandRecorder.Record(_entity).Set(new LookAt(Vector3.Right, Vector3.Up));
                }
            }

            //unpressing
            if(_controller.WasUnpressed("MoveLeft") || _controller.WasUnpressed("MoveRight")){
                //animation
                if(_entity.Has<Animation>()){
                    var currentAnimation = _entity.Get<Animation>();
                    if(currentAnimation.Name != "Armature|Idle"){
                        EntityCommandRecorder.Record(_entity).Remove<Animation>();
                    }
                }
                EntityCommandRecorder.Record(_entity).Set(new Animation("Armature|Idle"));

                //movement and looking at
                if(_controller.WasUnpressed("MoveLeft")){
                    if(_entity.Has<Movement>()) EntityCommandRecorder.Record(_entity).Remove<Movement>();
                    
                    if(_entity.Has<LookAt>()) EntityCommandRecorder.Record(_entity).Remove<LookAt>();
                }
                if(_controller.WasUnpressed("MoveRight")){
                    if(_entity.Has<Movement>()) EntityCommandRecorder.Record(_entity).Remove<Movement>();
                    
                    if(_entity.Has<LookAt>()) EntityCommandRecorder.Record(_entity).Remove<LookAt>();
                }
            }

//ROTATION/////////////////////////////////////////////////////////////////////////////////////////////////////
            //pressing
            if(_controller.WasPressed("RotateLeftDebug") || _controller.WasPressed("RotateRightDebug")){
                // if(_controller.WasPressed("RotateLeftDebug")){
                if(_controller.WasPressed("RotateLeftDebug")){
                    if(_entity.Has<Rotation>()) EntityCommandRecorder.Record(_entity).Remove<Rotation>();
                    // _entity.Set(new Rotation(Vector3.Right/10)); 
                    EntityCommandRecorder.Record(_entity).Set(new Rotation(Vector3.Right/10)); //////////IMPROVE
                }
                if(_controller.WasPressed("RotateRightDebug")){
                    if(_entity.Has<Rotation>()) EntityCommandRecorder.Record(_entity).Remove<Rotation>();
                    EntityCommandRecorder.Record(_entity).Set(new Rotation(Vector3.Left/10));
                }
            }

            //unpressing
            if(_controller.WasUnpressed("RotateLeftDebug") || _controller.WasUnpressed("RotateRightDebug")){
                if(_controller.WasUnpressed("RotateLeftDebug")){
                    if(_entity.Has<Rotation>()) EntityCommandRecorder.Record(_entity).Remove<Rotation>();
                }
                if(_controller.WasUnpressed("RotateRightDebug")){
                    if(_entity.Has<Rotation>()) EntityCommandRecorder.Record(_entity).Remove<Rotation>();
                }
            }

            if(EntityCommandRecorder.Size > 0) EntityCommandRecorder.Execute();
        }
    }
}