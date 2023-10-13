using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    Editor editor;

    public InspectorView()
    {

    }

    public void UpdateView(NodeView nodeView)
    {
        // 인스펙터 창의 요소들 제거
        Clear();

        // editor 인스턴스를 제거 (초기화)
        UnityEngine.Object.DestroyImmediate(editor);

        // 현재 선택한 노드의 정보를 가져와서 인스펙터 창에 구성(editor 할당)
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() => 
        { 
            // 인스펙터가 사라진 노드를 참조시키지 않게
            if(editor.target)
            {
                editor.OnInspectorGUI();
            }
        });
        Add(container);
    }
}