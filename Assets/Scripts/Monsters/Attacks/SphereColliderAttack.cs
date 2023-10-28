using System.Collections;
using Assets.Scripts.Monsters.AbstractClass;
using UnityEngine;

namespace Assets.Scripts.Monsters.Attacks
{

    public class SphereColliderAttack : AbstractAttack
    {
        private SphereCollider collider;

        public override void InitializeSphereCollider(SphereColliderAttackData data)
        {
            InitializedBase(data.attackValue, data.attackDuration, data.attackInterval, data.attackableDistance);
            owner = gameObject.tag.ToString();

            if (collider == null)
            {
                collider = gameObject.AddComponent<SphereCollider>();
                collider.isTrigger = true;
                collider.enabled = false;
            }
            gameObject.transform.localPosition = data.offset;
            collider.radius = data.radius;
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

    }
}