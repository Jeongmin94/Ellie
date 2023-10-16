using TheKiwiCoder;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public BehaviourTreeInstance behaviourTreeInstance;
    public EnemyData enemyData;

    private BlackboardKey<int> monsterHP;
    private BlackboardKey<float> monsterMovement;
    private BlackboardKey<float> monsterAttackRange;

    [SerializeField] private int hp;
    [SerializeField] private float movement;
    [SerializeField] private float attackRange;

    private void Start()
    {
        // 값 등록
        behaviourTreeInstance.SetBlackboardValue<int>("monsterHP", enemyData.monsterHP);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterMovement", enemyData.monsterMovement);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterAttackRange", enemyData.monsterAttackRange);

        // 값 가져오기
        hp = behaviourTreeInstance.GetBlackboardValue<int>("monsterHP");
        movement = behaviourTreeInstance.GetBlackboardValue<float>("monsterHP");
        attackRange = behaviourTreeInstance.GetBlackboardValue<float>("monsterHP");

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
