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
        // 값 등록
        behaviourTreeInstance.SetBlackboardValue<int>("monsterHP", enemyData.monsterHP);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterMovement", enemyData.monsterMovement);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterAttackRange", enemyData.monsterAttackRange);

        // 값 가져오기
        hp = behaviourTreeInstance.GetBlackboardValue<int>("monsterHP");
        movement = behaviourTreeInstance.GetBlackboardValue<float>("monsterMovement");
        attackRange = behaviourTreeInstance.GetBlackboardValue<float>("monsterAttackRange");

        // 참조값 저장
        monsterHP = behaviourTreeInstance.FindBlackboardKey<int>("monsterHP");
        monsterMovement = behaviourTreeInstance.FindBlackboardKey<float>("monsterMovement");
        monsterAttackRange = behaviourTreeInstance.FindBlackboardKey<float>("monsterAttackRange");
    }

    private void Update()
    {
        // 참조값 이용해서 MonoBehaviour에서 값 가져오거나, 세팅 가능
        monsterMovement.value = monsterAttackRange.value;
        monsterAttackRange.value += 0.3f;
    }
}