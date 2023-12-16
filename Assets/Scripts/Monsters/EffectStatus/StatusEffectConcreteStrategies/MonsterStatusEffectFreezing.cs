using System.Collections;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Monsters.EffectStatus.StatusEffectConcreteStrategies
{
    public class MonsterStatusEffectFreezing : MonoBehaviour, IMonsterStatusEffect
    {
        public float duration;

        private MonsterStatus status;

        public void ApplyStatusEffect(MonsterEffectStatusController controller, MonsterStatusEffectInfo info)
        {
            status = controller.GetComponent<MonsterStatus>();
            duration = info.EffectDuration;
            StartCoroutine(Freezing(controller));
        }

        private IEnumerator Freezing(MonsterEffectStatusController controller)
        {
            // !TODO : 몬스터 빙결 처리
            controller.AddStatusEffect(this);
            StartFreeze();

            yield return new WaitForSeconds(5.0f);

            controller.RemoveStatusEffect(this);
            EndFreeze();
        }

        private void StartFreeze()
        {
            Debug.Log($"{transform.name} 빙결 시작!!");

            var behaviour = GetComponent<BehaviourTreeInstance>();
            behaviour.FindBlackboardKey<bool>("IsFreezing").Value = true;
        }

        private void EndFreeze()
        {
            Debug.Log($"{transform.name} 빙결 해제!!");

            var behaviour = GetComponent<BehaviourTreeInstance>();
            behaviour.FindBlackboardKey<bool>("IsFreezing").Value = false;
        }
    }
}