using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Monsters.AbstractClass;
using UnityEngine;

namespace Assets.Scripts.Monsters.Attacks
{
    public class BoxColliderAttack : AbstractAttack
    {
        private BoxCollider collider;

        public override void InitializeBoxCollider(BoxColliderAttackData data)
        {
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableDistance);

            if (collider == null)
            {
                collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }

            collider.size = data.size;
            gameObject.transform.localPosition = data.offset;

            collider.enabled = false;
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
                    Debug.Log("PlayerAttacked");
                }
            }
        }

    }
}
