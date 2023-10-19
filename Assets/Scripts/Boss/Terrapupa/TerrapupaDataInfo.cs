using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    [CreateAssetMenu(fileName = "TerrapupaData", menuName = "Boss/TerrapupaData")]
    public class TerrapupaDataInfo : ScriptableObject
    {
        [Tooltip("몬스터 속성")]
        public int HP;
        public string Name;
        public Element Element;
        public float Movement;
    }
}