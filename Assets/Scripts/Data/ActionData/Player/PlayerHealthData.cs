using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "PlayerHealthData", menuName = "Player/PlayerHealthData")]
    public class PlayerHealthData : ScriptableObject
    {
        [SerializeField] private int maxHealth;
        public readonly Data<int> CurrentHealth = new Data<int>();

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public void InitHealth()
        {
            CurrentHealth.Value = maxHealth;
        }
    }
}