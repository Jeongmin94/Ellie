using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Roll", menuName = "GameData List/OreData")]
public class TerrapupaRollData : BaseBTData
{
    public TerrapupaRollData()
    {
        dataName = "TerrapupaRollData";
    }

    [Header("구르기 공격")]
    [Tooltip("패턴 사용 여부")] public bool rollUsable = true;
    [Tooltip("공격 감지 범위")] public float rollDetectionDistance = 25.0f;
    [Tooltip("구르기 종료 벽 인식 거리(Raycast 길이)")] public float rollRayCastLength = 7.0f;
    [Tooltip("구르기 이동속도")] public float rollMovementSpeed = 20.0f;
    [Tooltip("구르기 공격력")] public int rollAttackValue = 5;

    public BlackboardKey<float> detectionDistance;
    public BlackboardKey<float> rayCastLength;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        Debug.Log(tree);

        SetBlackboardValue<float>("detectionDistance", rollDetectionDistance, tree);
        SetBlackboardValue<float>("rayCastLength", rollRayCastLength, tree);
        SetBlackboardValue<float>("movementSpeed", rollMovementSpeed, tree);
        SetBlackboardValue<int>("attackValue", rollAttackValue, tree);

        detectionDistance = FindBlackboardKey<float>("detectionDistance", tree);
        rayCastLength = FindBlackboardKey<float>("rayCastLength", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}