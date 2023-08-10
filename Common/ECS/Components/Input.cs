using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Input
    {
        public bool ClipCursor;

        public Input(bool clipCursor)
        {
            ClipCursor = clipCursor;
        }
    }
}