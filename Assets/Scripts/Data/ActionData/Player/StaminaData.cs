using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "StaminaData", menuName = "Player/StaminaData")]
    public class StaminaData : ScriptableObject
    {
        [SerializeField] private int maxStamina;
        public readonly Data<int> CurrentStamina = new Data<int>();
        
        public int MaxStamina
        {
            get { return maxStamina; }
        }

        public void InitStamina()
        {
            CurrentStamina.Value = maxStamina;
        }

        public float GetPercentage()
        {
            return CurrentStamina.Value / maxStamina;
        }
    }
}