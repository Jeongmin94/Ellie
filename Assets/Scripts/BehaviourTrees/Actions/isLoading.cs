using System;
using Managers.SceneLoad;
using TheKiwiCoder;

[Serializable]
public class isLoading : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (SceneLoadManager.Instance.IsLoading)
        {
            return State.Failure;
        }

        return State.Success;
    }
}