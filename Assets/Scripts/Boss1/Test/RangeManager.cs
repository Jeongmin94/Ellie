using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    None,
    Cone,
    Circle,
    Trapezoid,
    Rectangle,
    Hybrid,
}

public class RangePayload
{
    public RangeType Type { get; set; }
    // 부채꼴, 원
    public Material DetectionMeterial { get; set; }
    public float Radius { get; set; }
    public float Angle { get; set; }
    // 사각형(Width, Height), 사다리꼴(시작부분이 Upper)
    public float Height { get; set; }
    public float Width { get; set; }
    public float UpperBase { get; set; }
    public float LowerBase { get; set; }
}

public class RangeManager : Singleton<RangeManager>
{
    private Material detectionMaterial;

    private void Start()
    {
        detectionMaterial = new Material(Shader.Find("Standard"));
    }

    public GameObject CreateRange(Transform parent, RangePayload payload)
    {
        if (payload == null)
            return null;

        Material material = payload.DetectionMeterial ?? detectionMaterial;
        GameObject rangeObject = InitGameObject("Range", parent);

        if (payload.DetectionMeterial != null)
            material = payload.DetectionMeterial;

        switch (payload.Type)
        {
            case RangeType.Cone:
                CreateCone(rangeObject, payload.Radius, payload.Angle, material);
                break;
            case RangeType.Circle:
                CreateCone(rangeObject, payload.Radius, 360.0f, material);
                break;
            case RangeType.Trapezoid:
                CreateTrapezoid(rangeObject, payload.UpperBase, payload.LowerBase, payload.Height, material);
                break;
            case RangeType.Rectangle:
                CreateRectangle(rangeObject, payload.Width, payload.Height, material);
                break;
            case RangeType.Hybrid:
                CreateHybrid(rangeObject, payload.Radius, payload.Angle, payload.UpperBase, material);
                break;
        }
        return rangeObject;
    }

    private GameObject InitGameObject(string objName, Transform parent)
    {
        GameObject sectorObject = new GameObject(objName);
        sectorObject.transform.SetParent(parent, false); // 지정된 부모 오브젝트의 로컬 좌표를 따릅니다.
        sectorObject.transform.localPosition = new Vector3(0, 0.2f, 0); // 부모 오브젝트의 위치에 맞춥니다.
        sectorObject.transform.localRotation = Quaternion.identity; // 부모 오브젝트의 회전에 맞춥니다.

        return sectorObject;
    }

    private void CreateCone(GameObject target, float radius, float angle, Material detectionMaterial)
    {
        // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // 부채꼴의 중심각에 비례하여 세그먼트의 수를 결정합니다.
        int segments = Mathf.CeilToInt(angle); // 여기에서 세그먼트의 수를 적절하게 조절합니다.
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // 원의 중심점
        float currentAngle = -angle / 2; // 부채꼴의 시작 각도
        float deltaAngle = angle / segments; // 각 세그먼트 사이의 각도 간격

        for (int i = 0; i <= segments; i++)
        {
            float radian = currentAngle * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Sin(radian) * radius, 0, Mathf.Cos(radian) * radius);
            currentAngle += deltaAngle;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // 마지막 삼각형을 설정합니다.
        triangles[(segments - 1) * 3] = 0;
        triangles[(segments - 1) * 3 + 1] = segments;
        triangles[(segments - 1) * 3 + 2] = 1;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = detectionMaterial;
    }


    private void CreateRectangle(GameObject target, float width, float height, Material detectionMaterial)
    {
        // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // 사각형의 네 꼭지점을 정의합니다.
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-width / 2, 0, -height / 2); // 왼쪽 아래 꼭지점
        vertices[1] = new Vector3(-width / 2, 0, height / 2); // 왼쪽 위 꼭지점
        vertices[2] = new Vector3(width / 2, 0, -height / 2); // 오른쪽 아래 꼭지점
        vertices[3] = new Vector3(width / 2, 0, height / 2); // 오른쪽 위 꼭지점

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

        meshRenderer.material = detectionMaterial;
    }


    private void CreateTrapezoid(GameObject target, float upperBase, float lowerBase, float height, Material detectionMaterial)
    {
        // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // 사다리꼴의 네 꼭지점을 정의합니다.
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-upperBase / 2, 0, 0); // 왼쪽 아래 꼭지점
        vertices[1] = new Vector3(upperBase / 2, 0, 0); // 오른쪽 아래 꼭지점
        vertices[2] = new Vector3(-lowerBase / 2, 0, height); // 왼쪽 위 꼭지점
        vertices[3] = new Vector3(lowerBase / 2, 0, height); // 오른쪽 위 꼭지점

        // 두 개의 삼각형을 이어서 사다리꼴을 만듭니다.
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

        meshRenderer.material = detectionMaterial;
    }

    private void CreateHybrid(GameObject target, float radius, float angle, float upperBase, Material detectionMaterial)
    {
        // MeshFilter와 MeshRenderer 컴포넌트를 추가합니다.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // 부채꼴의 중심각에 비례하여 세그먼트의 수를 결정합니다.
        int segments = Mathf.CeilToInt(angle);
        Vector3[] vertices = new Vector3[segments + 3];
        int[] triangles = new int[(segments + 2) * 3];

        float currentAngle = -angle / 2;
        float deltaAngle = angle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float radian = currentAngle * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Sin(radian) * radius, 0, Mathf.Cos(radian) * radius);
            currentAngle += deltaAngle;
        }

        // 중심점에서 UpperBase만큼 떨어진 위치에 두 개의 꼭짓점을 추가합니다.
        vertices[0] = new Vector3(-upperBase / 2, 0, 0);
        vertices[segments + 2] = new Vector3(upperBase / 2, 0, 0);

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

        meshRenderer.material = detectionMaterial;
    }
}