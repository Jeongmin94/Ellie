using Assets.Scripts.Channels.Item;
using Assets.Scripts.Data.GoogleSheet;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Item.Stone
{
    public class BaseStoneEffect : MonoBehaviour
    {
        public StoneEventType Type { get; set; }

        private event Action<Transform> effectAction;
        protected StoneData data;

        private void OnDisable()
        {
            effectAction = null;
        }

        public void InitData(StoneData data)
        {
            this.data = data;
        }

        public void SubscribeAction(Action<Transform> action)
        {
            effectAction -= action;
            effectAction += action;
        }

        public virtual void OccurEffect(Transform transform)
        {
            effectAction?.Invoke(transform);
        }
    }
}