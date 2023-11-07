using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.AbstractClass;
using Channels.Combat;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Monsters.Attacks
{
    public class FanShapeAttack : AbstractAttack
    {
        private FanShapeAttackData attackData;
        [SerializeField] private Transform target;

        private void Awake()
        {
            SetTicketMachine();
        }
        public override void InitializeFanShape(FanShapeAttackData data)
        {
            target = GameObject.Find("Player").transform;
            attackData = data;
            base.InitializeFanShape(data);
        }

        public bool CaculateDotProduct()
        {
            Vector3 interV = target.position - transform.position;

            float dot = Vector3.Dot(interV.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= attackData.angleRange / 2.0f)
            {
                return true;
            }

            return false;
        }

        public override void ActivateAttack()
        {
            StartCoroutine(AttackFanshape());
        }

        public IEnumerator AttackFanshape()
        {
            float accumTime = 0.0f;
            while (accumTime <= attackData.attackDuration)
            {
                if (CaculateDotProduct())
                {
                    SetAndAttack(attackData, target);
                    break;
                }
                accumTime += Time.deltaTime;
                yield return null;
            }
        }

        private void SetAndAttack(FanShapeAttackData data, Transform otherTransform)
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

        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, attackData.angleRange / 2, attackData.radius);
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -attackData.angleRange / 2, attackData.radius);
        }
    }
}