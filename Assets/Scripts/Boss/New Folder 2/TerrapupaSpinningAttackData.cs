using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinningAttack", menuName = "Terrapupa/SpinningAttack")]
public class TerrapupaSpinningAttackData : BaseBTData
{
    public TerrapupaSpinningAttackData()
    {
        dataName = "SpinningAttack";
    }

    [Header("회전 공격")]
    [Tooltip("플레이어에게 타겟팅 회전 속도")] public float spinningAttackRotationSpeed = 2.0f;
    [Tooltip("회전 공격 중 이동 속도")] public float spinningAttackMovementSpeed = 1.0f;
    [Tooltip("회전 공격 공격력")] public int spinningAttackAttackValue = 5;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        Debug.Log(tree);

        SetBlackboardValue<float>("rotationSpeed", spinningAttackRotationSpeed, tree);
        SetBlackboardValue<float>("movementSpeed", spinningAttackMovementSpeed, tree);
        SetBlackboardValue<int>("attackValue", spinningAttackAttackValue, tree);

        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}