using System.Collections;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // 현재 내 자식 노드를 반복시키는 노드
        // 설정에 따라 무한 루프도, 특정 횟수만 반복시킬 수도 있음

        child.Update();
        return State.Running;
    }
}