using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public Node rootNode;   // 현재 진입 노드
    public Node.State treeState = Node.State.Running;   // 현재 노드의 상태
    public List<Node> nodes = new List<Node>();

    public Node.State Update()
    {
        // 현재 Running 상태일 때만 업데이트가 실행되게
        if(rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
    }

    public Node CreateNode(System.Type type)
    {
        // 스크립터블 오브젝트 인스턴스를 만들고, 설정 후 리스트에 저장
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        // 에셋 폴더에 데이터베이스 추가 후 갱신
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        // 리스트 삭제
        nodes.Remove(node);

        // 에셋 폴더에 데이터베이스 삭제 후 갱신
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }
}
