//Written by Arka Deb for publisher-subscriber model


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static Dictionary<object, Dictionary<string, Delegate>> mEventsTable = new Dictionary<object, Dictionary<string, Delegate>>();

    private static Dictionary<string, Delegate> mGlobalEvents = new Dictionary<string, Delegate>();

    private static void RegisterEvent(string eventname, Delegate callback)
    {
        Delegate pCallback;
        if (mGlobalEvents.TryGetValue(eventname, out pCallback))
        {
            mGlobalEvents[eventname] = Delegate.Combine(pCallback, callback);
        }
        else
        {
            mGlobalEvents.Add(eventname, callback);
        }
    }



    private static void UnregisterEvent(string eventname, Delegate callback)
    {
        Delegate pCallback;
        if (mGlobalEvents.TryGetValue(eventname, out pCallback))
        {
            mGlobalEvents[eventname] = Delegate.Remove(pCallback, callback);
        }
    }

    private static void RegisterEvent(object obj, string eventname, Delegate callback)
    {
        if (obj == null)
        {
            Debug.LogError("The object cannot be null while registering event");
            return;
        }

        Dictionary<string, Delegate> callbacks;
        if (!mEventsTable.TryGetValue(obj, out callbacks))
        {
            callbacks = new Dictionary<string, Delegate>();
            mEventsTable.Add(obj, callbacks);
        }

        Delegate pCallback;
        if (callbacks.TryGetValue(eventname, out pCallback))
        {
            callbacks[eventname] = Delegate.Combine(pCallback, callback);
        }
        else
        {
            callbacks.Add(eventname, callback);
            Debug.Log(eventname + " registered for " + obj);
        }
    }

    private static void UnregisterEvent(object obj, string eventname, Delegate callback)
    {
        if (obj == null)
        {
            Debug.LogError("The object cannot be null while registering event");
            return;
        }

        Dictionary<string, Delegate> callbacks;
        if (mEventsTable.TryGetValue(obj, out callbacks))
        {
            Delegate pcallback;
            if (callbacks.TryGetValue(eventname, out pcallback))
            {
                callbacks[eventname] = Delegate.Remove(pcallback, callback);
            }
        }
    }

    public static void RegisterEvent(string eventname, Action callback)
    {
        RegisterEvent(eventname, (Delegate)callback);
    }

    public static void UnregisterEvent(string eventname, Action callback)
    {
        UnregisterEvent(eventname, (Delegate)callback);
    }

    public static void RegisterEvent(object obj, string eventname, Action callback)
    {
        RegisterEvent(obj, eventname, (Delegate)callback);
    }
    public static void UnregisterEvent(object obj, string eventname, Action callback)
    {
        UnregisterEvent(obj, eventname, (Delegate)callback);
    }

    public static void RegisterEvent<T>(string eventname, Action<T> callback)
    {
        RegisterEvent(eventname, (Delegate)callback);
    }

    public static void UnregisterEvent<T>(string eventname, Action<T> callback)
    {
        UnregisterEvent(eventname, (Delegate)callback);
    }

    public static void RegisterEvent<T1, T2>(string eventname, Action<T1, T2> callback)
    {
        RegisterEvent(eventname, (Delegate)callback);
    }

    public static void UnregisterEvent<T1, T2>(string eventname, Action<T1, T2> callback)
    {
        UnregisterEvent(eventname, (Delegate)callback);
    }

    public static void RegisterEvent<T>(object obj, string eventname, Action<T> callback)
    {
        RegisterEvent(obj, eventname, (Delegate)callback);
    }

    public static void UnregisterEvent<T>(object obj, string eventname, Action<T> callback)
    {
        UnregisterEvent(obj, eventname, (Delegate)callback);
    }

    public static void RegisterEvent<T1, T2>(object obj, string eventname, Action<T1, T2> callback)
    {
        RegisterEvent(obj, eventname, (Delegate)callback);
    }

    public static void UnregisterEvent<T1, T2>(object obj, string eventname, Action<T1, T2> callback)
    {
        UnregisterEvent(obj, eventname, (Delegate)callback);
    }

    public static void ExecuteEvent(string eventname)
    {
        Action callback = GetDelegate(eventname) as Action;
        if (callback != null)
        {
            callback();
        }
    }

    public static void ExecuteEvent<T>(string eventname, T param)
    {
        Action<T> callback = GetDelegate(eventname) as Action<T>;
        if (callback != null)
        {
            callback(param);
        }
    }

    public static void ExecuteEvent<T1, T2>(string eventname, T1 param1, T2 param2)
    {
        Action<T1, T2> callback = GetDelegate(eventname) as Action<T1, T2>;
        if (callback != null)
        {
            callback(param1, param2);
        }
    }

    public static void ExecuteEvent(object obj, string eventname)
    {
        var callback = GetDelegate(obj, eventname) as Action;
        if (callback != null)
        {
            Debug.Log("Here");
            callback();
        }
    }

    public static void ExecuteEvent<T>(object obj, string eventname, T param)
    {
        var callback = GetDelegate(obj, eventname) as Action<T>;
        if (callback != null)
        {
            callback(param);
        }
    }

    public static void ExecuteEvent<T1, T2>(object obj, string eventname, T1 param1, T2 param2)
    {
        Action<T1, T2> callback = GetDelegate(obj, eventname) as Action<T1, T2>;
        if (callback != null)
        {
            callback(param1, param2);
        }
    }




    private static Delegate GetDelegate(string eventname)
    {
        Delegate callback;
        if (mGlobalEvents.TryGetValue(eventname, out callback))
        {
            return callback;
        }
        return null;
    }

    private static Delegate GetDelegate(object obj, string eventname)
    {
        Dictionary<string, Delegate> callbacks;
        if (mEventsTable.TryGetValue(obj, out callbacks))
        {
            Delegate callback;
            if (callbacks.TryGetValue(eventname, out callback))
            {
                return callback;
            }
        }

        return null;
    }

}

