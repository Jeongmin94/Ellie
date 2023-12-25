using System;
using UnityEngine;

namespace TheKiwiCoder
{
    [Serializable]
    public class Log : ActionNode
    {
        [Tooltip("Message to log to the console")]
        public NodeProperty<string> message = new();

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            Debug.Log($"{message.Value}");
            return State.Success;
        }
    }
}