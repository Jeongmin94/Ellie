using Assets.Scripts.Equipments;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        private Pickaxe pickaxe;
        private void Awake()
        {
            pickaxe = GetComponent<PlayerController>().Pickaxe;
        }
        public void HandleSmithingAnimationEvent()
        {
            pickaxe.PrintSmithingEffect();
        }

        public void HandleMeleeAttackAnimationStopEvent()
        {
            if (GetComponent<PlayerStatus>().Stamina <= 10.0f)
            {
                GetComponent<PlayerController>().ChangeState(PlayerStateName.Exhaust);
            }
            else
            {
                GetComponent<PlayerController>().ChangeState(PlayerStateName.Idle);
            }
        }

        public void HandleMeleeAttackColliderOnEvent()
        {
            GetComponent<PlayerController>().TurnOnMeleeAttackCollider();
        }
    }
}