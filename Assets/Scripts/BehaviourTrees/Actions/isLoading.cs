using Assets.Scripts.Centers;
using TheKiwiCoder;

[System.Serializable]
public class isLoading : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (SceneLoadManager.Instance.IsLoading)
        {
            return State.Failure;
        }
        else return State.Success;
    }
}
