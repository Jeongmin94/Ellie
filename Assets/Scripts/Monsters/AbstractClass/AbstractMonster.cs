using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Data;
using Assets.Scripts.Monsters.Attacks;
using Assets.Scripts.Monsters.EffectStatus;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.UI.Framework.Billboard;
using Assets.Scripts.UI.Monster;
using Channels.Combat;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Monsters.AbstractClass
{

    public abstract class AbstractMonster : MonoBehaviour, ICombatant
    {
        protected bool isAttacking;
        protected AbstractAttack[] skills;
        protected Animator animator;
        public BehaviourTreeInstance behaviourTreeInstance;
        protected NavMeshAgent agent;

        public Dictionary<string, AbstractAttack> Attacks=new();

        protected Dictionary<MonsterDamageEffectType, IMonsterStatusEffect> statusEffects;
        protected MonsterEffectStatusController statusController;

        protected UIMonsterBillboard billboard;
        protected readonly MonsterDataContainer dataContainer = new();

        protected float currentHP;

        public AbstractAttack AddSkills(string skillName, Enums.AttackSkill attackSkill)
        {
            AbstractAttack attack = null;
            GameObject newSkill = new GameObject(skillName);
            newSkill.transform.SetParent(transform);
            newSkill.tag = gameObject.tag;
            newSkill.transform.localPosition = Vector3.zero;
            newSkill.transform.localRotation = Quaternion.Euler(Vector3.zero);
            newSkill.transform.localScale = Vector3.one;

            switch (attackSkill)
            {
                case Enums.AttackSkill.ProjectileAttack:
                    attack = newSkill.AddComponent<ProjectileAttack>();
                    break;
                case Enums.AttackSkill.BoxCollider:
                    attack = newSkill.AddComponent<BoxColliderAttack>();
                    break;
                case Enums.AttackSkill.SphereCollider:
                    attack = newSkill.AddComponent<SphereColliderAttack>();
                    break;
                case Enums.AttackSkill.WeaponAttack:
                    attack = newSkill.AddComponent<WeaponAttack>();
                    break;
                case Enums.AttackSkill.AOEAttack:
                    attack = newSkill.AddComponent<AOEPrefabAttack>();
                    break;
                case Enums.AttackSkill.FanshapeAttack:
                    attack = newSkill.AddComponent<FanShapeAttack>();
                    break;
            }

            if (attack != null)
            {
                Attacks.Add(skillName, attack);
            }

            return attack;
        }

        public virtual void Attack(IBaseEventPayload payload)
        { }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            CombatPayload combatPayload = payload as CombatPayload;
            //if(combatPayload.MonsterDamageEffectName!=MonsterDamageEffectType.None)
            //{
            //    statusEffects.TryGetValue(combatPayload.MonsterDamageEffectName, out IMonsterStatusEffect effect);
            //    statusController.ApplyStatusEffect(effect);
            //}
                UpdateHP(combatPayload.Damage);
 
        }

        public abstract void UpdateHP(float damage);

    }

}