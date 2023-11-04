using System.Collections;
using TheKiwiCoder;
using UnityEngine;

public class BTTestController : BehaviourTreeController
{
    public BehaviourTreeInstance behaviourTreeInstance;

    private BlackboardKey<int> test;

    private void Start()
    {
        test = behaviourTreeInstance.FindBlackboardKey<int>("test");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            test.Value += 1;
            Debug.Log(test.value);
            Debug.Log("asdf");
        }
    }
}