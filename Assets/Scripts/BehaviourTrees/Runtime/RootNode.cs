using System;
using UnityEngine;

namespace TheKiwiCoder
{
    [Serializable]
    public class RootNode : Node
    {
        [SerializeReference] [HideInInspector] public Node child;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (child != null)
            {
                return child.Update();
            }

            return State.Failure;
        }
    }
}