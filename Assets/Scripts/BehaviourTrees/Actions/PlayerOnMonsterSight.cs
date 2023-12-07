using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PlayerOnMonsterSight : ActionNode
{
    public NodeProperty<Vector3> playerPos;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 interV = playerPos.Value - context.transform.position;

        float dot = Vector3.Dot(interV.normalized, context.transform.forward.normalized);
        float theta = Mathf.Acos(dot);
        float degree = Mathf.Rad2Deg * theta;
        Debug.Log("Degree : " + degree);

        if(degree<=140.0f/2)
        {
            return State.Success;
        }

        return State.Failure;
    }
}
