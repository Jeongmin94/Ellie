using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "LowAttack", menuName = "Terrapupa/LowAttack")]
public class TerrapupaLowAttackData : BaseBTData
{
    public TerrapupaLowAttackData()
    {
        dataName = "TerrapupaLowAttack";
    }

    [Header("이펙트 설정")]
    [Tooltip("하단 공격 이펙트")] public GameObject lowAttackEffect1;

    [Header("하단 공격")]
    [Tooltip("타겟팅 회전 속도")] public float lowAttackRotationSpeed = 4.0f;
    [Tooltip("하단 공격 공격력")] public int lowAttackAttackValue = 5;

    public BlackboardKey<GameObject> effect1;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<int> attackValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<GameObject>("effect1", lowAttackEffect1, tree);
        SetBlackboardValue<float>("rotationSpeed", lowAttackRotationSpeed, tree);
        SetBlackboardValue<int>("attackValue", lowAttackAttackValue, tree);

        effect1 = FindBlackboardKey<GameObject>("effect1", tree);
        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        attackValue = FindBlackboardKey<int>("attackValue", tree);
    }
}