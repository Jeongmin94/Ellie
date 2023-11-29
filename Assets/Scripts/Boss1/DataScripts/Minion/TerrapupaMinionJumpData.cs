using Assets.Scripts.StatusEffects;
using Channels.Combat;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionJump", menuName = "Terrapupa/MinionJump")]
public class TerrapupaMinionJumpData : BaseBTData
{
    public TerrapupaMinionJumpData()
    {
        dataName = "TerrapupaMinionJump";
    }

    [Header("공격 설정")]
    [Tooltip("돌진 점프 높이")] public float jumpPower = 4.0f;
    [Tooltip("돌진 속도")] public float rushSpeed = 4.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("jumpPower", jumpPower, tree);
        SetBlackboardValue<float>("rushSpeed", rushSpeed, tree);
    }
}