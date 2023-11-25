using Assets.Scripts.Combat;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class NormalStone : BaseStoneEffect
    {
        private LayerMask layerMask;

        private void Start()
        {
            int exceptGroundLayer = LayerMask.NameToLayer("ExceptGround");
            int monsterLayer = LayerMask.NameToLayer("Monster");

            layerMask = (1 << exceptGroundLayer) | (1 << monsterLayer);
        }
    }
}