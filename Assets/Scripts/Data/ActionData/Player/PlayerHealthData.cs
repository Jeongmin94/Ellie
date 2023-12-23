using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "PlayerHealthData", menuName = "Player/PlayerHealthData")]
    public class PlayerHealthData : ScriptableObject
    {
        [SerializeField] private int maxHealth;
        public readonly Data<int> CurrentHealth = new();

        public int MaxHealth => maxHealth;

        public void InitHealth()
        {
            CurrentHealth.ClearAction();
            CurrentHealth.Value = maxHealth;
        }
    }
}