using System;
using System.Collections;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    // 모든 유형의 노드를 생성하기 위해 GraphView에 있는 Node를 직접 사용하는 것이 아니라
    // 새로 상속받아서 사용한다. (그냥은 namespace에 있는 한정적인 종류만 사용 가능)
    public Action<NodeView> nodeSelectedAction;

    public Node node;
    public Port input;
    public Port output;

    // NodeView.uxml을 생성자에서 전달 -> UI의 스타일을 변경시키는 설정값
    public NodeView(Node node) : base("Assets/Editor/NodeView.uxml")
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
        SetupClasses();
    }

    private void SetupClasses()
    {
        if (node is ActionNode)
        {
            AddToClassList("action");
        }
        else if (node is CompositeNode)
        {
            AddToClassList("composite");
        }
        else if (node is DecoratorNode)
        {
            AddToClassList("decorator");
        }
        else if (node is RootNode)
        {
            AddToClassList("root");
        }
    }

    // 노드의 엣지를 생성
    private void CreateInputPorts()
    {
        if(node is ActionNode)
        {
            input = InstantiatePort(Orientation.Vertical, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is CompositeNode)
        {
            input = InstantiatePort(Orientation.Vertical, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is DecoratorNode)
        {
            input = InstantiatePort(Orientation.Vertical, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        // 루트 노드는 모든 노드의 최상위 부모 노드이기 때문에, input이 존재하지 않는다.
        else if (node is RootNode)
        {

        }

        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
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
            output = InstantiatePort(Orientation.Vertical, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            output = InstantiatePort(Orientation.Vertical, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            output = InstantiatePort(Orientation.Vertical, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        Undo.RecordObject(node, "Behaviour Tree (Set Position)");

        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;

        EditorUtility.SetDirty(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();

        if(nodeSelectedAction != null)
        {
            nodeSelectedAction?.Invoke(this);
        }
    }

    // 노드의 순서를 정렬하는 함수
    // 왼쪽 -> 오른쪽 순서로, GUI에서 같은 높이의 노드의 순서를 설정한다
    public void SortChildren()
    {
        CompositeNode composite = node as CompositeNode;
        if(composite != null)
        {
            composite.children.Sort(SortByHorizontalPosition);
        }
    }

    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.position.x < right.position.x ? -1 : 1;
    }
}
