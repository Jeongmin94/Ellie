using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "StaminaData", menuName = "Player/StaminaData")]
    public class StaminaData : ScriptableObject
    {
        [SerializeField] private float maxStamina;
        public readonly Data<float> CurrentStamina = new Data<float>();

        public float MaxStamina
        {
            get { return maxStamina; }
        }

        public void InitStamina()
        {
            CurrentStamina.ClearAction();
            CurrentStamina.Value = maxStamina;
        }

        public float GetPercentage()
        {
            return CurrentStamina.Value / maxStamina;
        }
    }
}