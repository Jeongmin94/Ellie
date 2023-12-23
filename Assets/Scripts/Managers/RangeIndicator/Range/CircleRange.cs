﻿using System.Collections.Generic;
using UnityEngine;

public class CircleRange : BaseRange
{
    public float Radius { get; private set; }
    public float Angle { get; private set; }

    public override void Init(GameObject rangeObject, RangePayload payload)
    {
        base.Init(rangeObject, payload);

        RangeObject.name = "CircleRange";
    }

    public override void CreateRange(RangePayload payload)
    {
        Radius = payload.Radius;
        Angle = 360.0f;

        DetectionMaterial = payload.DetectionMaterial;

        // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
        var meshFilter = RangeObject.AddComponent<MeshFilter>();
        var meshRenderer = RangeObject.AddComponent<MeshRenderer>();

        if (!IsShowRange)
        {
            return;
        }

        var mesh = new Mesh();

        // 부채꼴의 중심각에 비례하여 세그먼트의 수를 결정합니다.
        var segments = Mathf.CeilToInt(Angle); // 여기에서 세그먼트의 수를 적절하게 조절합니다.
        var vertices = new Vector3[segments + 2];
        var triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // 원의 중심점
        var currentAngle = -Angle / 2; // 부채꼴의 시작 각도
        var deltaAngle = Angle / segments; // 각 세그먼트 사이의 각도 간격

        for (var i = 0; i <= segments; i++)
        {
            var radian = currentAngle * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Sin(radian) * Radius, 0, Mathf.Cos(radian) * Radius);
            currentAngle += deltaAngle;
        }

        for (var i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = DetectionMaterial;
    }

    public override List<Transform> CheckRange(string checkTag = "", int layerMask = -1)
    {
        var objTransform = RangeObject.transform;

        // 원의 중심과 방향을 계산합니다.
        var center = objTransform.position;
        var direction = objTransform.rotation;

        // 원 범위 내의 모든 콜라이더를 감지합니다.
        var collidersInCircle = Physics.OverlapSphere(center, Radius, layerMask);

        var targets = new List<Transform>();
        foreach (var collider in collidersInCircle)
        {
            if (checkTag == "" || collider.CompareTag(checkTag))
            {
                // 이 콜라이더의 위치가 원 범위 내에 있는지 확인합니다.
                var localPoint = objTransform.InverseTransformPoint(collider.transform.position);
                var distanceToCollider = Vector3.Distance(localPoint, Vector3.zero);
                if (distanceToCollider < Radius)
                {
                    targets.Add(collider.transform);
                }
            }
        }

        return targets;
    }
}