using Assets.Scripts.Boss.Objects;
using Assets.Scripts.Boss.Terrapupa;
using Assets.Scripts.Player;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class Boss1GameCenter : MonoBehaviour
    {
        [SerializeField] private TerrapupaController boss;
        [SerializeField] private PlayerController player;
        [SerializeField] private List<ManaFountain> manaObjects = new List<ManaFountain>();

        private void Start()
        {
            EventBus.Instance.Subscribe(EventBusEvents.GripStoneByBoss1, OnSpawnStone);
            EventBus.Instance.Subscribe<BaseEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnThrowStone);
            EventBus.Instance.Subscribe<BossEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnManaFountain);
        }

        private void OnSpawnStone()
        {
            boss.RightHand.gameObject.SetActive(true);
            Debug.Log("테스트");
        }

        private void OnThrowStone(BaseEventPayload payload)
        {
            BossEventPayload posPayload = payload as BossEventPayload;
            Debug.Log(posPayload.Vector3Value);
            Debug.Log("던지기");

            Instantiate(boss.RightHand.gameObject, boss.RightHand.position, Quaternion.identity);
            boss.RightHand.gameObject.SetActive(false);
        }

        private void OnManaFountain(BossEventPayload manaPayload)
        {

        }
    }
}