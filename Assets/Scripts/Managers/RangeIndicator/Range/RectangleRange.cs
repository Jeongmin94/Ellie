using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;


public class RectangleRange : BaseRange
{
    public float Width { get; private set; }
    public float Height { get; private set; }

    public override void Init(GameObject rangeObject, RangePayload payload)
    {
        base.Init(rangeObject, payload);

        RangeObject.name = "RectangleRange";
    }

    public override void CreateRange(RangePayload payload)
    {
        Height = payload.Height;
        Width = payload.Width;

        DetectionMaterial = payload.DetectionMaterial;

        // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
        MeshFilter meshFilter = RangeObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = RangeObject.AddComponent<MeshRenderer>();

        if (!IsShowRange)
        {
            return;
        }

        Mesh mesh = new Mesh();

        // 사각형의 네 꼭지점을 정의합니다.
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-Width / 2, 0, -Height / 2); // 왼쪽 아래 꼭지점
        vertices[1] = new Vector3(-Width / 2, 0, Height / 2); // 왼쪽 위 꼭지점
        vertices[2] = new Vector3(Width / 2, 0, -Height / 2); // 오른쪽 아래 꼭지점
        vertices[3] = new Vector3(Width / 2, 0, Height / 2); // 오른쪽 위 꼭지점

        // 두 개의 삼각형을 이어서 사각형을 만듭니다.
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = DetectionMaterial;
    }

    public override List<Transform> CheckRange(string checkTag = "", int layerMask = -1)
    {
        Transform objTransform = RangeObject.transform;

        // 사각형의 중심과 방향을 계산합니다.
        Vector3 center = objTransform.position;
        Quaternion direction = objTransform.rotation;

        // 사각형 범위 내의 모든 콜라이더를 감지합니다.
        Collider[] collidersInRectangle = Physics.OverlapBox(center, new Vector3(Width / 2, 1, Height / 2), direction, layerMask);

        // 이 콜라이더들 중에서 "적" 태그를 가진 것들만 선택합니다.
        List<Transform> targets = new List<Transform>();
        foreach (Collider collider in collidersInRectangle)
        {
            if (checkTag == "" || collider.CompareTag(checkTag))
            {
                // 이 콜라이더의 위치를 사각형의 로컬 좌표계로 변환합니다.
                Vector3 localColliderPosition = objTransform.InverseTransformPoint(collider.transform.position);

                // 이 콜라이더의 로컬 위치가 사각형 범위 내에 있는지 확인합니다.
                if (Mathf.Abs(localColliderPosition.x) < Width / 2 && Mathf.Abs(localColliderPosition.z) < Height / 2)
                {
                    targets.Add(collider.transform);
                }
            }
        }

        return targets;
    }
}
