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
    }
}