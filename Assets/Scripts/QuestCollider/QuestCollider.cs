using Assets.Scripts.InteractiveObjects.NPC;
using UnityEngine;

namespace QuestCollider
{
    public class QuestCollider : MonoBehaviour
    {
        [SerializeField] private NpcType type;
        
        public NpcType GetType() => type;
    }
}