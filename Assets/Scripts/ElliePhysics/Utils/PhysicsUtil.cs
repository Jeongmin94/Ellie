using UnityEngine;

namespace Assets.Scripts.ElliePhysics.Utils
{
    public static class PhysicsUtil
    {
        public static Vector3 GetDistance(Vector3 velocity, Vector3 accel, float time)
        {
            return velocity * time + 0.5f * time * time * accel;
        }
    }
}
