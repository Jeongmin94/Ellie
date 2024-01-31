using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Terrapupa
{
    [CreateAssetMenu(fileName = "Tempt", menuName = "Terrapupa/Tempt")]
    public class TerrapupaTemptData : BehaviourTreeData
    {
        [Title("유인 상태")] [InfoBox("유인 이동속도")] public float temptMovementSpeed = 2.0f;

        [InfoBox("섭취상태로 변경 시 감지 범위")] public float temptStateChangeDetectionDistance = 1.0f;

        public BlackboardKey<float> movementSpeed;
        public BlackboardKey<float> stateChangeDetectionDistance;

        public TerrapupaTemptData()
        {
            dataName = "TerrapupaTempt";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("movementSpeed", temptMovementSpeed, tree);
            SetBlackboardValue("stateChangeDetectionDistance", temptStateChangeDetectionDistance, tree);

            movementSpeed = FindBlackboardKey<float>("movementSpeed", tree);
            stateChangeDetectionDistance = FindBlackboardKey<float>("stateChangeDetectionDistance", tree);
        }
    }
}