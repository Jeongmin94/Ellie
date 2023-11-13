using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinningAttack", menuName = "Terrapupa/SpinningAttack")]
public class TerrapupaSpinningAttackData : BaseBTData
{
    public TerrapupaSpinningAttackData()
    {
        dataName = "TerrapupaSpinningAttack";
    }

    [Header("이펙트 설정")]
    [Tooltip("히전 공격 시 이펙트")] public GameObject spinningAttackEffect1;

    [Header("회전 공격")]
    [Tooltip("플레이어에게 타겟팅 회전 속도")] public float spinningAttackRotationSpeed = 2.0f;
    [Tooltip("회전 공격 중 이동 속도")] public float spinningAttackMovementSpeed = 1.0f;
    [Tooltip("회전 공격 공격 적중 거리")] public float spinningAttackEffectiveRadius = 7.0f;
    [Tooltip("회전 공격 공격력")] public int spinningAttackAttackValue = 5;

    public BlackboardKey<GameObject> effect1;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<float> effectiveRadius;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", spinningAttackEffect1, tree);
        SetBlackboardValue<float>("rotationSpeed", spinningAttackRotationSpeed, tree);
        SetBlackboardValue<float>("movementSpeed", spinningAttackMovementSpeed, tree);
        SetBlackboardValue<float>("effectiveRadius", spinningAttackEffectiveRadius, tree);
        SetBlackboardValue<int>("attackValue", spinningAttackAttackValue, tree);

        effect1 = FindBlackboardKey<GameObject>("effect1", tree);
        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        effectiveRadius = FindBlackboardKey<float>("effectiveRadius", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}