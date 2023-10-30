using Assets.Scripts.Boss.Terrapupa;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Boss
{
    public class BossEventPayload : IBaseEventPayload
    {
        private int intValue;
        private float floatValue;
        private Vector3 vector3Value;
        private Transform transformValue1;
        private Transform transformValue2;
        private TerrapupaAttackType attackTypeValue;

        public BossEventPayload()
        {
            intValue = 0;
            floatValue = 0.0f;
            vector3Value = Vector3.zero;
            transformValue1 = null;
            transformValue2 = null;
            attackTypeValue = TerrapupaAttackType.None;
        }

        public int IntValue
        {
            get { return intValue; }
            set { intValue = value; }
        }

        public float FloatValue
        {
            get { return floatValue; }
            set { floatValue = value; }
        }

        public Vector3 Vector3Value
        {
            get { return vector3Value; }
            set { vector3Value = value; }
        }

        public Transform TransformValue1
        {
            get { return transformValue1; }
            set { transformValue1 = value; }
        }

        public Transform TransformValue2
        {
            get { return transformValue2; }
            set { transformValue2 = value; }
        }

        public TerrapupaAttackType AttackTypeValue
        {
            get { return attackTypeValue; }
            set { attackTypeValue = value; }
        }
    }
}