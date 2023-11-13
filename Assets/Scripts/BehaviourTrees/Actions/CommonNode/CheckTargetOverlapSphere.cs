using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckTargetOverlapSphere : ActionNode
{
    public NodeProperty<Transform> returnObject;

    public NodeProperty<LayerMask> layer;
    public NodeProperty<string> tag;

    public NodeProperty<float> radius;

    public NodeProperty<bool> isCheckWall;
    public NodeProperty<bool> isCheckRootTransform;

    protected override void OnStart()
    {
        returnObject.Value = null;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(context.transform.position, radius.Value);

        foreach (Collider hitCollider in hitColliders)
        {
            if (IsTargetValid(hitCollider))
            {
                returnObject.Value = isCheckRootTransform.Value ? hitCollider.transform.root : hitCollider.transform;
                return State.Success;
            }
        }

        return State.Failure;
    }

    private bool IsTargetValid(Collider collider)
    {
        if (!string.IsNullOrEmpty(tag.Value) && !collider.CompareTag(tag.Value))
        {
            return false;
        }

        if (layer.Value != 0 && ((1 << collider.gameObject.layer) & layer.Value) == 0)
        {
            return false;
        }

        return !isCheckWall.Value || ClearPathToTarget(collider);
    }

    private bool ClearPathToTarget(Collider collider)
    {
        return !Physics.Linecast(context.transform.position, collider.transform.position, out RaycastHit hit) ||
               hit.collider == collider ||
               (layer.Value == 0 || (hit.collider.gameObject.layer == Mathf.Log(layer.Value, 2)));
    }
}
