using Assets.Scripts.Combat;
using Assets.Scripts.StatusEffects;
using UnityEngine;

namespace Channels.Combat
{
    public enum CombatType
    {
        Melee,
        Weapon,
        Projectile,
        Movement,
        RequestStone
    }


    public class CombatPayload : IBaseEventPayload
    {
        //페이로드 타입
        public CombatType Type { get; set; }


        //공격자의 transform
        public Transform Attacker { get; set; }

        //피격자의 transform
        public Transform Defender { get; set; }

        //공격자의 데미지
        public int Damage { get; set; }

        public Vector3 StoneSpawnPos { get; set; }
        public Vector3 StoneDirection { get; set; }
        public float StoneStrength { get; set; }
        //공격 이벤트 발행시의 공격 방향
        public Vector3 AttackDirection { get; set; }

        //공격 이벤트 발행시의 공격의 위치
        public Vector3 AttackPosition { get; set; }

        //공격자의 공격 시작 위치
        public Vector3 AttackStartPosition { get; set; }

        //공격이 플레이어에게 피격됐을 때 플레이어에게 유발되는 상태이상
        public PlayerStatusEffectName PlayerStatusEffectName { get; set; }
        //!TODO : ������ enemy�� �ǰݵ��� �� ���ߵǴ� �����̻��� enum�� �ʿ��մϴ�
    }

    public class CombatChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            CombatPayload combatPayload = payload as CombatPayload;
            //돌달라는 페이로드임?->
            if(combatPayload.Type == CombatType.RequestStone)
            {
                notifyAction?.Invoke(combatPayload);
                return;
            }
            ICombatant combatant = combatPayload.Defender.GetComponent<ICombatant>();
            combatant?.ReceiveDamage(CalculateCombatLogic(combatPayload));
        }

        private CombatPayload CalculateCombatLogic(CombatPayload payload)
        {
            CombatPayload newPayload = payload;
            // !TODO : �����ڿ� ������� transform�� ���ͼ� ���� ������ ������ ��, Payload�� �ٽ� �����

            return newPayload;
        }
    }
}