using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ActionNode : Node
{
    // 액션 노드는 Clone()을 오버라이딩 받을 필요는 없다
    // 자식 노드가 없기 때문에 Node()의 기본 Clone() 메서드를 사용하면 됨
}