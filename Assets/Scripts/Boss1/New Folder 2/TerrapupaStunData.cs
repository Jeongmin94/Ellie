using System.Collections;
using TheKiwiCoder;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "Terrapupa/Stun")]
public class TerrapupaStunData : BaseBTData
{
    public TerrapupaStunData()
    {
        dataName = "Stun";
    }

    [Header("기절 상태")]
    [Tooltip("기절 지속시간")] public float stunDuration = 10.0f;

    public BlackboardKey<float> duration;

    public override void Init(BehaviourTree tree)
    {
        Debug.Log(tree);

        SetBlackboardValue<float>("duration", stunDuration, tree);

        duration = FindBlackboardKey<float>("duration", tree);
    }
}