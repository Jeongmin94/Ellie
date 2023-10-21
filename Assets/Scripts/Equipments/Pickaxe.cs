using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Equipments
{
    public enum PickaxeTier
    {
        Tier5,
        Tier4,
        Tier3,
        Tier2,
        Tier1
    }
    public struct PickaxeStatus
    {
        public PickaxeTier tier;
        public bool isBroken;
        public int minSmithPower;
        public int maxSmithPower;
        public int maxDurability;
        public int curDurability;
    }
    public class Pickaxe : MonoBehaviour
    {
        public PickaxeStatus Status { get; private set; }
        private Mesh mesh;

        
    }
}