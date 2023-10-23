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
        [SerializeField] private BehaviourTree currentTree;

        [Tooltip("테라푸파의 데이터 테이블")]
        [SerializeField] private TerrapupaDataInfo data;


    }
}