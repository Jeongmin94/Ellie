﻿using Assets.Scripts.Combat;
using Assets.Scripts.Item.Stone;
using Channels.Combat;
using Channels.Components;
using Sirenix.OdinInspector;

namespace Assets.Scripts.Puzzle
{
    public class BreakableStone : SerializedMonoBehaviour, ICombatant
    {
        public int currentHP;
        public float stoneShakeTime;

        private TicketMachine ticketMachine;

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        public void Attack(IBaseEventPayload payload)
        {
            
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            CombatPayload combatPayload = payload as CombatPayload;

            var attackStone = combatPayload.Attacker;

            if (attackStone.GetComponent<ExplosionStone>() != null )
            {
                DestroyStone();
            }
            else
            {
                HitByStone(combatPayload.Damage);
            }
        }

        private void HitByStone(int damageValue)
        {
            // 돌맹이 피격 흔들림 처리

            // 데미지 처리
            GetDamaged(damageValue);
        }

        private void GetDamaged(int damageValue)
        {
            currentHP -= damageValue;
            if (currentHP <= 0)
            {
                // 돌 삭제
                DestroyStone();
            }
        }

        private void DestroyStone()
        {
            Destroy(this.gameObject);
        }
    }
}