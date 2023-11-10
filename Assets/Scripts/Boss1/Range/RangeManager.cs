using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    None,
    Cone,       // 부채꼴
    Circle,     // 원
    Trapezoid,  // 사다리꼴
    Rectangle,  // 사각형
    HybridCone, // 부채꼴 + 사다리꼴
}

public class RangePayload
{
    public RangeType Type { get; set; }
    public Material DetectionMaterial { get; set; }
    // 부채꼴, 원
    public float Radius { get; set; }
    public float Angle { get; set; }
    // 사각형(Width - 직선 범위, Height - 폭)
    public float Height { get; set; }
    public float Width { get; set; }
    // 부채꼴(UpperBase - 시작, LowerBase - 끝)
    public float UpperBase { get; set; }
    public float LowerBase { get; set; }
}

public class RangeManager : Singleton<RangeManager>
{
    [SerializeField] private List<GameObject> ranges;

    public GameObject CreateRange(Transform parent, RangePayload payload, bool isSetParent = true)
    {
        if (payload == null)
            return null;

        GameObject obj = new GameObject();
        BaseRange range = null;

        switch (payload.Type)
        {
            case RangeType.Cone:
                range = obj.AddComponent<ConeRange>();
                break;
            case RangeType.Circle:
                range = obj.AddComponent<CircleRange>();
                break;
            case RangeType.Trapezoid:
                range = obj.AddComponent<TrapezoidRange>();
                break;
            case RangeType.Rectangle:
                range = obj.AddComponent<RectangleRange>();
                break;
            case RangeType.HybridCone:
                range = obj.AddComponent<HybridConeRange>();
                break;
        }

        if (range != null)
        {
            range.Init(obj, parent, isSetParent);
            range.CreateRange(payload);

            return obj;
        }

        return null;
    }
}