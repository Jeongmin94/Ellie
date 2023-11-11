using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monsters.EffectStatus
{
    public interface IMonsterStatusEffect
    {
        public void ApplyStatusEffect(MonsterEffectStatusController controller);
    }
}
