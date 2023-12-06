using Assets.Scripts.Equipments;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        private PlayerController controller;
        private PlayerInventory inventory;
        private Pickaxe pickaxe;
        private void Awake()
        {
            pickaxe = GetComponent<PlayerController>().Pickaxe;
            controller = GetComponent<PlayerController>();
            inventory = GetComponent<PlayerInventory>();
        }
        public void HandleSmithingAnimationEvent()
        {
            pickaxe.PrintSmithingEffect();
        }

        public void HandleMeleeAttackAnimationStopEvent()
        {
            if (GetComponent<PlayerStatus>().Stamina <= 10.0f)
            {
                controller.ChangeState(PlayerStateName.Exhaust);
            }
            else
            {
                controller.ChangeState(PlayerStateName.Idle);
            }
        }

        public void HandleMeleeAttackColliderOnEvent()
        {
            controller.TurnOnMeleeAttackCollider();
        }

        public void HandleConsumingItemEvent()
        {
            inventory.ConsumeItemEvent();
        }
    }
}