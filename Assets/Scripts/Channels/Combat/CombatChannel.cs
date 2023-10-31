using Assets.Scripts.Combat;
using Assets.Scripts.StatusEffects;
using Codice.CM.SEIDInfo;
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
        //���� Ÿ��
        public CombatType Type { get; set; }
        //�������� transform
        public Transform Attacker { get; set; }
        //�ǰ����� transform
        public Transform Defender { get; set; }
        //�������� ������
        public int Damage { get; set; }
        public Vector3 StoneSpawnPos { get; set; }
        public Vector3 StoneDirection { get; set; }
        public float StoneStrength { get; set; }
        //���� �̺�Ʈ ������� ���� ����
        public Vector3 AttackDirection { get; set; }
        //���� �̺�Ʈ ������� ������ ��ġ
        public Vector3 AttackPosition { get; set; }
        //�������� ���� ���� ��ġ
        public Vector3 AttackStartPosition { get; set; }
        //������ �÷��̾�� �ǰݵ��� �� �÷��̾�� ���ߵǴ� �����̻�
        public PlayerStatusEffectName PlayerStatusEffectName { get; set; }
        //!TODO : ������ enemy�� �ǰݵ��� �� ���ߵǴ� �����̻��� enum�� �ʿ��մϴ�
    }

    public class CombatChannel : BaseEventChannel
    {
        public override void ReceiveMessage<T>(T payload)
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