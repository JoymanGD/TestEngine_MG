using Microsoft.Xna.Framework.Graphics;

namespace Common.ECS.Components
{
    public struct Player
    {
        public int Id { get; private set; }

        public Player(int _id = 0){
            Id = _id;
        }

        public void SetId(int _id){
            Id = _id;
        }
    }
}