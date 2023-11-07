using TheKiwiCoder;

[System.Serializable]
public class SetRigidbodyKinematic : ActionNode
{
    public NodeProperty<bool> isKinematic;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(isKinematic.Value)
        {
            context.physics.isKinematic = true;
        }
        else
        {
            context.physics.isKinematic = false;
        }
        return State.Success;
    }
}
