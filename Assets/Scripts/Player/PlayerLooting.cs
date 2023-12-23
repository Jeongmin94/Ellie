using Assets.Scripts.Item;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerLooting : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var lootable = other.gameObject.GetComponent<ILootable>();
            if (lootable != null)
            {
                Accept(lootable);
            }
        }

        private void Accept(ILootable item)
        {
            item.Visit(this);
        }
    }
}