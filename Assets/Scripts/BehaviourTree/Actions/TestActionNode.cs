using TheKiwiCoder;

[System.Serializable]
public class TestActionNode : ActionNode
{
    public NodeProperty<int> moveSpeed;
    public NodeProperty<string> logString;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        int currentSpeed = moveSpeed.Value;
        currentSpeed += 1;
        logString.Value = currentSpeed.ToString();
        moveSpeed.Value = currentSpeed;

        return State.Success;
    }
}
