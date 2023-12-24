using System.Collections;
using Data.Monster;
using Monsters.AbstractClass;
using UnityEngine;

namespace Monsters.Attacks
{
    public class SphereColliderAttack : AbstractAttack
    {
        private SphereCollider collider;

        private void OnTriggerEnter(Collider other)
        {
            if (owner == "Monster")
            {
                if (other.tag == "Player")
                {
                    //Player Recieve Attack
                }
            }
        }

        public override void InitializeSphereCollider(MonsterAttackData data)
        {
            InitializedBase(data);
            owner = gameObject.tag;

            if (collider == null)
            {
                collider = gameObject.AddComponent<SphereCollider>();
                collider.isTrigger = true;
                collider.enabled = false;
            }

            gameObject.transform.localPosition = data.offset;
            //collider.radius = data.radius;
        }

        public override void ActivateAttack()
        {
            collider.enabled = true;
            StartCoroutine("DisableCollider");
        }

        private IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(durationTime);
            collider.enabled = false;
        }
    }
}