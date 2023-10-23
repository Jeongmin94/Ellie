using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    public class TerrapupaController : MonoBehaviour
    {
        [Tooltip("테라푸파의 AI 트리 목록 관리")]
        public List<BehaviourTree> treeList = new List<BehaviourTree>();

        [Tooltip("테라푸파의 현재 트리 상태")]
        [SerializeField] private BehaviourTreeInstance behaviourTreeInstance;

        [Tooltip("테라푸파의 데이터 테이블")]
        [SerializeField] private TerrapupaDataInfo data;

        public Transform playerTemp;

        public BlackboardKey<Vector3> playerPos;

        private void Start()
        {
            behaviourTreeInstance.SetBlackboardValue<Vector3>("targetPosition", playerTemp.position);
            playerPos = behaviourTreeInstance.FindBlackboardKey<Vector3>("targetPosition");
        }

        private void Update()
        {
            playerPos.value = playerTemp.position;
        }
    }
}