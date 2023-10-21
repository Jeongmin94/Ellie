using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.AbstractClass;
using UnityEngine;

namespace Assets.Scripts.Monsters.Attacks
{

    public class BoxColliderAttack : AbstractAttack
    {
        [SerializeField] private BoxCollider collider;

        protected bool Attacked { get; private set; }

        public override void InitializeBoxCollider(float attackValue,
            float duration, float attackInterval, float attackRange, Vector3 size, Vector3 offset)
        {
            InitializedBase(attackValue, duration, attackInterval, attackRange);
            owner = gameObject.tag.ToString();

            if (collider == null)
            {
                collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }

            collider.enabled = false;
            collider.size = size;
            gameObject.transform.localPosition = offset;
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
            IsAttackReady = false;
            StartCoroutine(SetAttakingFalse());
        }
        private IEnumerator SetAttakingFalse()
        {
            yield return new WaitForSeconds(AttackInterval);
            IsAttackReady = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (owner == "Monster")
            {
                if (other.tag == "Player")
                {
                    Debug.Log("PlayerAttacked");
                }
            }
        }

    }

}