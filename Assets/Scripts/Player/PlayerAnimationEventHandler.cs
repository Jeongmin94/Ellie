using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        public void HandleSmithingAnimationEvent()
        {
            GetComponent<PlayerController>().Pickaxe.PrintSmithingEffect();
        }
    }
}