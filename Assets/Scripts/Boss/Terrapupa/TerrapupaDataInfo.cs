using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    [CreateAssetMenu(fileName = "TerrapupaData", menuName = "Boss/TerrapupaData")]
    public class TerrapupaDataInfo : ScriptableObject
    {
        [Header("몬스터 속성")]
        public int hp;
        public string bossName;
        public Element element;
        public float movementSpeed;

        [Header("돌 던지기")]
        public float throwStoneDetectionDistance;
        public int throwStoneAttackValue;

    }
}