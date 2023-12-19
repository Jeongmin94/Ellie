using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Managers;
using VFXManager = Assets.Scripts.Managers.VFXManager;

[System.Serializable]
public class AddVFXToTarget : ActionNode
{
    public NodeProperty<GameObject> effectPrefab;
    public NodeProperty<Transform> origin;
    public NodeProperty<Vector3> scale;
    public NodeProperty<Vector3> offset;

    public NodeProperty<bool> isFollowOrigin;

    protected override void OnStart() {
        if (origin.Value == null)
            origin.Value = context.transform;

        if (scale.Value == Vector3.zero)
            scale.Value = Vector3.one;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (effectPrefab == null)
            return State.Failure;

        VFXManager.Instance.GetVFX(effectPrefab.Value, new VFXPayload
        {
            Origin = origin.Value,
            IsFollowOrigin = isFollowOrigin.Value,
            Position = origin.Value.transform.position,
            Rotation = origin.Value.transform.rotation,
            Offset = offset.Value,
            Scale = scale.Value,
        });

        return State.Success;
    }
}
