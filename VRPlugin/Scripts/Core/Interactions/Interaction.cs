using System;
using UnityEngine;

public abstract class ControllerData
{
    public abstract Interaction GetInteraction(InteractionController owner);
}

[System.Serializable]
public class Interaction
{
    public event Action OnInteractionBreak;
    public event Action OnInteractionFinished;
    
    [SerializeField] 
    private ItemType _itemType;
    
    protected InteractionController _owner;

    private Grabable _grabable;

    public ItemType ItemType => _itemType;

    public virtual string InteractionID()
    {
        throw new NotImplementedException();
    }

    protected virtual GrabControlScheme GetGrabControlScheme()
    {
        throw new NotImplementedException();
    }
    
    protected Interaction(InteractionController owner)
    {
        _owner = owner;
    }
    public void Interact(Grabable grabable)
    {
        _grabable = grabable;
        _grabable.OverrideControlScheme(GetGrabControlScheme());
    }
    
    protected  void BreakInteraction()
    {
        OnInteractionBreak?.Invoke();
        _grabable.SetBackToDefaultControlScheme();
    }

    protected void FinishInteraction()
    {
        OnInteractionFinished?.Invoke();
    }
    


}
