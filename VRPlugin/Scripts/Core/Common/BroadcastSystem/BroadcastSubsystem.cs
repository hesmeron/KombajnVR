using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BroadcastSubsystem
{
    protected  bool _broadcasting = false;
    protected BroadcastSystem _system;

    public abstract void DrawGizmos();
}
public class BroadcastSubsystem<T> : BroadcastSubsystem
{
    private readonly List<BroadcastReceiver<T>> _receivers = new List<BroadcastReceiver<T>>();
    private readonly ManagedList<Broadcast<T>> _broadcasts = new ManagedList<Broadcast<T>>();

    public BroadcastSubsystem(BroadcastSystem system)
    {
        _system = system;
        _broadcasts.OnFirstElementAdded += BroadcastsOnFirstElementAdded;
        _broadcasts.OnEmptied += BroadcastsOnEmptied;
    }

    private void BroadcastsOnFirstElementAdded()
    {
        _broadcasting = true;
        _system.StartCoroutine(EvaluateLoop());
    }

    private void BroadcastsOnEmptied()
    {
        _broadcasting = false;
    }

    public void SendBroadcast(Broadcast<T> broadcast)
    {
        _broadcasts.Add(broadcast);
    }

    public void AddReceiver(BroadcastReceiver<T> receiver)
    {
        _receivers.Add(receiver);
    }
    
    public void RemoveReceiver(BroadcastReceiver<T> receiver)
    {
        _receivers.Remove(receiver);
    }

    private void EvaluateSystem()
    {
        List<Broadcast<T>> toRemove = new List<Broadcast<T>>();
        foreach (BroadcastReceiver<T> receiver in _receivers)
        {
            foreach (Broadcast<T> broadcast in _broadcasts)
            {
                if (receiver.TryReceiveBroadcast(broadcast))
                {
                    toRemove.Add(broadcast);
                }
            }
        }

        foreach (Broadcast<T> broadcast in toRemove)
        {
            _broadcasts.Remove(broadcast);
            broadcast.ReleaseBroadcast();
        }
    }

    IEnumerator EvaluateLoop()
    {
        _broadcasting = true;
        while (_broadcasting)
        {
            EvaluateSystem();
            yield return null;
        }
    }

    public override void DrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Broadcast broadcast in _broadcasts)
        {
            Gizmos.DrawSphere(broadcast.Position, 0.1f);
        }
    }
}
