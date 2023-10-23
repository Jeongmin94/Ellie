using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

[System.Serializable]
public class PublishEvent : ActionNode
{
    public NodeProperty<string> eventName;

    private Type type;

    protected override void OnStart() {
        type = Type.GetType(eventName.Value);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(type == null)
        {
            Debug.Log($"{eventName} 이벤트는 존재하지 않습니다.");
            return State.Failure;
        }

        return State.Success;
    }
}
