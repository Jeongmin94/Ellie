using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckRayCastForward : ActionNode
{
    public NodeProperty<float> rayCastLength;
    public NodeProperty<string> targetTag;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(context.transform.position, context.transform.forward, out hit, rayCastLength.Value))
        {
            if (targetTag.Value == null || hit.collider.CompareTag(targetTag.Value))
            {
                return State.Success;
            }
        }
        return State.Failure;
    }
}
