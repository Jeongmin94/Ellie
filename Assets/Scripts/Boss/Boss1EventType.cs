using Assets.Scripts.Boss.Terrapupa;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class BossEventPayload : BaseEventPayload
    {
        private int intValue;
        private Vector3 vector3Value;
        private Transform transformValue;
        private TerrapupaAttackType attackTypeValue;

        public BossEventPayload()
        {
            intValue = 0;
            vector3Value = Vector3.zero;
            transformValue = null;
            attackTypeValue = TerrapupaAttackType.None;
        }

        public int IntValue
        {
            get { return intValue; }
            set { intValue = value; }
        }

        public Vector3 Vector3Value
        {
            get { return vector3Value; }
            set { vector3Value = value; }
        }

        public Transform TransformValue
        {
            get { return transformValue; }
            set { transformValue = value; }
        }
        public TerrapupaAttackType AttackTypeValue
        {
            get { return attackTypeValue; }
            set { attackTypeValue = value; }
        }
    }
}