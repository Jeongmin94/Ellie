using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckTargetOverlapSphere : ActionNode
{
    public NodeProperty<Transform> returnObject;

    public NodeProperty<LayerMask> layer;
    public NodeProperty<float> radius;
    public NodeProperty<float> angle;
    public NodeProperty<bool> isCheckWall;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        Vector3 forward = context.transform.forward;

        Collider[] hitColliders = Physics.OverlapSphere(context.transform.position, radius.Value, layer.Value);
        foreach (Collider collider in hitColliders)
        {
            Vector3 toTarget = (collider.transform.position - context.transform.position).normalized;

            if (Vector3.Angle(forward, toTarget) <= angle.Value / 2)
            {
                // 광선을 쏴서 중간에 장애물이 있는지 체크
                if (!isCheckWall.Value ||
                    !Physics.Linecast(context.transform.position, collider.transform.position, out RaycastHit hit, layer.Value) ||
                    hit.transform == collider.transform)
                {
                    returnObject.Value = collider.transform;
                    return State.Success;
                }
            }
        }

        return State.Failure;
    }
}
