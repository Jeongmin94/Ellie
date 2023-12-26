using Sirenix.OdinInspector;
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

    [Title("사운드 설정")]
    [InfoBox("섭취 완료 사운드")] public string sound1 = "TerrapupaEatMagicStoneSuccess";

    [Title("이펙트")]
    [InfoBox("섭취 완료 이펙트")] public GameObject effect1;
    
    [Title("섭취 상태")]
    [InfoBox("섭취 지속시간")] public float intakeDuration = 5.0f;
    [InfoBox("섭취 시 체력 회복량")] public int intakeHealValue = 10;

    public BlackboardKey<float> duration;
    public BlackboardKey<int> healValue;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<string>("sound1", sound1, tree);
        SetBlackboardValue<GameObject>("effect1", effect1, tree);
        SetBlackboardValue<float>("duration", intakeDuration, tree);
        SetBlackboardValue<int>("healValue", intakeHealValue, tree);

        duration = FindBlackboardKey<float>("duration", tree);
        healValue = FindBlackboardKey<int>("healValue", tree);
    }
}