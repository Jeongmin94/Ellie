using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckRayCastForward : ActionNode
{
    public NodeProperty<float> rayCastLength;
    public NodeProperty<string> targetTag;
    public NodeProperty<LayerMask> layerMask;

    protected override void OnStart()
    {
        if (layerMask.Value == 0)
            layerMask.Value = -1;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(context.transform.position, context.transform.forward, rayCastLength.Value, layerMask.Value);

        foreach (RaycastHit hit in hits)
        {
            if (targetTag.Value == "" || hit.collider.CompareTag(targetTag.Value))
            {
                return State.Success;
            }
        }
        return State.Failure;
    }
}
