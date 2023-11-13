using Assets.Scripts.Combat;
using Channels.Combat;
using UnityEngine;
namespace Assets.Scripts.Item.Stone
{
    public class TestStoneLootable : BaseStone, ICombatant
    {
        [SerializeField] private LayerMask layerMask;

        void Start()
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
