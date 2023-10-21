using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "ChargingData", menuName = "Player/ChargingData")]
    public class ChargingData : ScriptableObject
    {
        public float[] timeSteps;
        
        [Range(0.0f, 1.0f)]
        public float[] percentages;
        
        public readonly Data<float> ChargingValue = new Data<float>();
    }
}