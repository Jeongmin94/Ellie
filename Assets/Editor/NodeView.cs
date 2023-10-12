using System.Collections;
using UnityEngine;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    // 모든 유형의 노드를 생성하기 위해 GraphView에 있는 Node를 직접 사용하는 것이 아니라
    // 새로 상속받아서 사용한다. (그냥은 namespace에 있는 한정적인 종류만 사용 가능)
    public Node node;
    public NodeView(Node node)
    {
        // 노드에 대한 참조를 가져오기
        this.node = node;
        this.title = node.name;
    }
}
