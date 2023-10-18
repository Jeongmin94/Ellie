using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "ChargingData", menuName = "Player/ChargingData")]
    public class ChargingData : ScriptableObject
    {
        public Data<float> ChargingValue = new Data<float>();
    }
}