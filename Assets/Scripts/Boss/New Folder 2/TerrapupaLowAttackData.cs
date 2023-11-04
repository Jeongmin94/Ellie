using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "LowAttack", menuName = "Terrapupa/LowAttack")]
public class TerrapupaLowAttackData : BaseBTData
{
    public TerrapupaLowAttackData()
    {
        dataName = "LowAttack";
    }

    [Header("하단 공격")]
    [Tooltip("타겟팅 회전 속도")] public float lowAttackRotationSpeed = 4.0f;
    [Tooltip("하단 공격 공격력")] public int lowAttackAttackValue = 5;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        Debug.Log(tree);

        SetBlackboardValue<float>("rotationSpeed", lowAttackRotationSpeed, tree);
        SetBlackboardValue<int>("attackValue", lowAttackAttackValue, tree);

        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}