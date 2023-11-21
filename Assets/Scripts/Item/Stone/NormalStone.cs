using Assets.Scripts.Combat;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class NormalStone : BaseStoneEffect
    {
        private LayerMask layerMask;

        private void Start()
        {
            int exceptGroundLayer = LayerMask.NameToLayer("ExceptGround");
            int monsterLayer = LayerMask.NameToLayer("Monster");

            layerMask = (1 << exceptGroundLayer) | (1 << monsterLayer);
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                GameObject hitObject = contact.otherCollider.gameObject;
                if ((layerMask.value & (1 << hitObject.layer)) == 0)
                {
                    continue;
                }

                ICombatant enemy = hitObject.GetComponent<ICombatant>();
                Debug.Log($"{collision} :: ICombatant? -> {enemy}");

                if (enemy != null && !hitObject.CompareTag("Player"))
                {
                    OccurEffect(hitObject.transform);
                    break;
                }
            }
        }
    }
}