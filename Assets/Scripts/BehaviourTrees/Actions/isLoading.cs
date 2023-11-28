using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Loading;

[System.Serializable]
public class isLoading : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (AsyncLoadManager.Instance.CheckIsLoading())
        {
            return State.Failure;
        }
        else return State.Success;
    }
}
