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
    [Tooltip("구르기 종료 벽 인식 거리(Raycast 길이)")] public float rollRayCastLength = 7.0f;
    [Tooltip("구르기 이동속도")] public float rollMovementSpeed = 20.0f;
    [Tooltip("구르기 공격력")] public int rollAttackValue = 5;

    public BlackboardKey<float> rayCastLength;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("rayCastLength", rollRayCastLength, tree);
        SetBlackboardValue<float>("movementSpeed", rollMovementSpeed, tree);
        SetBlackboardValue<int>("attackValue", rollAttackValue, tree);

        rayCastLength = FindBlackboardKey<float>("rayCastLength", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}