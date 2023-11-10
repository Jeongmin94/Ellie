using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.AbstractClass;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Assets.Scripts.Utils;
using UnityEngine;
using Assets.Scripts.Player;

namespace Assets.Scripts.Monsters.Attacks
{
    public class BoxColliderAttack : AbstractAttack
    {
        private BoxCollider collider;
        private PlayerStatus playerStatus;

        private TicketMachine ticketMachine;
        private CombatPayload payload=new();

        private BoxColliderAttackData attackData;

        public override void InitializeBoxCollider(BoxColliderAttackData data)
        {
            attackData = data;
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableDistance);

            if (collider == null)
            {
                collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }

            collider.size = data.size;
            gameObject.transform.localPosition = data.offset;
            collider.enabled = false;

            playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();

            SetTicketMachine();
        }

        public override void ActivateAttack()
        {
            collider.enabled = true;
            StartCoroutine(DisableCollider());
        }

        private IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(durationTime);
            collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (owner == "Monster")
            {
                if (other.tag == "Player")
                {
                    Debug.Log("MONSTER ATTACKED");
                    SetPayloadAttack(attackData);
                    Attack(payload);
                }
            }
        }

        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat);
        }

        private void SetPayloadAttack(BoxColliderAttackData data)
        {
            Debug.Log("SetPayloadAttack");
            payload.Type = data.combatType;
            payload.Attacker = transform;
            payload.Defender = playerStatus.transform;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = playerStatus.transform.position;
            payload.PlayerStatusEffectName = StatusEffects.StatusEffectName.WeakRigidity;
            payload.Damage = (int)data.attackValue;
        }
        public override void Attack(IBaseEventPayload payload)
        {
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

    }
}
