using Assets.Scripts.Data.ActionData.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] private StaminaData staminaData;

        private int hp;
        public int HP 
        { 
            get { return hp; }
            set 
            { 
                healthData.CurrentHealth.Value = value;
                hp = value;
            }
        }

        private int stamina;
        public int Stamina
        {
            get { return stamina; }
            set
            {
                staminaData.CurrentStamina.Value = value;
                stamina = value;
            }
        }
        private void Awake()
        {
        }
    }
}