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

        EventBus.Instance.Subscribe<IBaseEventPayload>(EventBusEvents.GripStoneByBoss1, OnTestObj);
        EventBus.Instance.Subscribe(EventBusEvents.None, OnTest);
        EventBus.Instance.Subscribe<TestEventPayload>(EventBusEvents.ThrowStoneByBoss1, OnTestInfo);

        EventBus.Instance.Publish<IBaseEventPayload>(EventBusEvents.GripStoneByBoss1, temp);
        EventBus.Instance.Publish(EventBusEvents.None);
        EventBus.Instance.Publish<TestEventPayload>(EventBusEvents.ThrowStoneByBoss1, new TestEventPayload(5, "ȣ����"));
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

        Debug.Log("����");
        Debug.Log($"{a.health} , {a.name}");
    }
}
