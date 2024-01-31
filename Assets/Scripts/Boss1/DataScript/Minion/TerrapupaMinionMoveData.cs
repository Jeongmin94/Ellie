using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Minion
{
    [CreateAssetMenu(fileName = "MinionMove", menuName = "Terrapupa/MinionMove")]
    public class TerrapupaMinionMoveData : BehaviourTreeData
    {
        [Header("공격 설정")] [Tooltip("돌진 점프 높이")]
        public float rotationSpeed = 2.0f;

        [Tooltip("돌진 속도")] public float movementSpeed = 2.0f;

        public TerrapupaMinionMoveData()
        {
            dataName = "TerrapupaMinionMove";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("rotationSpeed", rotationSpeed, tree);
            SetBlackboardValue("movementSpeed", movementSpeed, tree);
        }
    }
}