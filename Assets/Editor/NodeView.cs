using System;
using System.Collections;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    // 모든 유형의 노드를 생성하기 위해 GraphView에 있는 Node를 직접 사용하는 것이 아니라
    // 새로 상속받아서 사용한다. (그냥은 namespace에 있는 한정적인 종류만 사용 가능)
    public Action<NodeView> nodeSelectedAction;

    public Node node;
    public Port input;
    public Port output;

    public NodeView(Node node)
    {
        // 노드에 대한 참조를 가져오기
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;   // 노드를 검색할 때 사용

        // 노드의 좌표 지정
        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    // 노드의 엣지를 생성
    private void CreateInputPorts()
    {
        if(node is ActionNode)
        {
            input = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is CompositeNode)
        {
            input = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is DecoratorNode)
        {
            input = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        // 루트 노드는 모든 노드의 최상위 부모 노드이기 때문에, input이 존재하지 않는다.
        else if (node is RootNode)
        {

        }

        if (input != null)
        {
            input.portName = "";
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        // 액션 노드는 자식 노드가 없으므로, 출력이 있어서는 안된다. 공란으로 둠
        if (node is ActionNode)
        {

        }
        // CompositeNode는 자식 노드가 여러개 존재할 수 있으므로, Port.Capacity 타입을 Multi로 설정해둔다.
        else if (node is CompositeNode)
        {
            output = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            output = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            output = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null)
        {
            output.portName = "";
            outputContainer.Add(output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
        base.OnSelected();

        if(nodeSelectedAction != null)
        {
            nodeSelectedAction?.Invoke(this);
        }
    }
}
