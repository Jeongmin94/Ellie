using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public State state = State.Running;     // 현재 상태
    public bool isStarted = false;            // 실행 된 적이 있는지? (초기화 함수)

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

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
