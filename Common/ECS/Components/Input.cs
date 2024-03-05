using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Input
    {
        public CursorState CursorState;

        public Input(CursorState cursorState)
        {
            CursorState = cursorState;
        }
    }

    public enum CursorState
    {
        None = 0,
        Lock = 1,
    }
}