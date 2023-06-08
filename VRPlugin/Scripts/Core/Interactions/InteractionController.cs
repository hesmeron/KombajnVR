using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class InteractionController : MonoBehaviour
{
    [SerializeField]
    private List<InteractionWrapper> _interactionWrappers = new List<InteractionWrapper>();
    [SerializeField] 
    private InteractionBounds _interactionBounds;

    private BroadcastReceiver<Grabable> _broadcastReceiver;
    [SerializeField]
    [ReadOnly]
    [HideInEditorMode]
    private List<Interaction> _interactions = new List<Interaction>();



    private void Start()
    {
        _broadcastReceiver = new BroadcastReceiver<Grabable>(_interactionBounds);
        _broadcastReceiver.OnBroadcastReceived += BroadcastReceiverOnBroadcastReceived;

        foreach (InteractionWrapper interactionWrapper in _interactionWrappers)
        {
            _interactions.Add(interactionWrapper.GetInteraction(this));
        }
    }

    private void BroadcastReceiverOnBroadcastReceived(Grabable grabable)
    {
        Debug.Log("Interact");
        
        foreach (Interaction interaction in _interactions)
        {
            if (interaction.ItemType == grabable.ItemType)
            {
                interaction.Interact(grabable);
                //_interaction.OnInteractionBreak += InteractionOnInteractionBreak;
                //_interaction.OnInteractionFinished += InteractionOnInteractionFinished;
            }
        }

    }

    private void InteractionOnInteractionFinished()
    {
        Destroy(transform.parent.gameObject);
    }

    private void InteractionOnInteractionBreak()
    {
        
    }
}
