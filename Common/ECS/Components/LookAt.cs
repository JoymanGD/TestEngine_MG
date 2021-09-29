using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct LookAt
    {
        public Vector3 Forward { get; private set; }
        public Vector3 Up { get; private set; }

        public LookAt(Vector3 _forward, Vector3 _up){
            Forward = _forward;
            Up = _up;
        }
    }
}