using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Trace", menuName = "Terrapupa/Trace")]
public class TerrapupaTraceData : BaseBTData
{
    public TerrapupaTraceData()
    {
        dataName = "TerrapupaTrace";
    }

    [Header("추적 상태")]
    [Tooltip("타겟팅 회전 속도")] public float traceRotationSpeed = 2.0f;
    [Tooltip("추적 이동속도")] public float traceMovementSpeed = 2.0f;

    public BlackboardKey<float> rotationSpeed;
    public BlackboardKey<float> movementSpeed;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("rotationSpeed", traceRotationSpeed, tree);
        SetBlackboardValue<float>("movementSpeed", traceMovementSpeed, tree);

        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
    }
}