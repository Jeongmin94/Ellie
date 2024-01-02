using UnityEngine;

namespace Utils
{
    public static class CalculateHelper
    {
        public static Vector3 RandomPositionInSector(int numberOfSector, int sectorIndex, float fieldRadius, float fieldHeight)
        {
            var sectorAngleSize = 360f / numberOfSector;
            var minAngle = sectorAngleSize * sectorIndex;
            var maxAngle = minAngle + sectorAngleSize;

            var angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
            var distance = Mathf.Sqrt(Random.Range(0f, 1f)) * fieldRadius;

            return new Vector3(
                Mathf.Cos(angle) * distance,
                fieldHeight,
                Mathf.Sin(angle) * distance
            );
        }
    }
}
