using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Monsters.Attacks;
using Assets.Scripts.Monsters.Utility;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Monsters.AbstractClass
{

    public abstract class AbstractMonster : MonoBehaviour
    {
        //Monster Stat, Type
        

        //Actions
        protected bool isAttacking;

        //Components
        protected AbstractAttack[] skills;
        protected Animator animator;
        public BehaviourTreeInstance behaviourTreeInstance;
        protected NavMeshAgent agent;

        //Attack Dictionary
        public Dictionary<string, AbstractAttack> Attacks = new();


        // >> : Functions
        //protected void InitializeData(SkeletonMonsterData monsterData)
        //{
        //    this.monsterData = monsterData;
        //}

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
            }

            if (attack != null)
            {
                Attacks.Add(skillName, attack);
            }

            return attack;
        }

    }

}