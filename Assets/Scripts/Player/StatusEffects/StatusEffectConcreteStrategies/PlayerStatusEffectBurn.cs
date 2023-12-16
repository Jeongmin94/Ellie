using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.StatusEffects.StatusEffectConcreteStrategies
{
    public class PlayerStatusEffectBurn : MonoBehaviour, IPlayerStatusEffect
    {
        private float duration;
        private PlayerStatus status;
        private GameObject burnEffectParticle;
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
            var burnEffectObj = Instantiate(burnEffectParticle, this.transform);
            controller.AddStatusEffect(this);
            float startTime = Time.time;
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
