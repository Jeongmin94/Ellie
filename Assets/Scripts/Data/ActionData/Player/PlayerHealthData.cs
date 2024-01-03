using UnityEngine;

namespace Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "PlayerHealthData", menuName = "Player/PlayerHealthData")]
    public class PlayerHealthData : ScriptableObject
    {
        [SerializeField] private int maxHealth;
        public readonly Data<int> CurrentHealth = new();

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public void InitHealth()
        {
            CurrentHealth.ClearAction();
            CurrentHealth.Value = maxHealth;
        }
    }
}