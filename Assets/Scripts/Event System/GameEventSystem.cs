using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEvent
{
    public Action<GameEventData> listeners;
    public GameEventData eventData;

    public void Invoke(GameEventData data)
    {
        listeners.Invoke(data);
    }
}

public class GameEventData
{
    public GameObject instigator;
}

//public static class GameEventSystem
//{
//    static List<GameEvent> events;

//    public static void AddEvent(GameEvent gameEvent)
//    {
//        if(!events.Contains(gameEvent))
//            events.Add(gameEvent);
//    }
//    public static void RemoveEvent(GameEvent gameEvent)
//    {
//        if (events.Contains(gameEvent))
//            events.Remove(gameEvent);
//    }
//    public static void Subscribe(GameEvent gameEvent, Action<GameEventData> listener) {
//        if (!events.Contains(gameEvent))
//            gameEvent.listeners += listener;
//    }
//    public static void Unsubscribe(GameEvent gameEvent, Action<GameEventData> listener) {
//        if (events.Contains(gameEvent))
//            gameEvent.listeners -= listener;
//    }

//    public static void TriggerEvent(GameEvent gameEvent, GameEventData data)
//    {
//        if (events.Contains(gameEvent))
//            gameEvent.Invoke(data);
//    }
//}


public static class GameEventSystem
{
    static Dictionary<string, Action<GameEventData>> events = new Dictionary<string, Action<GameEventData>>();


    public static void AddEvent(string eventName)
    {
        if (events.ContainsKey(eventName))
            return;

        Action<GameEventData> newEvent = delegate {  };
        events.Add(eventName, newEvent);
    }

    public static void RemoveEvent(string eventName)
    {
        if (!events.ContainsKey(eventName))
            return;

        events.Remove(eventName);
    }

    public static void Subscribe(string eventName, Action<GameEventData> listener)
    {
        if (events.ContainsKey(eventName))
            events[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action<GameEventData> listener)
    {
        if (events.ContainsKey(eventName))
            events[eventName] -= listener;
    }

    public static void TriggerEvent(string eventName, GameEventData data)
    {
        if (events.ContainsKey(eventName))
            events[eventName].Invoke(data);
    }
}