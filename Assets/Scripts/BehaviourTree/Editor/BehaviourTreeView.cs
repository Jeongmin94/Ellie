using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    public BehaviourTreeView()
    {
        // 그리드 백그라운드 생성
        Insert(0, new GridBackground());

        // 4가지 조작기 추가하기
        this.AddManipulator(new ContentZoomer());           // 확대, 축소
        this.AddManipulator(new ContentDragger());          // 그래프 창을 이동
        this.AddManipulator(new SelectionDragger());        // 노드 선택 드래그
        this.AddManipulator(new RectangleSelector());       // 다중 선택 드래그

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviourTree/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }
}
