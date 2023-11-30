using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Centers.Test;
using Assets.Scripts.Centers;

[System.Serializable]
public class isLoading : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (SceneCenter.Instance.IsLoading)
        {
            return State.Failure;
        }
        else return State.Success;
    }
}
