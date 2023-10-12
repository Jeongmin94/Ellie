using System.Collections;
using UnityEngine;

// 루트 노드는 행동트리의 가장 높은 단계의 노드
// 부모 노드가 존재하지 않음, 행동트리의 진입 노드
public class RootNode : Node
{
    [HideInInspector] public Node child;

    protected override void OnStart()
    {
        
    }


    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return child.Update();
    }
}
