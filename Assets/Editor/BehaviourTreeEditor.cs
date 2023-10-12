using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;

public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTreeView treeView;
    InspectorView inspectorView;

    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    // BehaviourTree ScriptableObject 파일을 더블클릭 하면, 에디터 창이 열리게 설정하기
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if(Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        // UXML 추가
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        // UI의 구성요소들을 설정하기
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();

        // 노드 변경 시의 이벤트 구독
        treeView.nodeSelectedAction = OnNodeSelectionChanged;

        OnSelectionChange();
    }

    // ui가 선택이 변경되었을 때 실행되는 함수
    private void OnSelectionChange()
    {
        // 트리가 활성화 되어있는지, 열 수 있는지 확인하고
        // 활성화 되어 있다면 트리 내부를 채운다
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if(tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
        {
            treeView.PopulateView(tree);
        }
    }

    // 노드 선택이 변경될 때 호출
    private void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateView(node);
    }
}