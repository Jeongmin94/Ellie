using System;
using TheKiwiCoder;
using static Monsters.Utility.Enums;

[Serializable]
public class PerformAttack : ActionNode
{
    public NodeProperty<AttackSkill> skillType;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.controller.Attacks.TryGetValue(context.controller.attackData[(int)skillType.Value].attackName, out var atk))
        {
            atk.ActivateAttack();
            return State.Success;
        }

        return State.Failure;
    }
}