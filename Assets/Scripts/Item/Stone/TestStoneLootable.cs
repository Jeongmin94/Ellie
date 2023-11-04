using Assets.Scripts.Combat;
using Channels.Combat;
using UnityEngine;
namespace Assets.Scripts.Item.Stone
{
    public class TestStoneLootable : BaseStone, ICombatant
    {
        public void Attack(IBaseEventPayload payload)
        { 
            
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision");
            ICombatant enemy = collision.gameObject.GetComponent<ICombatant>();
            if (enemy != null && !collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("돌 발사");
                hatchery.Attack(GenerateStonePayload(collision.transform));
            }
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
            payload.Damage = 5;
            //<=
            return payload;
        }
        
    }
}
