using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;
public enum RangeType
{
    None,
    Cone,       // 부채꼴
    Circle,     // 원
    Trapezoid,  // 사다리꼴
    Rectangle,  // 사각형
    HybridCone, // 부채꼴 + 사다리꼴
}

public class RangePayload : IBaseEventPayload
{
    // 기본 설정
    public RangeType Type { get; set; }
    public Transform Original { get; set; }
    public Material DetectionMaterial { get; set; }
    public bool IsFollowOrigin { get; set; } = false;
    public bool IsShowRange { get; set; } = true;
    public Vector3 StartPosition { get; set; }
    public Quaternion StartRotation { get; set; }

    // 지속시간, 페이드 인, 페이드 아웃
    public float RemainTime { get; set; } = 0.0f; // 0.0f이라면 무한 지속
    public float FadeInTime { get; set; } = 0.3f;
    public float FadeOutTime { get; set; } = 0.3f;

    // 부채꼴, 원
    public float Radius { get; set; }
    public float Angle { get; set; }
    // 사각형(Width - 직선 범위, Height - 폭)
    public float Height { get; set; }
    public float Width { get; set; }
    // 부채꼴(UpperBase - 시작, LowerBase - 끝) + Height
    public float UpperBase { get; set; }
    public float LowerBase { get; set; }
}

public class RangeManager : Singleton<RangeManager>
{
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material invisibleMaterial;

    public override void Awake()
    {
        base.Awake();

        baseMaterial = Resources.Load<Material>("Materials/Sensor");
        invisibleMaterial = Resources.Load<Material>("Materials/Sensor2");
    }

    public GameObject CreateRange(RangePayload payload)
    {
        if (payload == null)
            return null;

        if (payload.DetectionMaterial == null && baseMaterial != null)
            payload.DetectionMaterial = baseMaterial;

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
            range.Init(obj, payload);
            range.CreateRange(payload);
            range.StartFadeInAndOut(payload);
            range.gameObject.GetOrAddComponent<VisibilityControl>();

            return obj;
        }

        return null;
    }


}