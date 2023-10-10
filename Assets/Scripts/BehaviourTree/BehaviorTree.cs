using UnityEngine;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public Node rootNode;   // 현재 진입 노드
    public Node.State treeState = Node.State.Running;   // 현재 노드의 상태

    public Node.State Update()
    {
        // 현재 Running 상태일 때만 업데이트가 실행되게
        if(rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
    }
}
