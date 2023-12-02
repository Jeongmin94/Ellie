using TheKiwiCoder;
using Assets.Scripts.Monsters.AbstractClass;
using static Assets.Scripts.Monsters.Utility.Enums;

[System.Serializable]
public class PerformAttack : ActionNode
{
    public NodeProperty<AttackSkill> skillType;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if(context.controller.Attacks.TryGetValue(context.controller.attackData[(int)skillType.Value].attackName, out AbstractAttack atk))
        {
            atk.ActivateAttack();
            return State.Success;
        }

        return State.Failure;
    }
}
