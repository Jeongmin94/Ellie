using System;

namespace TheKiwiCoder
{
    [Serializable]
    public class CompareProperty : ActionNode
    {
        public BlackboardKeyValuePair pair;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var source = pair.value;
            var destination = pair.key;

            if (source != null && destination != null)
            {
                if (destination.Equals(source))
                {
                    return State.Success;
                }
            }

            return State.Failure;
        }
    }
}