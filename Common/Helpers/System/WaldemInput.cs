using System;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Common.Helpers.System
{
    public static class WaldemInput
    {
        public static bool IsButtonDown(ExtendedStates _states, WaldemButtons _button)
        {
            var keyboardState = _states.KeyboardState;
            var mouseState = _states.MouseState;

            var intButton = (int)_button;
            if(intButton < 8)
            {
                return MouseExtended.GetState().IsButtonDown((MouseButton)intButton);
            }
            else if(intButton > 7)
            {
                return KeyboardExtended.GetState().IsKeyDown((Keys)intButton);
            }

            return false;
        }

        public static bool IsButtonUp(ExtendedStates _states, WaldemButtons _button)
        {
            var keyboardState = _states.KeyboardState;
            var mouseState = _states.MouseState;

            var intButton = (int)_button;
            if(intButton < 8)
            {
                return MouseExtended.GetState().IsButtonUp((MouseButton)intButton);
            }
            else if(intButton > 7)
            {
                return KeyboardExtended.GetState().IsKeyUp((Keys)intButton);
            }

            return false;
        }

        public static bool WasButtonJustDown(ExtendedStates _states, WaldemButtons _button)
        {
            var keyboardState = _states.KeyboardState;
            var mouseState = _states.MouseState;

            var intButton = (int)_button;

            if(intButton < 8)
            {
                return mouseState.WasButtonJustDown((MouseButton)intButton);
            }
            else if(intButton > 7)
            {
                return keyboardState.WasKeyJustUp((Keys)intButton);
            }

            return false;
        }
        
        public static bool WasButtonJustUp(ExtendedStates _states, WaldemButtons _button)
        {
            var keyboardState = _states.KeyboardState;
            var mouseState = _states.MouseState;

            var intButton = (int)_button;
            
            if(intButton < 8)
            {
                return mouseState.WasButtonJustUp((MouseButton)intButton);
            }
            else if(intButton > 7)
            {
                return keyboardState.WasKeyJustDown((Keys)intButton);
            }

            return false;
        }
    }

    public struct ExtendedStates{
        public MouseStateExtended MouseState;
        public KeyboardStateExtended KeyboardState;

        public ExtendedStates(MouseStateExtended _mouseState, KeyboardStateExtended _keyboardState){
            MouseState = _mouseState;
            KeyboardState = _keyboardState;
        }
    }
}