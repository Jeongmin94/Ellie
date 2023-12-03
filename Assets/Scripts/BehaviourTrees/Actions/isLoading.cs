using Assets.Scripts.Centers;
using TheKiwiCoder;
using UnityEngine;

[System.Serializable]
public class isLoading : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        Debug.Log("AA : "+SceneLoadManager.Instance.IsLoading);

        if (SceneLoadManager.Instance.IsLoading)
        {
            return State.Failure;
        }
        else return State.Success;
    }
}
