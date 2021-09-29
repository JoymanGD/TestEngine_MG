using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Movement
    {
        public Vector3 Direction { get; private set; }
        public float Speed { get; private set; }

        public Movement(Vector3 _direction, float _speed){
            Direction = _direction;
            Speed = _speed;
        }

        public void SetSpeed(float _speed){
            Speed = _speed;
        }

        public void SetDirection(Vector3 _direction){
            Direction = _direction;
        }
    }
}