using Assets.Scripts.Combat;
using Channels.Combat;
using System;
using UnityEngine;
namespace Assets.Scripts.Item.Stone
{
    public class StonePrefab : BaseStone, ICombatant
    {
        [SerializeField] private LayerMask layerMask;
        private void Start()
        {
            int exceptGroundLayer = LayerMask.NameToLayer("ExceptGround");
            int monsterLayer = LayerMask.NameToLayer("Monster");

            layerMask = (1 << exceptGroundLayer) | (1 << monsterLayer);
        }

        public void Attack(IBaseEventPayload payload)
        { 
            
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {

        }


        private void OnCollisionEnter(Collision collision)
        {
            // !TODO : 충돌 대상에 대해 조건을 나누고, 페이로드 작성해서 해처리에 보냅니다
            //trail off
            transform.GetChild(0).gameObject.SetActive(false);
            foreach (ContactPoint contact in collision.contacts)
            {
                GameObject hitObject = contact.otherCollider.gameObject;
                if ((layerMask.value & (1 << hitObject.layer)) == 0)
                {
                    continue;
                }

                ICombatant enemy = hitObject.GetComponent<ICombatant>();
                Debug.Log("!!!" + enemy);

                if (enemy != null && !hitObject.CompareTag("Player"))
                {
                    Debug.Log($"{contact} 돌 충돌");
                    hatchery.Attack(GenerateStonePayload(hitObject.transform));
                    break;
                }
            }
            //페이로드 만들어서 hatchery에 쏴줘
        }

        private CombatPayload GenerateStonePayload(Transform defender)
        {
            var payload = new CombatPayload();
            // !TODO : 돌맹이 데이터 읽어와서 현재 돌맹이에 맞는 값으로 페이로드 초기화
            
            //=> test
            payload.Type = CombatType.Projectile;
            payload.Attacker = transform;
            payload.Defender = defender;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = defender.position;
            payload.Damage = data.damage;
            //<=
            return payload;
        }
        
    }
}
