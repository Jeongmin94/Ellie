using Codice.CM.Common;
using TheKiwiCoder;
using UnityEngine;

[System.Serializable]
public class RunAwayTarget : ActionNode
{
    public NodeProperty<Transform> target;
    public NodeProperty<float> speed;
    public NodeProperty<float> rayDistance;

    private LayerMask groundLayer;

    protected override void OnStart() {
        groundLayer = LayerMask.GetMask("Ground", "InteractionObject");
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        // 대상과의 반대 방향 계산
        Vector3 direction = (context.transform.position - target.Value.position).normalized;

        RaycastHit hit;
        // 정면 레이캐스트 발사
        if (Physics.Raycast(context.transform.position, context.transform.forward, out hit, rayDistance.Value, groundLayer))
        {
            // 레이캐스트가 Ground 레이어 또는 Wall 태그에 맞으면 실패 반환
            if(hit.collider.CompareTag("Wall"))
            {
                return State.Failure;
            }
        }

        // 대상과 반대 방향으로 이동
        context.transform.position += direction * speed.Value * Time.deltaTime;

        return State.Success;
    }
}
