using UnityEngine;

namespace Data.ActionData.Monster
{
    [CreateAssetMenu(fileName = "MonsterHealthData", menuName = "Monster/MonsterHealthData", order = 0)]
    public class MonsterHealthData : ScriptableObject
    {
        [SerializeField] private int maxHealth;
        public readonly Data<int> CurrentHealth = new();

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