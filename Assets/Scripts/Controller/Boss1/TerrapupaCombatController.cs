using System.Collections;
using Boss1.BossRoomObjects;
using Boss1.Terrapupa;
using Boss1.TerrapupaMinion;
using Channels.Boss;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using Item.Stone;
using Managers.Event;
using Managers.Particle;
using Managers.Sound;
using UnityEngine;
using Utils;

namespace Controller.Boss1
{
    public class TerrapupaCombatController : BaseController
    {
        private bool isFirstTerrapupaHeal;
        
        private TicketMachine ticketMachine;

        public override void InitController()
        {
            Debug.Log($"{name} InitController");

            SubscribeEvents();
            InitTicketMachine();
        }

        private void InitTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets
                (ChannelType.Combat, ChannelType.BossBattle, ChannelType.BossDialog);
        }

        private void SubscribeEvents()
        {
            EventBus.Instance.Subscribe(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);
            EventBus.Instance.Subscribe(EventBusEvents.OccurEarthQuake, OnStartEarthQuake);
            EventBus.Instance.Subscribe(EventBusEvents.BossMeleeAttack, OnBossMeleeAttack);
            EventBus.Instance.Subscribe(EventBusEvents.BossLowAttack, OnBossLowAttack);
            EventBus.Instance.Subscribe(EventBusEvents.BossMinionAttack, OnBossMinionAttack);
            
            EventBus.Instance.Subscribe(EventBusEvents.BossAttractedByMagicStone, OnBossAtrractedByMagicStone);
            EventBus.Instance.Subscribe(EventBusEvents.BossUnattractedByMagicStone, OnBossUnattractedByMagicStone);
            EventBus.Instance.Subscribe(EventBusEvents.StartIntakeMagicStone, OnStartIntakeMagicStone);
            EventBus.Instance.Subscribe(EventBusEvents.IntakeMagicStoneByBoss1, OnIntakeMagicStoneByBoss1);
            
            EventBus.Instance.Subscribe(EventBusEvents.HitStone, OnHitStone);
            EventBus.Instance.Subscribe(EventBusEvents.ApplySingleBossCooldown, OnApplySingleBossCooldown);
        }

        /*
         * 
         */
        
        private void OnSpawnStone(IBaseEventPayload payload)
        {
            Debug.Log("OnSpawnStone :: 보스의 돌맹이 줍기");
            var stonePayload = payload as BossEventPayload;

            var actor = stonePayload.Sender.GetComponent<TerrapupaBehaviourController>();

            actor.Stone.gameObject.SetActive(true);
        }

        private void OnThrowStone(IBaseEventPayload payload)
        {
            Debug.Log("OnThrowStone :: 보스의 돌맹이 던지기");

            var stonePayload = payload as BossEventPayload;
            var actor = stonePayload.Sender.GetComponent<TerrapupaBehaviourController>();

            var stone = Instantiate(actor.Stone.gameObject, transform).GetComponent<TerrapupaStone>();
            
            stone.Init(actor.Stone.position, actor.transform.localScale,
                stonePayload.FloatValue, stonePayload.CombatPayload, stonePayload.PrefabValue, 
                stonePayload.Sender, ticketMachine);
            stone.MoveToTarget(stonePayload.TransformValue1);

            actor.Stone.gameObject.SetActive(false);
        }

        private void OnStartEarthQuake(IBaseEventPayload earthQuakePayload)
        {
            Debug.Log("OnStartEarthQuake :: 내려찍기 공격 피격 체크");
            var payload = earthQuakePayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var hitEffect = payload.PrefabValue;
            var playerTransform = payload.TransformValue1;
            var manaTransform = payload.TransformValue2;
            var hitBossTransform = payload.TransformValue3;
            var boss = payload.Sender;

            // 1. 플레이어 피격
            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;
                var jumpCheckValue = 1.0f;

                LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
                var isJumping = !Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, -Vector3.up, out hit,
                    jumpCheckValue + 0.1f, groundLayer);

                if (hit.collider != null)
                {
                    Debug.Log($"Raycast distance: {hit.distance}");
                }

                if (!isJumping)
                {
                    HitedPlayer(boss, playerTransform, payload.CombatPayload);
                    ParticleManager.Instance.GetParticle(hitEffect, playerTransform, 0.5f);
                }
            }

            // 2. 마나의 샘 피격
            if (manaTransform != null)
            {
                HitedManaFountaine(boss, manaTransform, hitEffect);
            }

            // 3. 타 보스 피격
            if (hitBossTransform != null)
            {
                hitBossTransform = hitBossTransform.GetComponent<TerrapupaDetection>().MyTerrapupa;
                var hitBossControllers = hitBossTransform.GetComponent<TerrapupaBehaviourController>();
                if (hitBossControllers?.TerrapupaData != null)
                {
                    hitBossControllers.TerrapupaData.hitEarthQuake.Value = true;
                    ParticleManager.Instance.GetParticle(hitEffect, hitBossTransform);
                }
            }
        }

        private void OnBossAtrractedByMagicStone(IBaseEventPayload payload)
        {
            if (payload is not BossEventPayload magicStonePayload)
            {
                return;
            }

            Debug.Log("OnBossAtrractedByMagicStone :: 보스 마법 돌맹이를 추적 시작");

            var magicStone = magicStonePayload.TransformValue1;
            var target = magicStonePayload.TransformValue2;
            if (target)
            {
                var actor = target.GetComponent<TerrapupaBehaviourController>();

                actor.AttractMagicStone(magicStone);
            }
        }

        private void OnBossUnattractedByMagicStone(IBaseEventPayload payload)
        {
            if (payload is not BossEventPayload magicStonePayload)
            {
                return;
            }

            Debug.Log("OnBossUnattractedByMagicStone :: 보스 마법 돌맹이를 추적 종료");

            var target = magicStonePayload.TransformValue2;
            if (target)
            {
                var bossController = target.GetComponent<TerrapupaBehaviourController>();
                bossController.UnattractMagicStone();
            }
        }

        private void OnIntakeMagicStoneByBoss1(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnIntakeMagicStoneByBoss1 :: 보스가 마법 돌맹이를 섭취함");

            var payload = bossPayload as BossEventPayload;

            var actor = payload.Sender.GetComponent<TerrapupaBehaviourController>();
            var _magicStone = payload.TransformValue1;
            var healValue = payload.IntValue;

            actor.EndIntakeMagicStone(healValue);

            if (!actor.IsDead && healValue != 0 && !isFirstTerrapupaHeal)
            {
                isFirstTerrapupaHeal = true;
                TerrapupaDialogChannel.SendMessage(TerrapupaDialogTriggerType.IntakeMagicStoneFirstTime, ticketMachine);
            }

            if (_magicStone != null)
            {
                Destroy(_magicStone.gameObject);
            }
        }
        
        private void OnHitStone(IBaseEventPayload payload)
        {
            if (payload is not BossEventPayload bossPayload)
            {
                return;
            }

            Debug.Log("OnHitStone :: 보스가 돌에 맞음");

            var target = bossPayload.TransformValue1.GetComponent<TerrapupaBehaviourController>().TerrapupaData;
            target.hitThrowStone.Value = true;
        }

        private void OnBossMeleeAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnMeleeAttack :: 보스의 근접 공격");

            var payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var hitEffect = payload.PrefabValue;
            var playerTransform = payload.TransformValue1;
            var manaTransform = payload.TransformValue2;
            var boss = payload.Sender;
            var attackValue = payload.IntValue;

            if (playerTransform != null)
            {
                HitedPlayer(boss, playerTransform, payload.CombatPayload);
                if (hitEffect != null)
                {
                    ParticleManager.Instance.GetParticle(hitEffect, playerTransform, 0.5f);
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "TerrapupaRollHit", boss.position);
                }
            }

            if (manaTransform != null)
            {
                HitedManaFountaine(boss, manaTransform);
            }
        }

        private void OnBossLowAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnLowAttack");
            var payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var playerTransform = payload.TransformValue1;
            var manaTransform = payload.TransformValue2;
            var boss = payload.Sender;

            var jumpCheckValue = 1.0f;

            if (playerTransform != null)
            {
                // 플레이어 아래 광선을 쏴서 점프 체크
                RaycastHit hit;

                LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");
                var isJumping = !Physics.Raycast(playerTransform.position + Vector3.up * 0.1f, -Vector3.up, out hit,
                    jumpCheckValue + 0.1f, groundLayer);

                Debug.Log($"Raycast distance: {hit.distance}");
                if (!isJumping)
                {
                    var bossController = boss.GetComponent<TerrapupaBehaviourController>();
                    HitedPlayer(boss, playerTransform, payload.CombatPayload);
                }
            }

            if (manaTransform != null)
            {
                HitedManaFountaine(payload.Sender, manaTransform);
            }
        }

        private void OnBossMinionAttack(IBaseEventPayload bossPayload)
        {
            Debug.Log("OnMinionAttack :: 보스 미니언의 공격");

            var payload = bossPayload as BossEventPayload;

            if (payload == null)
            {
                return;
            }

            var playerTransform = payload.TransformValue1;
            var minion = payload.Sender;

            Debug.Log(playerTransform);

            if (playerTransform != null)
            {
                HitedPlayer(minion, playerTransform, payload.CombatPayload);
            }

            StartCoroutine(MinionAttackCooldown(payload));
        }
        
        private void OnApplySingleBossCooldown(IBaseEventPayload cooldownPayload)
        {
            Debug.Log("ApplyBossAttackCooldown :: 쿨타임 적용");
            var payload = cooldownPayload as BossEventPayload;

            var bossController = payload.Sender.GetComponent<TerrapupaBehaviourController>();
            var cooldown = payload.FloatValue;
            var banType = payload.AttackTypeValue;

            bossController.Cooldown(cooldown, banType);
        }

        private void OnStartIntakeMagicStone(IBaseEventPayload bossPayload)
        {
            var payload = bossPayload as BossEventPayload;

            var bossController = payload.Sender.GetComponent<TerrapupaBehaviourController>();
            var magicStone = payload.TransformValue1;

            // 지속시간 체크 정지
            magicStone.GetComponent<MagicStone>().StopCheckDuration();
            bossController.StartIntakeMagicStone();
        }
        
        /*
         * 
         */
        
        private IEnumerator MinionAttackCooldown(BossEventPayload payload)
        {
            var minionController = payload.Sender.GetComponent<TerrapupaMinionBehaviourController>();
            Debug.Log($"{minionController} 공격 봉인");
            minionController.MinionData.canAttack.Value = false;

            yield return new WaitForSeconds(payload.FloatValue);

            Debug.Log($"MinionAttackCooldown :: {minionController} 쿨다운 완료");
            minionController.MinionData.canAttack.Value = true;
        }

        /*
         * 
         */
        
        private void HitedPlayer(Transform attacker, Transform player, CombatPayload payload)
        {
            if (payload == null)
            {
                return;
            }

            Debug.Log($"플레이어 피해 {payload.Damage} 입음");
            payload.Attacker = attacker;
            payload.Defender = player;

            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        private void HitedManaFountaine(Transform attacker, Transform manaTransform, GameObject hitEffect = null)
        {
            var manaFountain = manaTransform.GetComponent<ManaFountain>();

            if (manaFountain != null)
            {
                manaFountain.IsBroken = true;

                EventBus.Instance.Publish(EventBusEvents.DestroyedManaByBoss1,
                    new BossEventPayload
                    {
                        PrefabValue = hitEffect,
                        Sender = attacker,
                        TransformValue1 = manaTransform,
                        AttackTypeValue = manaFountain.banBossAttackType
                    });
            }
        }
    }
}
