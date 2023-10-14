using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using System;

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

    // 런타임에서 수정은 가능하되, 그 수정사항이 저장은 되지 않게 설정하기
    // 객체의 복제된 행동트리를 런타임에서 변경사항을 간단하게 테스트 할 수 있게 설정한다.
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange change)
    {
        switch (change)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }


    // ui가 선택이 변경되었을 때 실행되는 함수
    private void OnSelectionChange()
    {
        // 트리가 활성화 되어있는지, 열 수 있는지 확인하고
        // 활성화 되어 있다면 트리 내부를 채운다
        BehaviourTree tree = Selection.activeObject as BehaviourTree;

        if(!tree)
        {
            if (Selection.activeGameObject)
            {
                BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();   
                if(runner != null)
                {
                    tree = runner.tree;
                }
            }
        }

        // 런타임에서도 수정이 가능하게 설정
        if(Application.isPlaying)
        {
            if (tree)
            {
                treeView.PopulateView(tree);
            }
        }
        else
        {
            if(tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView.PopulateView(tree);
            }
        }

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