using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Animation
    {
        public string Name { get; private set; }

        public Animation(string _name){
            Name = _name;
        }

        public void SetName(string _name){
            Name = _name;
        }
    }
}