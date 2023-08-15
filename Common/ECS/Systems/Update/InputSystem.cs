using System.Linq;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Common.ECS.Components;
using Common.Helpers.System;
using MonoGame.Extended.Input;
using Common.Settings;
using System;
using Microsoft.Xna.Framework.Input;

namespace Common.ECS.Systems
{
    [With(typeof(Components.Core))]
    [With(typeof(Input))]
    public partial class InputSystem : AEntitySetSystem<GameTime>
    {
        public static MouseStateExtended MouseState;
        public static KeyboardStateExtended KeyboardState;
        public static Vector2 MousePosition => mousePosition.ToVector2();
        public static Vector2 MouseDelta => mouseDelta.ToVector2();
        public static float ScrollWheelValue => scrollWheelValue;

        private static Point oldMousePosition;
        private static Point mousePosition;
        private static Point mouseDelta;
        private static float scrollWheelValue;
        private IParallelRunner runner;
        private World world;
        
        public InputSystem(World _world, IParallelRunner _runner) : base(_world, CreateEntityContainer, null, 0){
            world = _world;
            runner = _runner;
        }

        [Update]
        private void Update(ref Components.Core main, ref Input input)
        {
            UpdateStates();

            oldMousePosition = mousePosition;
            mousePosition = MouseState.Position;

            if(!input.ClipCursor)
            {
                var screenSize = GameSettings.Instance.ScreenSize.ToPoint();

                if(mousePosition.X <= 0)
                {
                    Mouse.SetPosition(screenSize.X-1, mousePosition.Y);
                    UpdateStates();
                    oldMousePosition = mousePosition = MouseState.Position;
                }
                else if(mousePosition.X >= screenSize.X)
                {
                    Mouse.SetPosition(1, mousePosition.Y);
                    UpdateStates();
                    oldMousePosition = mousePosition = MouseState.Position;
                }
            }

            mouseDelta = mousePosition - oldMousePosition;
            scrollWheelValue = MathHelper.Clamp(MouseState.DeltaScrollWheelValue, -1, 1) * -1;
            
            // if(mouseState.DeltaPosition != Point.Zero)
            // {
            //     Console.WriteLine($"pos: {MousePosition}, delta: {MouseDelta}");
            // }
        }

        private void UpdateStates()
        {
            MouseState = MouseExtended.GetState();
            KeyboardState = KeyboardExtended.GetState();
        }
    }
}