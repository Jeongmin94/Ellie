using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Roll", menuName = "Terrapupa/Roll")]
public class TerrapupaRollData : BaseBTData
{
    public TerrapupaRollData()
    {
        dataName = "TerrapupaRoll";
    }

    [Header("구르기 공격")]
    [Tooltip("타겟팅 회전 속도")] public float rollRotationSpeed = 5.0f;
    [Tooltip("구르기 종료 벽 인식 거리(Raycast 길이)")] public float rollRayCastLength = 7.0f;
    [Tooltip("구르기 이동속도")] public float rollMovementSpeed = 20.0f;
    [Tooltip("구르기 가속도")] public float rollAcceleration = 0.3f;
    [Tooltip("구르기 감속도")] public float rollDeceleration = 0.3f;
    [Tooltip("구르기 공격력")] public int rollAttackValue = 5;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<float> rayCastLength;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<float> acceleration;
    public BlackboardKey<float> dcceleration;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("rotationSpeed", rollRotationSpeed, tree);
        SetBlackboardValue<float>("rayCastLength", rollRayCastLength, tree);
        SetBlackboardValue<float>("movementSpeed", rollMovementSpeed, tree);
        SetBlackboardValue<float>("acceleration", rollAcceleration, tree);
        SetBlackboardValue<float>("dcceleration", rollDeceleration, tree);
        SetBlackboardValue<int>("attackValue", rollAttackValue, tree);

        rayCastLength = FindBlackboardKey<float>("rotationSpeed", tree);
        rayCastLength = FindBlackboardKey<float>("rayCastLength", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        acceleration = FindBlackboardKey<float>("acceleration", tree);
        dcceleration = FindBlackboardKey<float>("dcceleration", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}