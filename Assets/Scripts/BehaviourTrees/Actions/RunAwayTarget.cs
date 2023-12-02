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
        // ������ �ݴ� ���� ���
        Vector3 direction = (context.transform.position - target.Value.position).normalized;

        RaycastHit hit;
        // ���� ����ĳ��Ʈ �߻�
        if (Physics.Raycast(context.transform.position, context.transform.forward, out hit, rayDistance.Value, groundLayer))
        {
            // ����ĳ��Ʈ�� Ground ���̾� �Ǵ� Wall �±׿� ������ ���� ��ȯ
            if(hit.collider.CompareTag("Wall"))
            {
                return State.Failure;
            }
        }

        // ���� �ݴ� �������� �̵�
        context.transform.position += direction * speed.Value * Time.deltaTime;

        return State.Success;
    }
}
