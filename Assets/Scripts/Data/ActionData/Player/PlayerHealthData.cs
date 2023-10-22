using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "PlayerHealthData", menuName = "Player/PlayerHealthData")]
    public class PlayerHealthData : ScriptableObject
    {
        public readonly Data<int> MaxHealth = new Data<int>();
        public readonly Data<int> CurrentHealth = new Data<int>();
    }
}