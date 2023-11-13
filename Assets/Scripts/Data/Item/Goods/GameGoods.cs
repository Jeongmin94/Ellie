using Assets.Scripts.ActionData;
using UnityEngine;

namespace Assets.Scripts.Item.Goods
{
    [CreateAssetMenu(fileName = "GameGoods", menuName = "Item/GameGoods")]
    public class GameGoods : ScriptableObject
    {
        public readonly Data<int> gold = new Data<int>();
        public readonly Data<int> stonePiece = new Data<int>();

        public void Init()
        {
            gold.Value = 0;
            stonePiece.Value = 0;
        }
    }
}