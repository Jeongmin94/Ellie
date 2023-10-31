using Assets.Scripts.Item;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerLooting : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            ILootable lootable = collision.gameObject.GetComponent<ILootable>();
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