using Sirenix.OdinInspector;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Tempt", menuName = "Terrapupa/Tempt")]
public class TerrapupaTemptData : BaseBTData
{
    public TerrapupaTemptData()
    {
        dataName = "TerrapupaTempt";
    }

    [Title("유인 상태")]
    [InfoBox("유인 이동속도")] public float temptMovementSpeed = 2.0f;
    [InfoBox("섭취상태로 변경 시 감지 범위")] public float temptStateChangeDetectionDistance = 1.0f;

    public BlackboardKey<float> movementSpeed;
    public BlackboardKey<float> stateChangeDetectionDistance;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("movementSpeed", temptMovementSpeed, tree);
        SetBlackboardValue<float>("stateChangeDetectionDistance", temptStateChangeDetectionDistance, tree);

        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
        stateChangeDetectionDistance = FindBlackboardKey<float>("stateChangeDetectionDistance", tree);
    }
}