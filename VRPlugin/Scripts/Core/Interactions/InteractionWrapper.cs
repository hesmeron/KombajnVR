using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Railroaded,
    Simple
}
[System.Serializable]
class InteractionWrapper<T> where T : ControllerData
{
    [SerializeField]
    private T _interactionData;

    public Interaction GetInteraction(InteractionController owner)
    {
        return _interactionData.GetInteraction(owner);
    }
}

[System.Serializable]
class InteractionWrapper
{
    [SerializeField] 
    private InteractionType _interactionType;

    [SerializeField]
    private RailroadedGrabController.RailroadedData _railroadedData;
    //private InteractionWrapper<InteractionData> _interactionWrapper;

    public Interaction GetInteraction(InteractionController owner)
    {
        switch (_interactionType)
        {
            case InteractionType.Railroaded:
                //_interactionWrapper = new InteractionWrapper<RailroadedGrabController.RailroadedData>();
                return _railroadedData.GetInteraction(owner);
                break;
        }

        return null;
    }
}
