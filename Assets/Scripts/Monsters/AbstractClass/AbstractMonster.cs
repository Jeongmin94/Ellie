using System.Collections;
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
        private const float monsterRespawnTime = 5.0f;

        [SerializeField] public SkeletonMonsterData monsterData;

        [SerializeField] public RunToPlayerAttackData runToPlayerData;
        [SerializeField] public BoxColliderAttackData meleeAttackData;
        [SerializeField] public WeaponAttackData weaponAttackData;
        [SerializeField] public ProjectileAttackData projectileAttackData;
        [SerializeField] public FleeSkillData fleeSkilldata;
        [SerializeField] public FanShapeAttackData fanshapeAttackData;

        public BlackboardKey<bool> isDamaged;
        public BlackboardKey<bool> isDead;
        public BlackboardKey<bool> isReturning;

        protected bool isAttacking;
        protected AbstractAttack[] skills;
        protected Animator animator;
        public BehaviourTreeInstance behaviourTreeInstance;
        protected NavMeshAgent agent;

        public Dictionary<string, AbstractAttack> Attacks=new();

        protected MonsterEffectStatusController statusController;

        protected UIMonsterBillboard billboard;
        protected readonly MonsterDataContainer dataContainer = new();

        protected float currentHP;

        protected Vector3 spawnPosition;

        protected MonsterAudioController audioController;

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
            UpdateHP(combatPayload.Damage);
        }

        public void UpdateHP(float damage)
        {
            if (isReturning.value) return;
            currentHP -= damage;
            dataContainer.CurrentHp.Value = (int)currentHP;
            isDamaged.value = true;
            if (currentHP <= 0)
            {
                isDead.value = true;
                MonsterDead();
            }
            else
            {
                audioController.PlayAudio(MonsterAudioType.Hit);
            }
        }

        private void MonsterDead()
        {
            audioController.PlayAudio(MonsterAudioType.Dead);
            StartCoroutine(DisableMonster());
        }

        private IEnumerator DisableMonster()
        {
            if(animator==null)
            {
                animator = GetComponent<Animator>();
            }
            animator.Play("Dead");
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(monsterRespawnTime);

            GetComponent<Collider>().enabled = true;
            ReturnSpawnLocation();
            animator.Play("IdleAttack");
            isDamaged.value = false;
            currentHP = monsterData.maxHP;
            dataContainer.CurrentHp.Value = (int)currentHP;
            yield return new WaitForSeconds(1.0f);
            isDead.value = false;
        }

        public virtual void ReturnSpawnLocation()
        { }
    }

}