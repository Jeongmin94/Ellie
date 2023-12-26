using Assets.Scripts.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventBusEvents
{
    None,
    GripStoneByBoss1,
    ThrowStoneByBoss1,
    HitManaByPlayerStone,
    DestroyedManaByBoss1,
    OccurEarthQuake,
    DropMagicStalactite,
    BossAttractedByMagicStone,
    BossUnattractedByMagicStone,
    IntakeMagicStoneByBoss1,
    BossDeath,
    HitStone,
    BossMeleeAttack,
    BossLowAttack,
    ApplyBossCooldown,
    BossMinionAttack,
    DestroyAllManaFountain,
    ApplySingleBossCooldown,
    StartIntakeMagicStone,
    ActivateMagicStone,
}

public interface IBaseEventPayload
{
}

public class EventWrapper
{
    private Action<IBaseEventPayload> actionEvent;

    public void Subscribe(Action<IBaseEventPayload> listener)
    {
        actionEvent -= listener;
        actionEvent += listener;
    }

    public void Invoke(IBaseEventPayload payload) => actionEvent?.Invoke(payload);

    public void Clear() => actionEvent = null;
}

public class EventBus : Singleton<EventBus>
{
    [ShowInInspector][ReadOnly] private Dictionary<EventBusEvents, EventWrapper> eventTable = new Dictionary<EventBusEvents, EventWrapper>();

    public override void Awake()
    {
        base.Awake();
        
        InitEventTable();
    }

    private void InitEventTable()
    {
        eventTable.Clear();
        var eventTypes = Enum.GetValues(typeof(EventBusEvents));
        for (int i = 0; i < eventTypes.Length; i++)
        {
            eventTable.TryAdd((EventBusEvents)eventTypes.GetValue(i), new EventWrapper());
        }
    }

    public override void ClearAction()
    {
        Debug.Log($"{name} ClearAction");

        foreach (var eventName in eventTable.Keys)
        {
            if (eventTable.TryGetValue(eventName, out var wrapper))
            {
                wrapper.Clear();
            }
        }

        InitEventTable();
    }

    public void Subscribe(EventBusEvents eventName, Action<IBaseEventPayload> listener)
    {
        if (eventTable.TryGetValue(eventName, out var wrapper))
        {
            wrapper.Subscribe(listener);
        }
    }

    public void Publish(EventBusEvents eventName, IBaseEventPayload payload)
    {
        if (eventTable.TryGetValue(eventName, out var wrapper))
        {
            wrapper.Invoke(payload);
        }
    }
}