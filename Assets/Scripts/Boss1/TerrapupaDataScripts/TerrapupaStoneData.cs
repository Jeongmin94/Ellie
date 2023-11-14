using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Stone", menuName = "Terrapupa/Stone")]
public class TerrapupaStoneData : BaseBTData
{
    public TerrapupaStoneData()
    {
        dataName = "TerrapupaStone";
    }

    [Header("돌 던지기 공격")]
    [Tooltip("타겟팅 회전 속도")] public float stoneRotationSpeed = 1.0f;
    [Tooltip("돌의 이동 속도")] public float stoneMovementSpeed = 15.0f;
    [Tooltip("돌의 공격력")] public int stoneAttackValue = 5;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("rotationSpeed", stoneRotationSpeed, tree);
        SetBlackboardValue<float>("movementSpeed", stoneMovementSpeed, tree);
        SetBlackboardValue<int>("attackValue", stoneAttackValue, tree);

        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}