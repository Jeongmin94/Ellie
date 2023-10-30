using Assets.Scripts.Combat;
using Assets.Scripts.StatusEffects;
using Codice.CM.SEIDInfo;
using UnityEngine;

namespace Channels.Combat
{
    public enum CombatType
    {
        Melee,
    }

    public class CombatPayload : IBaseEventPayload
    {
        //공격 타입
        public CombatType Type { get; set; }
        //공격자의 transform
        public Transform Attacker { get; set; }
        //피격자의 transform
        public Transform Defender { get; set; }
        //공격자의 데미지
        public int Damage { get; set; }
        //공격 이벤트 발행시의 공격 방향
        public Vector3 AttackDirection { get; set; }
        //공격 이벤트 발행시의 공격의 위치
        public Vector3 AttackPosition { get; set; }
        //공격자의 공격 시작 위치
        public Vector3 AttackStartPosition { get; set; }
        //공격이 플레이어에게 피격됐을 때 플레이어에게 유발되는 상태이상
        public PlayerStatusEffectName PlayerStatusEffectName { get; set; }
        //!TODO : 공격이 enemy에 피격됐을 시 유발되는 상태이상의 enum이 필요합니다
    }

    public class CombatChannel : BaseEventChannel
    {
        public override void ReceiveMessage<T>(T payload)
        {
            CombatPayload combatPayload = payload as CombatPayload;
            ICombatant combatant = combatPayload.Defender.GetComponent<ICombatant>();
            combatant?.ReceiveDamage(CalculateCombatLogic(combatPayload));

        }

        private CombatPayload CalculateCombatLogic(CombatPayload payload)
        {
            CombatPayload newPayload = payload;
            // !TODO : 공격자와 방어자의 transform을 따와서 전투 로직을 실행한 후, Payload를 다시 만들기

            return newPayload;
        }
    }
}