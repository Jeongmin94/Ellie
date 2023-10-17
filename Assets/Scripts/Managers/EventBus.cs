using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : Singleton<EventBus>
{
    private Dictionary<Type, List<Delegate>> eventTable = new Dictionary<Type, List<Delegate>>();

    public void Subscribe<T>(Action<T> del) where T : class
    {
        Type type = typeof(T);

        if (!eventTable.ContainsKey(type))
        {
            eventTable[type] = new List<Delegate>();
        }

        eventTable[type].Add(del);
    }

    public void Unsubscribe<T>(Action<T> del) where T : class
    {
        Type type = typeof(T);

        if (eventTable.ContainsKey(type))
        {
            eventTable[type].Remove(del);

            if (eventTable[type].Count == 0)
            {
                eventTable.Remove(type);
            }
        }
    }

    public void Publish<T>(T eventToPublish) where T : class
    {
        Type type = typeof(T);

        if (eventTable.ContainsKey(type))
        {
            foreach (Delegate del in eventTable[type])
            {
                del.DynamicInvoke(eventToPublish);
            }
        }
    }
}