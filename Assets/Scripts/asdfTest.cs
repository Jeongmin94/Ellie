using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestEventPayload : IBaseEventPayload
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
        TestEventPayload temp = new TestEventPayload(100, "�׽�Ʈ");
        object obj = temp;

        EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.SpawnStoneEvent, OnTestObj);
        EventBus.Instance.Subscribe(EventBusEvents.TestEvent, OnTest);
        EventBus.Instance.Subscribe<TestEventPayload>(EventBusEvents.ThrowStoneEvent, OnTestInfo);

        EventBus.Instance.Publish<IBaseEventPayload>(EventBusEvents.SpawnStoneEvent, temp);
        EventBus.Instance.Publish(EventBusEvents.TestEvent);
        EventBus.Instance.Publish<TestEventPayload>(EventBusEvents.ThrowStoneEvent, new TestEventPayload(5, "ȣ����"));
    }

    private void OnTestInfo(TestEventPayload info)
    {
        Debug.Log($"{info.health} , {info.name}");
    }

    private void OnTest()
    {
        Debug.Log("�׽�Ʈ����");
    }

    private void OnTestObj(IBaseEventPayload obj)
    {
        TestEventPayload a = obj as TestEventPayload;
        Type type = obj.GetType();
        Debug.Log(type);
        Debug.Log(type.FullName);

        if (type == Type.GetType("TestEventPayload"))
        {
            TestEventPayload test = (TestEventPayload)obj;
            Debug.Log("����");
            Debug.Log($"{test.health} , {test.name}");
        }
    }
}
