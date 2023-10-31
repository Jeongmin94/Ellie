using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class StoneSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject stone;
        private Pool pool;

        private void Awake()
        {
            pool = PoolManager.Instance.CreatePool(stone, 30);
        }
    }
}