using System.Collections.Generic;
using Assets.Scripts.Monsters.Attacks;
using Assets.Scripts.Monsters.Utility;
using UnityEngine;

namespace Assets.Scripts.Monsters.AbstractClass
{

    public abstract class AbstractMonster : MonoBehaviour
    {
        //Monster Stat, Type
        protected Structures.MonsterStat monsterStat;
        protected Structures.MonsterType monsterType;

        //PlayerDistance
        protected float travelDistance;
        protected float playerDistance; //삭제 및 자식 클래스 수정 필요

        //Actions
        protected bool isAttacking;

        //Components
        private Rigidbody rigidBody;
        protected AbstractAttack[] skills;
        protected Animator animator;

        //Attack Dictionary
        protected Dictionary<string, AbstractAttack> Attacks = new();


        // >> : Functions
        protected void InitializeStat(Structures.MonsterStat monsterStat)
        {
            this.monsterStat = monsterStat;
        }

        protected void InitializeType(Structures.MonsterType monsterType)
        {
            this.monsterType = monsterType;
        }
        protected AbstractAttack AddSkill(string skillName, Enums.AttackSkill attackSkill)
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
                Attacks.Add(skillName, attack);

            return attack;
        }

    }

}