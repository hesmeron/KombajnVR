using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BroadcastSender<T> : BroadcastSender
{
    private BroadcastSubsystem<T> _subsystem;


    public BroadcastSender()
    {
        _subsystem = BroadcastSystem.Instance().SubsystemInstance<T>();
    }

    public void Send(Vector3 position, T target)
    {
       Send(new Broadcast<T>(position, this, target));
    }
    
    public void Send(Broadcast<T> broadcast)
    {
        if (!_broadcasting)
        {
            _broadcasting = true;
            _currentBroadcast = broadcast;
            _subsystem.SendBroadcast(broadcast);
        }
    }

    public void ReleaseBroadcast()
    {
        _broadcasting = false;
        _currentBroadcast = null;
    }


}

public class BroadcastSender
{
    protected bool _broadcasting = false;
    protected Broadcast _currentBroadcast;
    
    public void UpdateBroadcastPosition(Vector3 position)
    {
        if (_broadcasting)
        {
            _currentBroadcast.UpdatePosition(position);
        }
    }
}
public class BroadcastSenderBehaviour : MonoBehaviour
{
    private Dictionary<Type, BroadcastSender> _senders = new Dictionary<Type, BroadcastSender>();
    private Vector3 _lastPosition;
    
    private void Update()
    {
        if (Vector3.Distance(transform.position, _lastPosition) > Single.Epsilon)
        {
            _lastPosition = transform.position;
            foreach (KeyValuePair<Type, BroadcastSender> valuePair in _senders)
            {
                valuePair.Value.UpdateBroadcastPosition(_lastPosition);
            }
        }
    }
    public void Broadcast<T>(T target)
    {
        Type key = typeof(T);
        BroadcastSender<T> broadcastSender;
        if (_senders.ContainsKey(key))
        {
            broadcastSender = _senders[key] as BroadcastSender<T>;
        }
        else
        {
            broadcastSender = new BroadcastSender<T>();
            _senders.Add(key, broadcastSender);
        }
        Assert.IsNotNull(broadcastSender);
        broadcastSender.Send(transform.position, target);
    }
}