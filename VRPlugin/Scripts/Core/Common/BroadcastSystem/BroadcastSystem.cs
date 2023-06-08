using System;
using System.Collections.Generic;
using UnityEngine;

public partial class BroadcastSystem : MonoBehaviour
{
    private Dictionary<Type, BroadcastSubsystem> _subsystems = new Dictionary<Type, BroadcastSubsystem>();
    
    public static BroadcastSystem Instance()
    {
        BroadcastSystem broadcastSubsystem = MonoBehaviour.FindObjectOfType<BroadcastSystem>();
        if (broadcastSubsystem)
        {
            return broadcastSubsystem;
        }
        else
        {
            return new GameObject("BroadcastSystem").AddComponent<BroadcastSystem>();
        }
    }
    
    public BroadcastSubsystem<T> SubsystemInstance<T>()
    {
        Type key = typeof(T);
        if (_subsystems.ContainsKey(key))
        {
            return (BroadcastSubsystem<T>) _subsystems[key];
        }
        else
        {
            BroadcastSubsystem<T> subsystem = new BroadcastSubsystem<T>(this);
            _subsystems.Add(key, subsystem);
            return subsystem;
        }
    }
}
