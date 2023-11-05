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
    public NodeProperty<float> angle;

    public NodeProperty<bool> isCheckWall;
    public NodeProperty<bool> isCheckRootTransform;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 forward = context.transform.forward;
        Collider[] hitColliders = Physics.OverlapSphere(context.transform.position, radius.Value);

        foreach (Collider hitCollider in hitColliders)
        {
            if (IsTargetValid(hitCollider, forward))
            {
                AssignReturnObject(hitCollider);
                return State.Success;
            }
        }

        return State.Failure;
    }

    private bool IsTargetValid(Collider collider, Vector3 forward)
    {
        Vector3 toTarget = (collider.transform.position - context.transform.position).normalized;
        if (Vector3.Angle(forward, toTarget) > angle.Value / 2)
        {
            return false;
        }

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

    private void AssignReturnObject(Collider collider)
    {
        returnObject.Value = isCheckRootTransform.Value ? collider.transform.root : collider.transform;
    }
}
