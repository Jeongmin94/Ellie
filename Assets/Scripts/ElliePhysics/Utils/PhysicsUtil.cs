using System.Collections.Generic;
using UnityEngine;

namespace ElliePhysics.Utils
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
            var speed = (distance - 0.5f * Physics.gravity.magnitude * time * time) / time;
            return direction.normalized * speed;
        }

        public static Vector3[] CalculateTrajectoryPoints(
            Vector3 releasePosition,
            Vector3 direction,
            float strength,
            float time,
            int pointCount,
            LayerMask layerMask)
        {
            if (Equals(time, 0.0f))
            {
                return new Vector3[1] { releasePosition };
            }

            var startPosition = releasePosition;
            var startVelocity = direction * strength;

            var pointList = new List<Vector3>();
            var timeInterval = time / (pointCount + 1);
            var i = 0;

            pointList.Add(startPosition);
            for (var accInterval = timeInterval; accInterval < time; accInterval += timeInterval)
            {
                i++;
                if (i >= pointCount + 1)
                {
                    break;
                }

                var currentPosition = startPosition + startVelocity * accInterval;
                currentPosition.y = startPosition.y + startVelocity.y * accInterval +
                                    Physics.gravity.y / 2.0f * accInterval * accInterval;

                var prevPosition = pointList[i - 1];
                var prevToCurrent = currentPosition - prevPosition;

                if (Physics.Raycast(prevPosition, prevToCurrent.normalized, out var hit, prevToCurrent.magnitude,
                        layerMask))
                {
                    pointList.Add(hit.point);
                    break;
                }

                pointList.Add(currentPosition);
            }

            return pointList.ToArray();
        }
    }
}