using UnityEngine;
using TheKiwiCoder;
using static Assets.Scripts.Monsters.Utility.Enums;

[System.Serializable]
public class RunToPlayer : ActionNode
{
    public NodeProperty<GameObject> player;

    private float accumTime;
    protected override void OnStart()
    {
        context.agent.speed = context.controller.attackData[(int)AttackSkill.RunToPlayer].movementSpeed;
        accumTime = 0.0f;
    }

    protected override void OnStop()
    {
        context.agent.speed = context.controller.monsterData.movementSpeed;
    }

    protected override State OnUpdate()
    {
        context.agent.destination = player.Value.transform.position;
        float distance = Vector3.SqrMagnitude(context.agent.destination - context.transform.position);
        if (accumTime < context.controller.attackData[(int)AttackSkill.RunToPlayer].attackDuration)
        {
            if (distance < context.agent.stoppingDistance * context.agent.stoppingDistance)
            {
                return State.Success;
            }
            accumTime += Time.deltaTime;
            return State.Running;
        }
        return State.Success;
    }
}
