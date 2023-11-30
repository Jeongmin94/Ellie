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
using static Assets.Scripts.Monsters.Utility.Enums;

namespace Assets.Scripts.Monsters.AbstractClass
{
    public enum MonsterNumber
    {
        NormalSkeleton = 1000,
        AdventureSkeleton=1001,
        WizardSkeleton=1002,
        CaveBat=1003,
        GuildguardSkeleton = 1004,
    }
    public abstract class AbstractMonster : MonoBehaviour, ICombatant
    {
        private const float monsterRespawnTime = 10.0f;
        private const float monsterDisable = 5.0f;


        [SerializeField] public SkeletonMonsterData monsterData;
        public MonsterAttackData[] attackData = new MonsterAttackData[(int)AttackSkill.End];

        //[SerializeField] public RunToPlayerAttackData runToPlayerData;
        //[SerializeField] public BoxColliderAttackData meleeAttackData;
        //[SerializeField] public WeaponAttackData weaponAttackData;
        //[SerializeField] public ProjectileAttackData projectileAttackData;
        //[SerializeField] public FleeSkillData fleeSkilldata;
        //[SerializeField] public FanShapeAttackData fanshapeAttackData;
        //[SerializeField] public MonsterDropableItemData dropableItemData;

        public BlackboardKey<bool> isDamaged;
        public BlackboardKey<bool> isDead;
        public BlackboardKey<bool> isReturning;

        protected bool isAttacking;
        protected AbstractAttack[] skills;
        protected Animator animator;
        public BehaviourTreeInstance behaviourTreeInstance;
        protected NavMeshAgent agent;
        private bool isHeadShot;

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
            Debug.Log("ATTACK : " + attack);
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

        public void RecieveHeadShot()
        {
            isHeadShot = true;
        }

        public void UpdateHP(float damage)
        {
            if (isReturning.value) return;
            if (isHeadShot) damage *= monsterData.weakRatio;
            currentHP -= damage;
            dataContainer.CurrentHp.Value = (int)currentHP;
            isDamaged.value = true;
            if (currentHP <= 0)
            {
                DropItem();
                isDead.value = true;
                MonsterDead();
            }
            else
            {
                audioController.PlayAudio(MonsterAudioType.Hit);
            }

            isHeadShot = false;
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

            billboard.scaleFactor = 0.0f;

            yield return new WaitForSeconds(monsterDisable);

            transform.position = new Vector3(999, 999, 999);
            yield return new WaitForSeconds(monsterRespawnTime-monsterDisable);

            ResetMonster();
        }

        private void ResetMonster()
        {
            ReturnSpawnLocation();
            billboard.scaleFactor = 0.003f;
            GetComponent<Collider>().enabled = true;
            animator.Play("IdleAttack");
            isDamaged.value = false;
            currentHP = monsterData.maxHP;
            dataContainer.CurrentHp.Value = (int)currentHP;
            isDead.value = false;
        }


        public virtual void ReturnSpawnLocation()
        { }

        public void DropItem()
        {
            //foreach (DropItem a in dropableItemData.items)
            //{
            //    float random = Random.Range(0.0f, 1.0f);
            //    if (random < a.dropChance)
            //    {
            //        Vector3 itemLocation = transform.position;
            //        itemLocation.y += 1.0f;
            //        Instantiate(a, itemLocation, transform.rotation);
            //        break;
            //    }
            //}
        }
    }

}