using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class WaitWithTimeOut : ActionNode
{
    public NodeProperty<float> timeValue;

    [Tooltip("Amount of time to wait before returning success")] public float duration = 1;
    float startTime;

    protected override void OnStart()
    {
        if (timeValue.Value > 0.0f)
        {
            duration = timeValue.Value;
        }
        startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        float timeRemaining = Time.time - startTime;
        if (timeRemaining > duration)
        {
            if (timeRemaining - duration > 1.0f)
            {
                Debug.Log($"Wait Return Failure :: {timeRemaining - duration}");
                return State.Failure;
            }
            else
            {
                return State.Success;
            }
            return State.Success;
        }
        return State.Running;
    }
}
