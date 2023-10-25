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

        EventBus.Instance.Subscribe<BaseEventPayload>(EventBusEvents.SpawnStoneEvent, OnTestObj);
        EventBus.Instance.Subscribe(EventBusEvents.TestEvent, OnTest);
        EventBus.Instance.Subscribe<TestEventPayload>(EventBusEvents.ThrowStoneEvent, OnTestInfo);

        EventBus.Instance.Publish<BaseEventPayload>(EventBusEvents.SpawnStoneEvent, temp);
        EventBus.Instance.Publish(EventBusEvents.TestEvent);
        EventBus.Instance.Publish<TestEventPayload>(EventBusEvents.ThrowStoneEvent, new TestEventPayload(5, "호옹이"));
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
        Type type = obj.GetType();
        Debug.Log(type);
        Debug.Log(type.FullName);

        if (type == Type.GetType("TestEventPayload"))
        {
            TestEventPayload test = (TestEventPayload)obj;
            Debug.Log("ㅇㅇ");
            Debug.Log($"{test.health} , {test.name}");
        }
    }
}
