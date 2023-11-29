using System.Collections;
using Assets.Scripts.Combat;
using Assets.Scripts.Monsters.AbstractClass;
using Channels.Combat;
using UnityEngine;

namespace Assets.Scripts.Monsters.Attacks
{
    public class WeaponAttack : AbstractAttack
    {
        private Collider collider;
        private WeaponAttackData attackData;

        private void Awake()
        {
            SetTicketMachine();
        }

        public override void InitializeWeapon(WeaponAttackData data)
        {
            attackData = data;
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableDistance);
            if(collider==null)
            {
                collider = data.weapon.GetComponent<Collider>();
                if (collider == null) Debug.Log("[WeaponAttack] CanNotFindCollider");
                collider.isTrigger = true;
            }
            collider.enabled = false;
        }

        public override void ActivateAttack()
        {
            if (collider == null)
            {
                collider = gameObject.GetComponent<Collider>();
                collider.enabled = false;
                if (collider == null) Debug.Log("[WeaponAttack] CanNotFindCollider");
            }
            collider.enabled = true;
            StartCoroutine(DisableCollider());
        }

        private IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(durationTime);
            collider.enabled = false;
            StartCoroutine(SetAttackReady());
        }

        private IEnumerator SetAttackReady()
        {
            yield return new WaitForSeconds(AttackInterval);
        }

        public void OnWeaponTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<ICombatant>() != null)
                {
                    SetAndAttack(attackData, other.transform);
                }
            }
        }

        private void SetAndAttack(WeaponAttackData data, Transform otherTransform)
        {
            Debug.Log("SetPayloadAttack");
            CombatPayload payload = new();
            payload.Type = data.combatType;
            payload.Attacker = transform;
            payload.Defender = otherTransform;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = otherTransform.position;
            payload.PlayerStatusEffectName = StatusEffects.StatusEffectName.WeakRigidity;
            payload.Damage = (int)data.attackValue;
            Attack(payload);
        }

    }
}