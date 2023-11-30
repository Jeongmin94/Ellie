
using UnityEngine;
using System.Collections;

using TheKiwiCoder;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Monsters.Others;
using Assets.Scripts.Monsters.AbstractClass;


using UnityEngine.AI;
using Assets.Scripts.UI.Monster;
using Assets.Scripts.Managers;
using Channels.Components;
using Assets.Scripts.Utils;
using Channels.Type;
using Assets.Scripts.Combat;
using Assets.Scripts.StatusEffects;
using System.Collections.Generic;

namespace Assets.Scripts.Monsters
{
    public class MonsterController : AbstractMonster, ICombatant
    {
        //Temp
        public GameObject player;

        private TicketMachine ticketMachine;

        


        private void Awake()
        {
            //temp, will change with gamecenter
            player = GameObject.Find("Player");

            behaviourTreeInstance = GetComponent<BehaviourTreeInstance>();
            audioController = GetComponent<MonsterAudioController>();

            SetSkills();
            SetTicketMachine();
            InitUI();
            InitData();
        }

        private void Start()
        {
            SetNavMesh();
            SetBehaviourTreeInstance();
        }
        private void SetSkills()
        {
            //if(meleeAttackData!=null)
            //{
            //    AddSkills(meleeAttackData.attackName, Enums.AttackSkill.BoxCollider);
            //    Attacks[meleeAttackData.attackName].InitializeBoxCollider(meleeAttackData);
            //}
            //if(weaponAttackData!=null)
            //{
            //    GameObject obj = Functions.FindChildByName(gameObject, weaponAttackData.weaponName);
            //    weaponAttackData.weapon = obj;
            //    AddSkills(weaponAttackData.attackName, Enums.AttackSkill.WeaponAttack);
            //    Attacks[weaponAttackData.attackName].InitializeWeapon(weaponAttackData);
            //}
            //if(projectileAttackData!=null)
            //{
            //    AddSkills(projectileAttackData.attackName, Enums.AttackSkill.ProjectileAttack);
            //    Attacks[projectileAttackData.attackName].InitializeProjectile(projectileAttackData);
            //}
            //if(fanshapeAttackData!=null)
            //{
            //    AddSkills(fanshapeAttackData.attackName, Enums.AttackSkill.FanshapeAttack);
            //    Attacks[fanshapeAttackData.attackName].InitializeFanShape(fanshapeAttackData);
            //}
        }

        private void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTicket(ChannelType.Combat);
        }

        private void InitUI()
        {
            Transform billboardPos = Functions.FindChildByName(gameObject, "Billboard").transform;

            billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardPos, UIManager.UIMonsterBillboard);
            billboard.scaleFactor = 0.003f;
            billboard.InitBillboard(billboardPos);
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

            //if (meleeAttackData != null)
            //{
            //    behaviourTreeInstance.SetBlackboardValue<float>("MeleeAnimationHold", meleeAttackData.animationHold);
            //    behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackableDistance", meleeAttackData.attackableDistance);
            //    behaviourTreeInstance.SetBlackboardValue<float>("MeleeAttackInterval", meleeAttackData.attackInterval);
            //}
            //if (weaponAttackData != null)
            //{
            //    behaviourTreeInstance.SetBlackboardValue<float>("WeaponAnimationHold", weaponAttackData.animationHold);
            //    behaviourTreeInstance.SetBlackboardValue<float>("WeaponAttackableDistance", weaponAttackData.attackableDistance);
            //    behaviourTreeInstance.SetBlackboardValue<float>("WeaponAttackInterval", weaponAttackData.attackInterval);
            //}
            //if (projectileAttackData != null)
            //{
            //    behaviourTreeInstance.SetBlackboardValue<float>("ProjectimeAnimationHold", projectileAttackData.animationHold);
            //    behaviourTreeInstance.SetBlackboardValue<float>("projectileAttackableDistance", projectileAttackData.attackableMinimumDistance);
            //    behaviourTreeInstance.SetBlackboardValue<float>("ProjectileAttackInterval", projectileAttackData.attackInterval);
            //}
            //if (runToPlayerData != null)
            //{
            //    behaviourTreeInstance.SetBlackboardValue<float>("RunAttackMinimumDistance", runToPlayerData.activateMinimumDistance);
            //    behaviourTreeInstance.SetBlackboardValue<float>("RunAttackInterval", runToPlayerData.attackInterval);
            //}
            //if (fleeSkilldata != null)
            //{
            //    behaviourTreeInstance.SetBlackboardValue<float>("ActivatableFleeDistance", fleeSkilldata.activatableDistance);
            //    behaviourTreeInstance.SetBlackboardValue<float>("ActivateFleeInterval", fleeSkilldata.activateInterval);
            //    behaviourTreeInstance.SetBlackboardValue<float>("FleeDistance", fleeSkilldata.fleeDistance);
            //    behaviourTreeInstance.SetBlackboardValue<float>("FleeSpeed", fleeSkilldata.fleeSpeed);
            //}
            //if(fanshapeAttackData!=null)
            //{
            //    behaviourTreeInstance.SetBlackboardValue<float>("FanshapeAnimationHold", fanshapeAttackData.animationHold);
            //    behaviourTreeInstance.SetBlackboardValue<float>("FanshpaeAttackableDistance", fanshapeAttackData.attackableDistance);
            //    behaviourTreeInstance.SetBlackboardValue<float>("FanshpaeAttackInterval", fanshapeAttackData.attackInterval);
            //}
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

