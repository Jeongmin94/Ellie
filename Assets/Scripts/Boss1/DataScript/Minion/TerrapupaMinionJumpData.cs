using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionJump", menuName = "Terrapupa/MinionJump")]
public class TerrapupaMinionJumpData : BaseBTData
{
    [Header("공격 설정")] [Tooltip("돌진 점프 높이")]
    public float jumpPower = 4.0f;

    [Tooltip("돌진 속도")] public float rushSpeed = 4.0f;

    public TerrapupaMinionJumpData()
    {
        dataName = "TerrapupaMinionJump";
    }

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue("jumpPower", jumpPower, tree);
        SetBlackboardValue("rushSpeed", rushSpeed, tree);
    }
}