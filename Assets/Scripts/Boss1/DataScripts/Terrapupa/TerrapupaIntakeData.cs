using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Intake", menuName = "Terrapupa/Intake")]
public class TerrapupaIntakeData : BaseBTData
{
    public TerrapupaIntakeData()
    {
        dataName = "TerrapupaIntake";
    }

    [Header("섭취 상태")]
    [Tooltip("섭취 지속시간")] public float intakeDuration = 5.0f;
    [Tooltip("섭취 시 체력 회복량")] public int intakeHealValue = 10;

    public BlackboardKey<float> duration;
    public BlackboardKey<int> healValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("duration", intakeDuration, tree);
        SetBlackboardValue<int>("healValue", intakeHealValue, tree);

        duration = FindBlackboardKey<float>("duration", tree);
        healValue = FindBlackboardKey<int>("healValue", tree);
    }
}