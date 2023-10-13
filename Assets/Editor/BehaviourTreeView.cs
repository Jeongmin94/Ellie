using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> nodeSelectedAction;

    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    private BehaviourTree tree;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        // 행동 트리 GUI 창에서 되돌리기 시 화면 업데이트 할 수 있게 이벤트 구독
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    // 행동트리 GUI 화면의 초기화 
    public void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // 루트 노드가 없으면 새로 생성한다.
        if(tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;

            // 편집기 유틸리티를 불러와서 파싱. 데이터 갱신
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        // 노드 정보들을 불러와서 생성한다
        foreach (var node in tree.nodes)
        {
            CreateNodeView(node);
        }

        // 노드들의 엣지 정보들을 불러와서 생성한다
        foreach (var node in tree.nodes)
        {
            CreateEdgeView(node);
        }
    }

    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    // startPort와 호환이 되는 포트들의 목록을 반환하는 함수 (호환성 체크)
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        // 입력 포트와 출력 포트의 호환성 판단 (startPort와 endPort가 방향이 달라야 한다)
        // LINQ의 Where()로 조건에 맞는 포트들만 필터링 -> 결과를 리스트로 반환
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    // 포커싱이 다른창으로 넘어갔다가 다시 행동트리 GUI로 넘어올 때 호출
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // 프로젝트에서 다른 파일을 볼 때(포커싱이 다른 창으로 넘어가면) 정보를 갱신시켜줘야 한다.
        // 체크를 해주지 않으면 노드나 엣지가 중복으로 생성된다.
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var elem in graphViewChange.elementsToRemove)
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
            }
        }

        // 엣지가 생성되었을 때 노드의 자식을 추가시켜준다.
        if(graphViewChange.edgesToCreate != null)
        {
            foreach(var edge in graphViewChange.edgesToCreate)
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            }
        }

        return graphViewChange;
    }

    // 행동트리 GUI의 오른쪽 클릭 메뉴를 추가하는 함수
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        {
            // TypeCache로 특정 형태(여기선 ActionNode)에서 파생된 모든 타입을 반환받음
            // ActionNode에서 파생된 클래스들(실제로 동작하는 액션 노드들의 기능)을 표시시키기 위함
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                // 메뉴에 '[기본타입 이름] 파생된 타입 이름' 으로 표기
                // ex : [Action Node] DebugLogNode
                // 메뉴를 왼쪽클릭으로 선택하면 (a) => CreateNode(type) 함수가 호출되어 노드가 생성됨
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
    }

    private void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.nodeSelectedAction = nodeSelectedAction;
        AddElement(nodeView);
    }

    private void CreateEdgeView(Node node)
    {
        var children = tree.GetChildren(node);
        foreach (var child in children)
        {
            NodeView parentView = FindNodeView(node);
            NodeView childView = FindNodeView(child);

            Edge edge = parentView.output.ConnectTo(childView.input);
            AddElement(edge);
        }
    }
}
