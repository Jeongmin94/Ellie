using System.Collections;
using UnityEngine;

namespace Player.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectBurn : MonoBehaviour, IPlayerStatusEffect
    {
        private GameObject burnEffectParticle;
        private float duration;
        private PlayerStatus status;

        public void InitStatusEffect()
        {
            burnEffectParticle = Resources.Load<GameObject>("Prefabs/StatusEffectParticles/BurnEffect");
        }

        public void ApplyStatusEffect(PlayerStatusEffectController controller, StatusEffectInfo info)
        {
            status = controller.GetComponent<PlayerStatus>();
            duration = info.effectDuration;
            StartCoroutine(Burn(controller));
        }

        private IEnumerator Burn(PlayerStatusEffectController controller)
        {
            var burnEffectObj = Instantiate(burnEffectParticle, transform);
            controller.AddStatusEffect(this);
            var startTime = Time.time;
            //화상 로직
            while (Time.time - startTime < duration)
            {
                yield return new WaitForSeconds(1.0f);
                status.ReduceHP(1);
            }

            controller.RemoveStatusEffect(this);
            Destroy(burnEffectObj);
        }
    }
}