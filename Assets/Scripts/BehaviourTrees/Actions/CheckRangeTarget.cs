using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckRangeTarget : ActionNode
{
    public NodeProperty<BaseRange> range;
    public NodeProperty<Transform> target;

    public NodeProperty<string> checkTag;
    public NodeProperty<LayerMask> checkLayer;

    public NodeProperty<bool> isRootTransform;

    protected override void OnStart()
    {
        if (checkLayer.Value == 0)
            checkLayer.Value = -1;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (range.Value == null)
        {
            target = null;
            return State.Failure;
        }

        List<Transform> targets = range.Value.CheckRange(checkTag.Value, checkLayer.Value);
        if(targets == null)
            return State.Failure;


        if (targets.Count == 0)
        {
            if(target != null)
            {
                target.Value = null;
            }
            return State.Success;
        }
        foreach (var item in targets)
        {
            Debug.Log(item);
        }

        foreach (var item in targets)
        {
            if (item.root == context.transform.root)
                continue;

            if (isRootTransform.Value)
            {
                target.Value = item.root;
            }
            else
            {
                target.Value = item;
            }
            break;
        }

        return State.Success;
    }
}
