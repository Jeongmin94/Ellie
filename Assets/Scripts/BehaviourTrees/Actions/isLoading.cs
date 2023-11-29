using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Centers.Test;

[System.Serializable]
public class isLoading : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (TestCenterWithScene.Instance.IsLoading)
        {
            return State.Failure;
        }
        else return State.Success;
    }
}
