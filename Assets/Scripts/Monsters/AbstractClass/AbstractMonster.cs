using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Combat;
using Channels.Components;
using Channels.Monsters;
using Channels.Type;
using Combat;
using Data.GoogleSheet;
using Data.Monster;
using Monsters.Attacks;
using Monsters.Controllers;
using Monsters.EffectStatus;
using Monsters.Utility;
using Player.HitComponent;
using Player.StatusEffects;
using Sirenix.OdinInspector;
using TheKiwiCoder;
using UI.Monster;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using static Monsters.Utility.Enums;

namespace Monsters.AbstractClass
{
    public enum MonsterNumber
    {
        NormalSkeleton = 1000,
        AdventureSkeleton = 1001,
        WizardSkeleton = 1002,
        CaveBat = 1003,
        GuildguardSkeleton = 1004,
        CaveWitch = 1005
    }

    public abstract class AbstractMonster : MonoBehaviour, ICombatant, IMonster
    {
        private const float billboardScale = 0.003f;
        private const float battleStateTime = 8.0f;

        [SerializeField] public SkeletonMonsterData monsterData;
        [SerializeField] public GameObject freezeEffect;
        public MonsterAttackData[] attackData = new MonsterAttackData[(int)AttackSkill.End];
        public BehaviourTreeInstance behaviourTreeInstance;
        public Renderer renderer;
        [SerializeField] public SkeletonMesh characterMesh;
        public bool isBillboardOn;

        protected readonly MonsterDataContainer dataContainer = new();
        protected NavMeshAgent agent;
        protected Animator animator;

        public Dictionary<string, AbstractAttack> Attacks = new();

        protected MonsterAudioController audioController;
        private Coroutine battleCoroutine;

        protected UIMonsterBillboard billboard;
        protected Transform billboardObject;
        private Transform cameraObj;

        protected float currentHP;
        private MaterialHitComponent hitComponent;

        protected bool isAttacking;

        public BlackboardKey<bool> isDamaged;
        public BlackboardKey<bool> isDead;
        private bool isHeadShot;
        public BlackboardKey<bool> isReturning;
        private MonsterParticleController particleController;
        protected GameObject player;
        protected AbstractAttack[] skills;

        protected Vector3 spawnPosition;

        protected MonsterEffectStatusController statusController;

        // 디버프 관리 클래스
        private MonsterStatus statusEffect;
        private TicketMachine ticketMachine;

        protected virtual void Awake()
        {
            statusEffect = gameObject.GetOrAddComponent<MonsterStatus>();
            particleController = gameObject.GetOrAddComponent<MonsterParticleController>();
            hitComponent = gameObject.GetOrAddComponent<MaterialHitComponent>();

            freezeEffect = transform.Find("FreezeEffect").gameObject;
            freezeEffect.SetActive(false);
        }

        private void Update()
        {
            MonsterOnPlayerForward();
        }

        public void Attack(IBaseEventPayload payload)
        {
            var a = payload as CombatPayload;
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            var combatPayload = payload as CombatPayload;

            if (combatPayload.StatusEffectName != StatusEffectName.None && currentHP > 0)
            {
                if (statusEffect == null)
                {
                    Debug.LogError($"{transform.name} 상태이상 처리 로직 없음, Awake() 추가");
                }

                // 디버프 처리
                statusEffect.ApplyStatusEffect(combatPayload);
            }

            hitComponent.Hit();
            UpdateHP(combatPayload.Damage);
        }

        public void MonsterDead(IBaseEventPayload payload)
        {
            throw new NotImplementedException();
        }

        public void SetPlayer(GameObject ply)
        {
            player = ply;
        }

        public AbstractAttack AddSkills(string skillName, AttackSkill attackSkill)
        {
            AbstractAttack attack = null;
            var newSkill = new GameObject(skillName);
            newSkill.transform.SetParent(transform);
            newSkill.tag = gameObject.tag;
            newSkill.transform.localPosition = Vector3.zero;
            newSkill.transform.localRotation = Quaternion.Euler(Vector3.zero);
            newSkill.transform.localScale = Vector3.one;
            switch (attackSkill)
            {
                case AttackSkill.ProjectileAttack:
                    attack = newSkill.AddComponent<ProjectileAttack>();
                    break;
                case AttackSkill.BoxCollider:
                    attack = newSkill.AddComponent<BoxColliderAttack>();
                    break;
                case AttackSkill.SphereCollider:
                    attack = newSkill.AddComponent<SphereColliderAttack>();
                    break;
                case AttackSkill.WeaponAttack:
                    attack = newSkill.AddComponent<WeaponAttack>();
                    break;
                case AttackSkill.AOEAttack:
                    attack = newSkill.AddComponent<AOEPrefabAttack>();
                    break;
                case AttackSkill.FanshapeAttack:
                    attack = newSkill.AddComponent<FanShapeAttack>();
                    break;
            }

            if (attack != null)
            {
                Attacks.Add(skillName, attack);
            }

            return attack;
        }

        public void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Monster);
        }

        [Button("몬스터 빙결 상태이상 체크", ButtonSizes.Large)]
        public void Test()
        {
            ReceiveDamage(new CombatPayload
            {
                Damage = 1,
                StatusEffectName = StatusEffectName.Incarceration,
                statusEffectduration = 10.0f
            });
        }

        public void RecieveHeadShot()
        {
            isHeadShot = true;
        }

        public void UpdateHP(float damage)
        {
            if (isReturning.value)
            {
                return;
            }

            ShowBillboard();

            if (isHeadShot)
            {
                damage *= monsterData.weakRatio;
            }

            if (battleCoroutine != null)
            {
                StopCoroutine(battleCoroutine);
            }

            battleCoroutine = StartCoroutine(EndBattleState());

            currentHP -= damage;
            dataContainer.CurrentHp.Value = (int)currentHP;

            if (!behaviourTreeInstance.FindBlackboardKey<bool>("IsFreezing").Value)
            {
                isDamaged.value = true;
            }

            if (currentHP < 1)
            {
                SendTicket();
                SetMonsterDead();
                HideBillobard();
                isDead.Value = true;
                isBillboardOn = false;
            }
            else
            {
                if (!isHeadShot)
                {
                    audioController.PlayAudio(MonsterAudioType.Hit);
                    particleController.PlayParticle(MonsterParticleType.Hit);
                }
                else
                {
                    audioController.PlayAudio(MonsterAudioType.HeadShot);
                    particleController.PlayParticle(MonsterParticleType.HeadShot);
                }
            }

            isHeadShot = false;
        }

        private IEnumerator EndBattleState()
        {
            yield return new WaitForSeconds(battleStateTime);
            HideBillobard();
            isBillboardOn = false;
        }

        private void SetMonsterDead()
        {
            audioController.PlayAudio(MonsterAudioType.Dead);
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            animator.Play("Dead");
            GetComponent<Collider>().enabled = false;
        }

        public void ResetMonster()
        {
            ReturnSpawnLocation();
            GetComponent<Collider>().enabled = true;
            animator.Play("IdleAttack");
            isDamaged.value = false;
            currentHP = monsterData.maxHP;
            dataContainer.CurrentHp.Value = (int)currentHP;
            isDead.value = false;
        }

        public virtual void ReturnSpawnLocation()
        {
        }

        public void SendTicket()
        {
            MonsterPayload monsterPayload = new();
            monsterPayload.RespawnTime = monsterData.respawnTime;
            monsterPayload.Monster = transform;
            monsterPayload.ItemDrop = monsterData.itemDropTable;
            ticketMachine.SendMessage(ChannelType.Monster, monsterPayload);
        }

        protected void InitUI()
        {
            billboardObject = Functions.FindChildByName(gameObject, "Billboard").transform;
            cameraObj = Camera.main.transform;

            billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardObject,
                UIManager.UIMonsterBillboard);
            HideBillobard();
            billboard.InitBillboard(billboardObject);
        }

        public void ShowBillboard()
        {
            billboardObject.transform.localScale = Vector3.one;
            isBillboardOn = true;
        }

        public void HideBillobard()
        {
            billboardObject.transform.localScale = Vector3.zero;
        }

        public void MonsterOnPlayerForward()
        {
            if (isBillboardOn)
            {
                var direction = transform.position - cameraObj.position;
                var dot = Vector3.Dot(direction.normalized, cameraObj.forward.normalized);

                if (dot > 0)
                {
                    ShowBillboard();
                }
                else
                {
                    HideBillobard();
                }
            }
        }

        public Transform GetPlayer()
        {
            return player.transform;
        }
    }
}