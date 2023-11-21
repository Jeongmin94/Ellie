using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Particle;

[System.Serializable]
public class EarthQuakeParticle : ActionNode
{
    public NodeProperty<GameObject> effectPrefab;
    public NodeProperty<Vector3> scale;

    public NodeProperty<float> forwardOffset;
    public NodeProperty<float> angle;
    public NodeProperty<int> particleCount;

    protected override void OnStart()
    {
        if (scale.Value == Vector3.zero)
            scale.Value = Vector3.one;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (effectPrefab == null)
            return State.Failure;

        // 시작 위치 계산 (transform의 앞쪽 방향에 forwardOffset만큼 이동)
        Vector3 startPos = context.transform.position + (context.transform.forward * forwardOffset.Value);
        Quaternion startRotation = context.transform.rotation;

        // 각 파티클에 대한 각도 계산
        float totalAngle = angle.Value;
        float angleStep = totalAngle / (particleCount.Value - 1);

        for (int i = 0; i < particleCount.Value; i++)
        {
            // 현재 파티클의 각도 계산
            float currentAngle = -totalAngle / 2 + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0) * context.transform.rotation;

            // 파티클 생성 (모든 파티클은 startPos에서 시작)
            CreateParticle(startPos, rotation);
        }

        return State.Success;
    }

    private void CreateParticle(Vector3 position, Quaternion rotation)
    {
        ParticleManager.Instance.GetParticle(effectPrefab.Value, new ParticlePayload
        {
            Position = position,
            Rotation = rotation,
            Scale = scale.Value,
        });
    }
}
