using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthQuake", menuName = "Terrapupa/EarthQuake")]
public class TerrapupaEarthQuakeData : BaseBTData
{
    public TerrapupaEarthQuakeData()
    {
        dataName = "EarthQuake";
    }

    [Header("땅 뒤집기 공격")]
    [Tooltip("타겟팅 회전 속도")] public float earthQuakeRotationSpeed = 4.0f;
    [Tooltip("땅 뒤집기 시 이동 거리")] public float earthQuakeMoveDistance = 10.0f;
    [Tooltip("땅 뒤집기 시 이동 속도")] public float earthQuakeMovementSpeed = 8.0f;
    [Tooltip("땅 뒤집기 감지 각도")] public float earthQuakeAttackAngle = 45.0f;
    [Tooltip("땅 뒤집기 적중 거리")] public float earthQuakeEffectiveRadius = 50.0f;
    [Tooltip("땅 뒤집기 공격력")] public int earthQuakeAttackValue = 5;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<float> moveDistance;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<float> attackAngle;
    public BlackboardKey<float> effectiveRadius;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        Debug.Log(tree);

        SetBlackboardValue<float>("rotationSpeed", earthQuakeRotationSpeed, tree);
        SetBlackboardValue<float>("moveDistance", earthQuakeMoveDistance, tree);
        SetBlackboardValue<float>("movementSpeed", earthQuakeMovementSpeed, tree);
        SetBlackboardValue<float>("attackAngle", earthQuakeAttackAngle, tree);
        SetBlackboardValue<float>("effectiveRadius", earthQuakeEffectiveRadius, tree);
        SetBlackboardValue<int>("attackValue", earthQuakeAttackValue, tree);

        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        moveDistance = FindBlackboardKey<float>("moveDistance", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        attackAngle = FindBlackboardKey<float>("attackAngle", tree);
        effectiveRadius = FindBlackboardKey<float>("effectiveRadius", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}