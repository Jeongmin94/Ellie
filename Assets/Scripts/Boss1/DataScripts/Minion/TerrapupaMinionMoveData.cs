using Assets.Scripts.StatusEffects;
using Channels.Combat;
using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionMove", menuName = "Terrapupa/MinionMove")]
public class TerrapupaMinionMoveData : BaseBTData
{
    public TerrapupaMinionMoveData()
    {
        dataName = "TerrapupaMinionMove";
    }

    [Header("공격 설정")]
    [Tooltip("돌진 점프 높이")] public float rotationSpeed = 2.0f;
    [Tooltip("돌진 속도")] public float movementSpeed = 2.0f;

    public override void Init(BehaviourTree tree)
    {
        SetBlackboardValue<float>("rotationSpeed", rotationSpeed, tree);
        SetBlackboardValue<float>("movementSpeed", movementSpeed, tree);
    }
}