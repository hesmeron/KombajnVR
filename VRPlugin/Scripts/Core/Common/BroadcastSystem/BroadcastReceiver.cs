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
        if (!_interactionBounds)
        {
            BroadcastSystem.Instance().SubsystemInstance<T>().RemoveReceiver(this);
            return false;
        }
        if (_interactionBounds.IsWithinReach(broadcast.Position))
        {
            OnBroadcastReceived?.Invoke(broadcast.Target);
            return true;
        }
        return false;
    }
}
