using System;
using UnityEngine;

public class BroadcastReceiver<T>
{
    public event Action<T> OnBroadcastReceived;
    
    
    private InteractionBounds _interactionBounds;

    public BroadcastReceiver(InteractionBounds interactionBounds)
    {
        _interactionBounds = interactionBounds;
        BroadcastSystem.Instance().SubsystemInstance<T>().AddReceiver(this);
    }

    public bool TryReceiveBroadcast(Broadcast<T> broadcast)
    {
        Debug.Log("Try receive Broadcast" + broadcast.Position + " " + _interactionBounds.transform.position);
        if (_interactionBounds.IsWithinReach(broadcast.Position))
        {
            Debug.Log("receive Broadcast");
            OnBroadcastReceived?.Invoke(broadcast.Target);
            return true;
        }
        return false;
    }
}
