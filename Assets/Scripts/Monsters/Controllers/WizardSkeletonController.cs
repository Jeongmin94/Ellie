using UnityEngine;
using System.Collections;
using TheKiwiCoder;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Monsters.Others;
using Assets.Scripts.Monsters.AbstractClass;
using UnityEngine.AI;
using Assets.Scripts.UI.Monster;
using Assets.Scripts.Managers;
using Assets.Scripts.Combat;
using Assets.Scripts.StatusEffects;
using static Assets.Scripts.Monsters.Utility.Enums;

namespace Assets.Scripts.Monsters
{
    public class WizardSkeletonController : AbstractMonster, ICombatant
    {
        const int projectileAttackNum = 2009;
        const int fleeSkillNum = 2008;

        private void Awake()
        {
            behaviourTreeInstance = GetComponent<BehaviourTreeInstance>();
            audioController = GetComponent<MonsterAudioController>();

            SetTicketMachine();
            InitUI();
        }

        private void Start()
        {
            StartCoroutine(InitParsingData());
        }

        private IEnumerator InitParsingData()
        {
            yield return DataManager.Instance.CheckIsParseDone();
            monsterData = DataManager.Instance.GetIndexData<SkeletonMonsterData, SkeletonMonsterDataParsingInfo>(((int)MonsterNumber.WizardSkeleton));

            SetSkills();
            InitData();
            SetNavMesh();
            SetBehaviourTreeInstance();
        }
        private void SetSkills()
        {
            for (int i = 0; i < (int)AttackSkill.End; i++)
            {
                attackData[i] = null;
            }

            attackData[(int)AttackSkill.Flee] = DataManager.Instance.GetIndexData<MonsterAttackData, MonsterAttackDataparsingInfo>(fleeSkillNum);
            attackData[(int)AttackSkill.ProjectileAttack]= DataManager.Instance.GetIndexData<MonsterAttackData, MonsterAttackDataparsingInfo>(projectileAttackNum);

            for (int i = 0; i < (int)AttackSkill.End; i++)
            {
                MonsterAttackData temp = attackData[i];
                if (temp == null) continue;

                AbstractAttack tempAttack = AddSkills(temp.attackName, temp.attackType);
                // Debug.Log(temp.attackName);

                switch (temp.attackType)
                {
                    case AttackSkill.RunToPlayer:
                        behaviourTreeInstance.SetBlackboardValue<float>("RunAttackMinimumDistance", temp.attackableDistance);
                        behaviourTreeInstance.SetBlackboardValue<float>("RunAttackInterval", temp.attackInterval);
                        break;
                    case AttackSkill.Flee:
                        behaviourTreeInstance.SetBlackboardValue<float>("ActivatableFleeDistance", temp.attackableDistance);
                        behaviourTreeInstance.SetBlackboardValue<float>("ActivateFleeInterval", temp.attackInterval);
                        behaviourTreeInstance.SetBlackboardValue<float>("FleeDistance", temp.fleeDistance);
                        behaviourTreeInstance.SetBlackboardValue<float>("FleeSpeed", temp.movementSpeed);
                        break;
                    case AttackSkill.BoxCollider:
                        tempAttack.InitializeBoxCollider(temp);
                        behaviourTreeInstance.SetBlackboardValue<float>("MeleeAnimationHold", temp.animationHold);
                        behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackableDistance", temp.attackableDistance);
                        behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackInterval", temp.attackInterval);
                        break;
                    case AttackSkill.ProjectileAttack:
                        tempAttack.InitializeProjectile(temp);
                        behaviourTreeInstance.SetBlackboardValue<float>("ProjectimeAnimationHold", temp.animationHold);
                        behaviourTreeInstance.SetBlackboardValue<float>("projectileAttackableDistance", temp.attackableMinimumDistance);
                        behaviourTreeInstance.SetBlackboardValue<float>("ProjectileAttackInterval", temp.attackInterval);
                        break;
                    case AttackSkill.FanshapeAttack:
                        tempAttack.InitializeFanShape(temp);
                        behaviourTreeInstance.SetBlackboardValue<float>("FanshapeAnimationHold", temp.animationHold);
                        behaviourTreeInstance.SetBlackboardValue<float>("FanshpaeAttackableDistance", temp.attackableDistance);
                        behaviourTreeInstance.SetBlackboardValue<float>("FanshpaeAttackInterval", temp.attackInterval);
                        break;
                }
            }
        }

        private void InitData()
        {
            dataContainer.MaxHp = (int)monsterData.maxHP;
            currentHP = monsterData.maxHP;
            dataContainer.PrevHp = (int)currentHP;
            dataContainer.CurrentHp.Value = (int)currentHP;
            dataContainer.Name = monsterData.monsterName;

            billboard.InitData(dataContainer);
        }

        private void SetBehaviourTreeInstance()
        {
            //Temp
            behaviourTreeInstance.SetBlackboardValue<GameObject>("Player", player);
            //<<

            GameObject obj = Functions.FindChildByName(gameObject, "ChasePlayer");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("DetectChaseAI", obj);
            obj.GetComponent<DistanceDetectedAI>().SetDetectDistance(monsterData.chasePlayerDistance);

            obj = Functions.FindChildByName(gameObject, "PlayerDetect");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("DetectPlayerAI", obj);
            obj.GetComponent<DistanceDetectedAI>().SetDetectDistance(monsterData.detectPlayerDistance);

            obj = Functions.FindChildByName(gameObject, "PatrolPoints");
            behaviourTreeInstance.SetBlackboardValue<GameObject>("PatrolPoints", obj);
            obj.SetActive(false);

            spawnPosition = gameObject.transform.position;
            behaviourTreeInstance.SetBlackboardValue<Vector3>("SpawnPosition", spawnPosition);
            behaviourTreeInstance.SetBlackboardValue<float>("MovementSpeed", monsterData.movementSpeed);
            isDead = behaviourTreeInstance.FindBlackboardKey<bool>("IsDead");
            isDamaged = behaviourTreeInstance.FindBlackboardKey<bool>("IsDamaged");
            isReturning = behaviourTreeInstance.FindBlackboardKey<bool>("IsReturning");
            isDead.value = false;
            isDamaged.value = false;
        }

        private void SetNavMesh()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = monsterData.movementSpeed;
            agent.angularSpeed = monsterData.rotationSpeed;
            agent.stoppingDistance = monsterData.stopDistance;
            agent.baseOffset = -0.1f;
        }

        public void ChangeEffectState(StatusEffectName effect)
        {

        }
        public override void ReturnSpawnLocation()
        {
            gameObject.transform.position = spawnPosition;
        }
    }
}

