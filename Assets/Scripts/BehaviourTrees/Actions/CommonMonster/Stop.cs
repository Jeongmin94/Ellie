using System;
using TheKiwiCoder;

[Serializable]
public class Stop : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.agent.destination = context.transform.position;
        return State.Success;
    }
}