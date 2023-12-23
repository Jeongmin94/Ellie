using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Trace", menuName = "Terrapupa/Trace")]
public class TerrapupaTraceData : BaseBTData
{
    [Title("사운드 설정")] [InfoBox("이동 사운드")] public string sound1 = "TerrapupaWalk";

    [Title("추적 상태")] [InfoBox("타겟팅 회전 속도")]
    public float traceRotationSpeed = 2.0f;

    [InfoBox("추적 이동속도")] public float traceMovementSpeed = 2.0f;
    public BlackboardKey<float> movementSpeed;

    public BlackboardKey<float> rotationSpeed;

    public TerrapupaTraceData()
    {
        dataName = "TerrapupaTrace";
    }

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue("sound1", sound1, tree);
        SetBlackboardValue("rotationSpeed", traceRotationSpeed, tree);
        SetBlackboardValue("movementSpeed", traceMovementSpeed, tree);

        rotationSpeed = FindBlackboardKey<float>("rotationSpeed", tree);
        movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
    }
}