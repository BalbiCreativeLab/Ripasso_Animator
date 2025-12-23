using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent { }

public class GameEventData
{
    public GameObject instigator;
}

public static class GameEventSystem
{
    static Dictionary<Type, Action<GameEventData>> events = new Dictionary<Type, Action<GameEventData>>();

    public static void AddEvent(Type eventType)
    {
        if (events.ContainsKey(eventType))
            return;

        Action<GameEventData> newEvent = delegate {  };
        events.Add(eventType, newEvent);
    }

    public static void RemoveEvent(Type eventType)
    {
        if (!events.ContainsKey(eventType))
            return;

        events.Remove(eventType);
    }

    public static void Subscribe(Type eventType, Action<GameEventData> listener)
    {
        if (events.ContainsKey(eventType))
            events[eventType] += listener;
    }

    public static void Unsubscribe(Type eventType, Action<GameEventData> listener)
    {
        if (events.ContainsKey(eventType))
            events[eventType] -= listener;
    }

    public static void TriggerEvent(Type eventType, GameEventData data)
    {
        if (events.ContainsKey(eventType))
            events[eventType].Invoke(data);
    }
}