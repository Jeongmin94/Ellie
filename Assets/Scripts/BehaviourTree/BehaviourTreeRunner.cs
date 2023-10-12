using UnityEngine;

// (테스트용) 실제로 행동트리 패턴들을 구현할 대상 오브젝트
public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    private void Start()
    {
        tree = tree.Clone();
    }

    private void Update()
    {
        tree.Update();
    }
}
