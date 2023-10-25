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
        [SerializeField] private List<FountainOfMana> manaObjects = new List<FountainOfMana>();

        private void Start()
        {
            EventBus.Instance.Subscribe(EventBusEvents.SpawnStoneEvent, OnSpawnStone);
            EventBus.Instance.Subscribe<BaseEventPayload>(EventBusEvents.ThrowStoneEvent, OnThrowStone);
        }

        private void OnSpawnStone()
        {
            boss.RightHand.gameObject.SetActive(true);
            Debug.Log("테스트");
        }

        private void OnThrowStone(BaseEventPayload payload)
        {
            PositionEventPayload posPayload = payload as PositionEventPayload;
            Debug.Log(posPayload.Position);
            Debug.Log("던지기");

            Instantiate(boss.RightHand.gameObject, boss.RightHand.position, Quaternion.identity);
            boss.RightHand.gameObject.SetActive(false);
        }
    }
}