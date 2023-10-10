using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AND 연산 -> 자식 노드를 순차적으로 실행한다
// 자식 노드중에 하나라도 실패하면 걍 다실패
public class SequenceNode : CompositeNode
{
    int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // 자식 노드가 성공을 반환하면, 그 다음 노드로 순서가 넘어감
        var child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                current++;
                break;
        }

        // 모든 자식 노드가 성공을 반환했다면 성공 반환 (다음 노드로)
        // 아니라면 현재 자식노드가 성공을 반환시킬 때까지 지속됨
        return current == children.Count ? State.Success : State.Running;
    }
}
