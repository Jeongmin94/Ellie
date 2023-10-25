using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestEventPayload : BaseEventPayload
{
    public int health;
    public string name;

    public TestEventPayload(int health, string name)
    {
        this.health = health;
        this.name = name;
    }
}



public class asdfTest : MonoBehaviour
{

    private void Start()
    {
        TestEventPayload temp = new TestEventPayload(100, "테스트");
        object obj = temp;

        EventBus.Instance.Subscribe<BaseEventPayload>(EventBusEvents.GripStoneByBoss1, OnTestObj);
        EventBus.Instance.Subscribe(EventBusEvents.None, OnTest);
        EventBus.Instance.Subscribe<TestEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnTestInfo);

        EventBus.Instance.Publish<BaseEventPayload>(EventBusEvents.GripStoneByBoss1, temp);
        EventBus.Instance.Publish(EventBusEvents.None);
        EventBus.Instance.Publish<TestEventPayload>(EventBusEvents.ThrowStoneByBoss1, new TestEventPayload(5, "호옹이"));
    }

    private void OnTestInfo(TestEventPayload info)
    {
        Debug.Log($"{info.health} , {info.name}");
    }

    private void OnTest()
    {
        Debug.Log("테스트에용");
    }

    private void OnTestObj(BaseEventPayload obj)
    {
        TestEventPayload a = obj as TestEventPayload;
        
        Debug.Log("ㅇㅇ");
        Debug.Log($"{a.health} , {a.name}");
    }
}
