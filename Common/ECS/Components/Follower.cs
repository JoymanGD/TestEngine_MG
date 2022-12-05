using Microsoft.Xna.Framework;

namespace Common.ECS.Components
{
    public struct Follower
    {
        public Transform FollowTarget { get; private set; }
        public float FollowSpeed { get; private set; }
        public Vector3 Offset { get; private set; }

        public Follower(Transform followTarget, float followSpeed)
        {
            FollowTarget = followTarget;
            FollowSpeed = followSpeed;
            Offset = Vector3.Zero;
        }

        public Follower(Transform followTarget, float followSpeed, Vector3 offset)
        {
            FollowTarget = followTarget;
            FollowSpeed = followSpeed;
            Offset = offset;
        }
    }
}