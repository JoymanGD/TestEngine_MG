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

namespace Common.ECS.Systems;

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

    private float AdvancedModulo(float Input, float Max)
    {
        return Input > 0 ? Input % Max : Max - Math.Abs(Input) % Max;
    }

    private Vector2 AdvancedModulo(Vector2 Input, Vector2 Max)
    {
        return new Vector2(AdvancedModulo(Input.X, Max.X), AdvancedModulo(Input.Y, Max.Y));
    }

    private Point AdvancedModulo(Point Input, Point Max)
    {
        return AdvancedModulo(Input.ToVector2(), Max.ToVector2()).ToPoint();
    }
    
    public InputSystem(World world, IParallelRunner runner) : base(world, CreateEntityContainer, null, 0)
    {
        this.world = world;
        this.runner = runner;
    }

    [Update]
    private void Update(ref Components.Core main, ref Input input)
    {
        UpdateStates();
        
        mousePosition = MouseState.Position;
        mouseDelta = mousePosition - oldMousePosition;
        scrollWheelValue = MathHelper.Clamp(MouseState.DeltaScrollWheelValue, -1, 1) * -1;

        var screenSize = GameSettings.Instance.ScreenSize.ToPoint();
        
        switch (input.CursorState)
        {
            case CursorState.None:
                break;
            case CursorState.Lock:
                mousePosition = new Point(screenSize.X / 2, screenSize.Y / 2);
                Mouse.SetPosition(mousePosition.X, mousePosition.Y);
                break;
        }
        
        oldMousePosition = mousePosition;
    }

    private void UpdateStates()
    {
        MouseState = MouseExtended.GetState();
        KeyboardState = KeyboardExtended.GetState();
    }
}