using Assets.Scripts.Combat;
using Assets.Scripts.Particle;
using Channels.Combat;
using Channels.Type;
using System;
using UnityEngine;
namespace Assets.Scripts.Item.Stone
{
    public class StonePrefab : BaseStone, ICombatant
    {
        public BaseStoneEffect StoneEffect { get; set; }

        public void Attack(IBaseEventPayload payload)
        {

        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {

        }

        public void OccurEffect(Transform defender)
        {
            Debug.Log($"{defender} 돌 충돌");
            if (gameObject.activeSelf)
            {
                hatchery.Attack(GenerateStonePayload(defender));
            }
        }

        private CombatPayload GenerateStonePayload(Transform defender)
        {
            var payload = new CombatPayload();
            // !TODO : 돌맹이 데이터 읽어와서 현재 돌맹이에 맞는 값으로 페이로드 초기화
            
            //=> test
            payload.Type = CombatType.Projectile;
            payload.Attacker = transform;
            payload.Defender = defender;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = defender.position;
            payload.Damage = data.damage;
            payload.StatusEffectName = data.statusEffect;
            payload.statusEffectduration = data.statusEffectDuration;
            payload.force = data.force;
            //<=
            return payload;
        }
        
    }
}
