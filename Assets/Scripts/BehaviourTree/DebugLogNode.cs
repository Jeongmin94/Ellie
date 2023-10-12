using UnityEngine;

// 디버그 메세지를 반환하는 액션 노드
public class DebugLogNode : ActionNode
{
    public string message;

    protected override void OnStart()
    {
        Debug.Log($"OnStart :: {message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"OnStop :: {message}");
    }

    protected override State OnUpdate()
    {
        Debug.Log($"OnUpdate :: {message}");
        return State.Success;
    }
}
