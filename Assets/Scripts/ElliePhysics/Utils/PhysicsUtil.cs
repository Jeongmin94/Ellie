using UnityEngine;

namespace Assets.Scripts.ElliePhysics.Utils
{
    public static class PhysicsUtil
    {
        public static Vector3 GetDistance(Vector3 velocity, Vector3 accel, float time)
        {
            return velocity * time + 0.5f * time * time * accel;
        }

        public static Vector3 GetVelocity(Vector3 velocity, Vector3 accel, float time)
        {
            return velocity + accel * time;
        }

        public static Vector3 CalculateInitialVelocity(Vector3 direction, float time, float distance)
        {
            float speed = (distance - 0.5f * Physics.gravity.magnitude * time * time) / time;
            return direction.normalized * speed;
        }

        public static Vector3[] CalculateTrajectoryPoints(
            Vector3 releasePosition,
            Vector3 direction,
            float strength,
            float moveTime,
            int pointCount)
        {
            Vector3 startPosition = releasePosition;
            Vector3 startVelocity = direction * strength;

            Vector3[] points = new Vector3[pointCount + 1];

            float timeInterval = moveTime / (float)(pointCount + 1);
            int i = 0;

            points[0] = startPosition;
            for (float accInterval = timeInterval; accInterval < moveTime; accInterval += timeInterval)
            {
                i++;
                if (i >= pointCount + 1)
                    break;

                float time = accInterval;
                Vector3 currentPosition = startPosition + startVelocity * time;
                currentPosition.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2.0f * time * time);

                points[i] = currentPosition;
            }

            return points;
        }
        
    }
}