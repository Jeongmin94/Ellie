using Assets.Scripts.Managers;
using Assets.Scripts.Player;
using UnityEngine;
namespace Assets.Scripts.Item.Stone
{
    public class TestStoneLootable : BaseStone, ILootable
    {
        public void Visit(PlayerLooting player)
        {
            Debug.Log("Player Loot : " + this.name);
            PoolManager.Instance.Push(this);
        }
    }
}
