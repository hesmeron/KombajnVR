using UnityEngine;

//TODO:Generalize the shit of of this motherfucker
public enum ItemType
{
    PowderLoad, 
    CleaningStick
}

[System.Serializable]
public class Grabable : MonoBehaviour
{
    [SerializeField] 
    private ItemType _itemType;
    [SerializeField] 
    protected InteractionHandle _handle;
    [SerializeField] 
    protected BroadcastSenderBehaviour _broadcastSenderBehaviour;
    [SerializeField] 
    private GrabBehaviour _grabBehaviour;
    
    private bool _isOverriden = false;
    [SerializeField]
    private GrabControlScheme _startGrabControlScheme;
    
    public ItemType ItemType => _itemType;


    private void Start()
    {
        _handle.OnGrabbed += HandleOnGrabbed;
        _handle.OnReleased += HandleOnReleased;
        _grabBehaviour.SetControlScheme(_startGrabControlScheme);
    }

    private void HandleOnReleased(Interactor interactor)
    {
        //_interactor.Deactivate();
    }

    private void HandleOnGrabbed(Interactor interactor)
    {
        _broadcastSenderBehaviour.Broadcast(this);
    }
    
    public void SetBackToDefaultControlScheme()
    {
        _isOverriden = false;
        _grabBehaviour.SetControlScheme(_startGrabControlScheme);
    }
    
    public void OverrideControlScheme(GrabControlScheme controlScheme)
    {
        _isOverriden = true;
        _grabBehaviour.SetControlScheme(controlScheme);
    }

}
