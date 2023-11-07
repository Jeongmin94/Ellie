using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.StatusEffects;

namespace Assets.Scripts.Monsters.EffectStatus
{
    public class StatusEffectPlainHit : MonoBehaviour, IMonsterStatusEffect
    {
        MonsterController monsterController;
        public void ApplyStatusEffect(MonsterEffectStatusController controller)
        {
            monsterController = controller.gameObject.GetComponent<MonsterController>();
            MonsterStatusPlainHit(controller);
        }

        private void MonsterStatusPlainHit(MonsterEffectStatusController controller)
        {
            controller.AddStatusEffect(this);
            monsterController.ChangeEffectState(StatusEffectName.None);
        }
    }
}