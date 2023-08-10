using System.Linq;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Common.Helpers.System;
using MonoGame.Extended.Input;

namespace Common.ECS.Systems
{
    [With(typeof(Controller))]
    [With(typeof(Bindings))]
    public partial class BindingsReadingSystem : AEntitySetSystem<GameTime>
    {        
        private IParallelRunner runner;
        private World world;
        
        public BindingsReadingSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0)
        {
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Controller controller, ref Bindings _bindings)
        {
            ControlButtonsPressing(InputSystem.MouseState, InputSystem.KeyboardState, ref controller, ref _bindings);
        }

        void ControlButtonsPressing(MouseStateExtended _mouseState, KeyboardStateExtended _keyboardState, ref Controller _controller, ref Bindings _bindings)
        {
            ExtendedStates states = new ExtendedStates(_mouseState, _keyboardState);
            
            foreach (var item in _controller.Holdings.ToList())
            {
                var value = WaldemInput.IsButtonDown(states, (WaldemButtons)_bindings.Pairs[item.Key]);
                _controller.SetHolding(item.Key, value);
            }
            foreach (var item in _controller.Pressings.ToList())
            {
                var value = WaldemInput.WasButtonJustDown(states, (WaldemButtons)_bindings.Pairs[item.Key]);
                _controller.SetPressing(item.Key, value);
            }
            foreach (var item in _controller.Unpressings.ToList())
            {
                var value = WaldemInput.WasButtonJustUp(states, (WaldemButtons)_bindings.Pairs[item.Key]);
                _controller.SetUnpressing(item.Key, value);
            }
        }
    }
}