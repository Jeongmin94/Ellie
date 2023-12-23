using System.Collections;
using Assets.Scripts.Managers;
using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Others;
using Assets.Scripts.StatusEffects;
using Channels.Combat;
using UnityEngine;

namespace Assets.Scripts.Monsters.Attacks
{
    public class AOEPrefabAttack : AbstractAttack
    {
        [SerializeField] private GameObject prefabObject;
        public MonsterAttackData attackData;
        private float damageInterval;
        private Vector3 offset;
        private Transform player;
        private Vector3 position;

        public override void InitializeAOE(MonsterAttackData data)
        {
            InitializedBase(data);
            attackData = data;
            damageInterval = data.attackInterval;
            player = transform.parent.GetComponent<AbstractMonster>().GetPlayer();
            prefabObject = ResourceManager.Instance.LoadExternResource<GameObject>(data.projectilePrefabPath);
        }

        public override void ActivateAttack()
        {
            var obj = Instantiate(prefabObject, player.position - new Vector3(0, 0.9f, 0), transform.rotation)
                .GetComponent<AOE>();
            obj.spawner = gameObject.GetComponent<AOEPrefabAttack>();
            StartCoroutine(StartAttackReadyCount());
        }

        private IEnumerator StartAttackReadyCount()
        {
            IsAttackReady = false;
            yield return new WaitForSeconds(AttackInterval);
            IsAttackReady = true;
        }

        public void SetAndAttack(Transform otherTransform)
        {
            CombatPayload payload = new();
            payload.Defender = otherTransform;
            payload.Damage = attackData.attackValue;
            payload.StatusEffectName = StatusEffectName.WeakRigidity;
            Attack(payload);
        }
    }
}