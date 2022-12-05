using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct LookAtTransform
    {
        public Transform Target { get; private set; }
        public bool Smooth { get; private set; }

        public LookAtTransform(Transform target, bool smooth = false){
            Target = target;
            Smooth = smooth;
        }
    }
}