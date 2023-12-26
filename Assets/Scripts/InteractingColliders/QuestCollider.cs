using Assets.Scripts.InteractiveObjects.NPC;
using UnityEngine;

namespace InteractingColliders
{
    public class QuestCollider : MonoBehaviour
    {
        [SerializeField] private NpcType type;
        
        public new NpcType GetType() => type;
    }
}