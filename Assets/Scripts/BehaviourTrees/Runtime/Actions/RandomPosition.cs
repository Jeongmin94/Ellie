using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheKiwiCoder
{
    [Serializable]
    public class RandomPosition : ActionNode
    {
        [Tooltip("Minimum bounds to generate point")]
        public Vector2 min = Vector2.one * -10;

        [Tooltip("Maximum bounds to generate point")]
        public Vector2 max = Vector2.one * 10;

        [Tooltip("Blackboard key to write the result to")]
        public NodeProperty<Vector3> result;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var pos = new Vector3();
            pos.x = Random.Range(min.x, max.x);
            pos.y = Random.Range(min.y, max.y);
            result.Value = pos;
            return State.Success;
        }
    }
}