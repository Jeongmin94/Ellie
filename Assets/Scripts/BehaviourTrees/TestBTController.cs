using TheKiwiCoder;
using UnityEngine;

public class TestBTController : MonoBehaviour
{
    public BehaviourTreeInstance behaviourTreeInstance;
    public EnemyData enemyData;

    public int hp;
    public float movement;
    public float attackRange;

    private BlackboardKey<int> monsterHP;
    private BlackboardKey<float> monsterMovement;
    private BlackboardKey<float> monsterAttackRange;



    private void Start()
    {
        behaviourTreeInstance.SetBlackboardValue<int>("monsterHP", enemyData.monsterHP);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterMovement", enemyData.monsterMovement);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterAttackRange", enemyData.monsterAttackRange);

        hp = behaviourTreeInstance.GetBlackboardValue<int>("monsterHP");
        movement = behaviourTreeInstance.GetBlackboardValue<float>("monsterMovement");
        attackRange = behaviourTreeInstance.GetBlackboardValue<float>("monsterAttackRange");

        monsterHP = behaviourTreeInstance.FindBlackboardKey<int>("monsterHP");
        monsterMovement = behaviourTreeInstance.FindBlackboardKey<float>("monsterMovement");
        monsterAttackRange = behaviourTreeInstance.FindBlackboardKey<float>("monsterAttackRange");
    }

    private void Update()
    {
        monsterMovement.value = monsterAttackRange.value;
        monsterAttackRange.value += 0.3f;

    }
}