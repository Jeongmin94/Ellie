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

    [Header("이펙트 설정")]
    [Tooltip("돌진 시작 시 이펙트")] public GameObject rollEffect1;
    [Tooltip("돌진 중 충돌 시 이펙트")] public GameObject rollEffect2;

    [Header("구르기 공격")]
    [Tooltip("타겟팅 회전 속도")] public float rollRotationSpeed = 5.0f;
    [Tooltip("구르기 종료 벽 인식 거리(Raycast 길이)")] public float rollRayCastLength = 7.0f;
    [Tooltip("구르기 이동속도")] public float rollMovementSpeed = 20.0f;
    [Tooltip("구르기 가속도")] public float rollAcceleration = 0.3f;
    [Tooltip("구르기 감속도")] public float rollDeceleration = 0.3f;
    [Tooltip("구르기 공격력")] public int rollAttackValue = 5;

    public BlackboardKey<GameObject> effect1;
    public BlackboardKey<GameObject> effect2;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<float> rayCastLength;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<float> acceleration;
    public BlackboardKey<float> dcceleration;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", rollEffect1, tree);
        SetBlackboardValue<GameObject>("effect2", rollEffect2, tree);
        SetBlackboardValue<float>("rotationSpeed", rollRotationSpeed, tree);
        SetBlackboardValue<float>("rayCastLength", rollRayCastLength, tree);
        SetBlackboardValue<float>("movementSpeed", rollMovementSpeed, tree);
        SetBlackboardValue<float>("acceleration", rollAcceleration, tree);
        SetBlackboardValue<float>("dcceleration", rollDeceleration, tree);
        SetBlackboardValue<int>("attackValue", rollAttackValue, tree);

        effect1 = FindBlackboardKey<GameObject>("effect1", tree);
        effect2 = FindBlackboardKey<GameObject>("effect2", tree);
        rayCastLength = FindBlackboardKey<float>("rotationSpeed", tree);
        rayCastLength = FindBlackboardKey<float>("rayCastLength", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        acceleration = FindBlackboardKey<float>("acceleration", tree);
        dcceleration = FindBlackboardKey<float>("dcceleration", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}