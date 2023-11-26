using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthQuake", menuName = "Terrapupa/EarthQuake")]
public class TerrapupaEarthQuakeData : BaseBTData
{
    public TerrapupaEarthQuakeData()
    {
        dataName = "TerrapupaEarthQuake";
    }

    [Header("이펙트 설정")]
    [Tooltip("내려 찍기 이펙트")] public GameObject earthQuakeEffect1;

    [Header("땅 뒤집기 공격")]
    [Tooltip("공격 쿨타임")] public float earthQuakeCooldown = 10.0f;
    [Tooltip("타겟팅 회전 속도")] public float earthQuakeRotationSpeed = 4.0f;
    [Tooltip("공격 이동 거리")] public float earthQuakeMoveDistance = 10.0f;
    [Tooltip("점프 중 이동 속도")] public float earthQuakeMovementSpeed = 8.0f;
    [Tooltip("공격 범위 중심각")] public float earthQuakeAttackAngle = 45.0f;
    [Tooltip("공격 적중 거리")] public float earthQuakeEffectiveRadius = 50.0f;
    [Tooltip("공격력")] public int earthQuakeAttackValue = 5;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", earthQuakeEffect1, tree);
        SetBlackboardValue<float>("cooldown", earthQuakeCooldown, tree);
        SetBlackboardValue<float>("rotationSpeed", earthQuakeRotationSpeed, tree);
        SetBlackboardValue<float>("moveDistance", earthQuakeMoveDistance, tree);
        SetBlackboardValue<float>("movementSpeed", earthQuakeMovementSpeed, tree);
        SetBlackboardValue<float>("attackAngle", earthQuakeAttackAngle, tree);
        SetBlackboardValue<float>("effectiveRadius", earthQuakeEffectiveRadius, tree);
        SetBlackboardValue<int>("attackValue", earthQuakeAttackValue, tree);
    }
}