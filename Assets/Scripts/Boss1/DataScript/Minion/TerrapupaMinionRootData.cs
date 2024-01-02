using Sirenix.OdinInspector;
using TheKiwiCoder;
using UnityEngine;

namespace Boss1.DataScript.Minion
{
    [CreateAssetMenu(fileName = "Root", menuName = "Terrapupa/MinionRoot")]
    public class TerrapupaMinionRootData : BaseBTData
    {
        [Header("기본 수치")] 
        [InfoBox("보스 이름")] public string bossName;
        [InfoBox("보스의 체력")] public int hp = 5;

        [Header("미니언 특성")] 
        [InfoBox("패턴 전환 시간")] public float transferTime = 2.0f;

        [Header("공격 감지 범위")]
        [InfoBox("감지 범위")] public float attackDetectionDistance = 4.0f;
        
        [Title("피격시 카메라 강도")] 
        [InfoBox("카메라 흔들림 강도")] public float cameraShakeIntensity = 0.05f;
        [InfoBox("카메라 흔들림 지속시간")] public float cameraShakeDuration = 0.05f;

        public BlackboardKey<bool> canAttack;
        public BlackboardKey<int> currentHP;
        public BlackboardKey<bool> isHit;

        public BlackboardKey<Transform> player;

        public TerrapupaMinionRootData()
        {
            dataName = "TerrapupaMinionRoot";
        }

        public override void Init(BehaviourTree tree)
        {
            SetBlackboardValue("currentHP", hp, tree);
            SetBlackboardValue("transferTime", transferTime, tree);
            SetBlackboardValue("attackDetectionDistance", attackDetectionDistance, tree);

            SetBlackboardValue("canAttack", true, tree);
            SetBlackboardValue("isHit", false, tree);

            currentHP = FindBlackboardKey<int>("currentHP", tree);
            player = FindBlackboardKey<Transform>("player", tree);
            canAttack = FindBlackboardKey<bool>("canAttack", tree);
            isHit = FindBlackboardKey<bool>("isHit", tree);
        }
    }
}
