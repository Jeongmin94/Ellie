using System;
using System.Collections.Generic;

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
}

public interface IBaseEventPayload
{

}

public class EventBus : Singleton<EventBus>
{
    private Dictionary<EventBusEvents, Delegate> eventTable = new Dictionary<EventBusEvents, Delegate>();

    public void Subscribe(EventBusEvents eventName, Action listener)
    {
        if (!eventTable.ContainsKey(eventName))
            eventTable[eventName] = null;
        eventTable[eventName] = (Action)Delegate.Combine(eventTable[eventName], listener);
    }

    public void Unsubscribe(EventBusEvents eventName, Action listener)
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName] = (Action)Delegate.Remove(eventTable[eventName], listener);
    }

    public void Publish(EventBusEvents eventName)
    {
        if (eventTable.ContainsKey(eventName) && eventTable[eventName] != null)
        {
            ((Action)eventTable[eventName])();
        }
    }

    public void Subscribe<T>(EventBusEvents eventName, Action<T> listener) where T : IBaseEventPayload
    {
        if (!eventTable.ContainsKey(eventName))
            eventTable[eventName] = null;
        eventTable[eventName] = Delegate.Combine(eventTable[eventName], listener);
    }

    public void Unsubscribe<T>(EventBusEvents eventName, Action<T> listener) where T : IBaseEventPayload
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName] = Delegate.Remove(eventTable[eventName], listener);
    }

    public void Publish<T>(EventBusEvents eventName, T newEvent) where T : IBaseEventPayload
    {
        if (eventTable.ContainsKey(eventName) && eventTable[eventName] != null) 
        {
            ((Action<T>)eventTable[eventName])(newEvent);
        }
    }
}