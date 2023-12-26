using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class HybridConeRange : BaseRange
{
    public float Radius { get; private set; }
    public float Angle { get; private set; }
    public float UpperBase { get; private set; }

    public override void Init(GameObject rangeObject, RangePayload payload)
    {
        base.Init(rangeObject, payload);

        RangeObject.name = "HybridConeRange";
    }


    public override void CreateRange(RangePayload payload)
    {
        Radius = payload.Radius;
        Angle = payload.Angle;
        UpperBase = payload.UpperBase;

        DetectionMaterial = payload.DetectionMaterial;

        // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
        MeshFilter meshFilter = RangeObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = RangeObject.AddComponent<MeshRenderer>();

        if (!IsShowRange)
        {
            return;
        }

        Mesh mesh = new Mesh();

        // 부채꼴의 중심각에 비례하여 세그먼트의 수를 결정합니다.
        int segments = Mathf.CeilToInt(Angle);
        Vector3[] vertices = new Vector3[segments + 3];
        int[] triangles = new int[(segments + 2) * 3];

        float currentAngle = -Angle / 2;
        float deltaAngle = Angle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float radian = currentAngle * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Sin(radian) * Radius, 0, Mathf.Cos(radian) * Radius);
            currentAngle += deltaAngle;
        }

        // 중심점에서 UpperBase만큼 떨어진 위치에 두 개의 꼭짓점을 추가합니다.
        vertices[0] = new Vector3(-UpperBase / 2, 0, 0);
        vertices[segments + 2] = new Vector3(UpperBase / 2, 0, 0);

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // 사다리꼴 부분의 삼각형을 설정합니다.
        triangles[segments * 3] = 0;
        triangles[segments * 3 + 1] = segments + 1;
        triangles[segments * 3 + 2] = segments + 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = DetectionMaterial;
    }

    public override List<Transform> CheckRange(string checkTag = "", int layerMask = -1)
    {
        Transform objTransform = RangeObject.transform;

        // 하이브리드 범위의 중심과 방향을 계산합니다.
        Vector3 center = objTransform.position;
        Quaternion direction = objTransform.rotation;

        // 부채꼴 범위의 현(Chord)의 길이를 계산합니다.
        float lowerBase = 2 * Radius * Mathf.Sin(Mathf.Deg2Rad * Angle / 2);

        // 하이브리드 범위 내의 모든 콜라이더를 감지합니다.
        Collider[] collidersInHybrid = Physics.OverlapSphere(center, Radius, layerMask);

        List<Transform> targets = new List<Transform>();
        foreach (Collider collider in collidersInHybrid)
        {
            if (checkTag == "" || collider.CompareTag(checkTag))
            {
                // 이 콜라이더의 위치를 하이브리드 범위의 로컬 좌표계로 변환합니다.
                Vector3 localPoint = objTransform.InverseTransformPoint(collider.transform.position);

                // 부채꼴 범위 내에 있는지 확인합니다.
                if (localPoint.z >= 0 && localPoint.z <= Radius && Mathf.Abs(Mathf.Atan2(localPoint.x, localPoint.z) * Mathf.Rad2Deg) <= Angle / 2)
                {
                    targets.Add(collider.transform);
                }
                // 사다리꼴 범위 내에 있는지 확인합니다.
                else if (localPoint.z >= 0 && localPoint.z <= Radius)
                {
                    float halfWidthAtThisPoint = Mathf.Lerp(UpperBase / 2, lowerBase / 2, localPoint.z / Radius);
                    if (Mathf.Abs(localPoint.x) <= halfWidthAtThisPoint)
                    {
                        targets.Add(collider.transform);
                    }
                }
            }
        }

        return targets;
    }
}
