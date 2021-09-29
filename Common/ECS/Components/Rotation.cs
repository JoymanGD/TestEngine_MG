using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Rotation
    {
        public Vector3 Value { get; private set; }

        public Rotation(Vector3 _value){
            Value = _value;
        }
    }
}