using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Channels.Boss;

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

        EventBus.Instance.Subscribe(EventBusEvents.GripStoneByBoss1, OnTestObj);
        EventBus.Instance.Subscribe(EventBusEvents.None, OnTest);
        EventBus.Instance.Subscribe(EventBusEvents.ThrowStoneByBoss1, OnTestInfo);

        EventBus.Instance.Publish(EventBusEvents.GripStoneByBoss1, temp);
        EventBus.Instance.Publish(EventBusEvents.None, new TestEventPayload(5, string.Empty));
        EventBus.Instance.Publish(EventBusEvents.ThrowStoneByBoss1, new TestEventPayload(5, "ȣ����"));
    }

    private void OnTestInfo(IBaseEventPayload payload)
    {
        if (payload is not TestEventPayload info)
            return;
        Debug.Log($"{info.health} , {info.name}");
    }

    private void OnTest(IBaseEventPayload payload)
    {
        if (payload is not TestEventPayload info)
            return;
        Debug.Log("�׽�Ʈ����");
    }

    private void OnTestObj(IBaseEventPayload obj)
    {
        TestEventPayload a = obj as TestEventPayload;

        Debug.Log("����");
        Debug.Log($"{a.health} , {a.name}");
    }
}
