using UnityEngine;

namespace Data.ActionData.Player
{
    [CreateAssetMenu(fileName = "AimTargetData", menuName = "Player/AimTargetData")]
    public class AimTargetData : ScriptableObject
    {
        public readonly Data<Vector3> TargetPosition = new();
    }
}