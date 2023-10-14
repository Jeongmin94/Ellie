using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    [HideInInspector] public State state = State.Running;     // 현재 상태
    [HideInInspector] public bool isStarted = false;          // 실행 된 적이 있는지? (초기화 함수)
    [HideInInspector] public Vector2 position;                // GUI에서 노드의 위치를 저장
    [HideInInspector] public string guid;                     // 진입 키
    [HideInInspector] public Blackboard blackboard;           // 블랙보드
    [TextArea] public string description;                     // 노드 주석 달기

    // 세가지 상태 중 하나를 반환한다
    public State Update()
    {
        // 노드의 최초 진입 시, OnStart(); 메서드로 초기화
        if(!isStarted)
        {
            OnStart();
            isStarted = true;
        }

        state = OnUpdate();

        // 노드를 빠져나갈 때, 노드의 상태를 정리해줄 OnStop();
        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            isStarted = false;
        }

        return state;
    }

    // 여러개의 오브젝트가 하나의 행동트리를 참조할 수 있으므로
    // 같은 부분을 참조시키지 않게 개별적으로 복제해서 생성해준다
    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
