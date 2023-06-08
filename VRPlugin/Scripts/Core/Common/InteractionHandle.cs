using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class InteractionHandle : MonoBehaviour
{
#region Events
    public event Action onStay;
    public event Action<TransformData[]> OnBeingGrabbed;
    public event Action<Interactor> OnGrabbed;
    public event Action<Interactor> OnReleased;
#endregion
#region SerializedFields
    [SerializeField] 
    private InteractionLayer _interactionMask;
    [SerializeField]
    public UnityEvent _onEntered;
    [SerializeField] 
    [Required]
    private InteractionBounds _interactionBounds;
    [SerializeField]
    public UnityEvent _onExited;
    [SerializeField] 
    private bool _releaseInteractorsWhenTheyLeaveHandleArea = false;
    
#endregion
    private InteractionSystem _interactionSystem;
    private bool _isGrabbed = false;
    [SerializeField]
    [ReadOnly]
    private List<Interactor> _interactorsInside = new List<Interactor>();
    private ManagedList<Interactor> _interactorsGrabbing = new ManagedList<Interactor>();

#region Properties
    public bool IsGrabbed => _isGrabbed;

    public ManagedList<Interactor> InteractorsGrabbing => _interactorsGrabbing;

#endregion

#region UnityMethods
    private void Awake()
    {
        _interactionSystem = InteractionSystem.GetInteractionSystemInstance();
        _interactorsGrabbing.OnFirstElementAdded += OnFirstGrabbed;
        _interactorsGrabbing.OnEmptied += OnLastReleased;
    }
    private void OnEnable()
    {
        _interactionSystem.Handles.Add(this);
    }

    private void OnDisable()
    {
        _interactionSystem.Handles.Remove(this);
    }
#endregion
    
#region Callbacks
    private void OnLastReleased()
    {
        _isGrabbed = false;
    }

    private void OnFirstGrabbed()
    {
        _isGrabbed = true;
    }
    
    private void InteractorOnPositionChanged(Interactor interactor)
    {
        if (!_interactionBounds.IsWithinReach(interactor.GetPosition()))
        {
            interactor.OnPositionChanged -= InteractorOnPositionChanged;
            _interactorsInside.Remove(interactor);
            if (_releaseInteractorsWhenTheyLeaveHandleArea)
            {
                _interactorsGrabbing.Remove(interactor);
            }
        }
        if(_isGrabbed)
        {
            HandleBeingGrabbed();
        }
    }
    
    private void InteractorOnReleased(Interactor interactor)
    {
        Release(interactor);
    }
#endregion
    
#region PublicMethods
    [Button("Enter")]
    public void Enter(Interactor interactor)
    {
        if (!_interactorsInside.Contains(interactor))
        {
            _interactorsInside.Add(interactor);
            if (interactor.Interactive)
            {
                OnActivated(interactor);
            }
            else
            {
                interactor.OnActivated += OnActivated;
            }
            interactor.OnPositionChanged += InteractorOnPositionChanged;
            _onEntered?.Invoke();
        }
    }

    public void OnActivated(Interactor interactor)
    {
        _isGrabbed = true;
        interactor.OnReleased += InteractorOnReleased;
        _interactorsGrabbing.Add(interactor);
        OnGrabbed?.Invoke(interactor);
        StartCoroutine(GrabbedCoroutine());
    }
    
    public void HandleBeingGrabbed()
    {            
        int count = _interactorsGrabbing.Count;
        TransformData[] args = new TransformData[count];
        for (int i=0; i < count; i++)
        {
            Interactor interactor = _interactorsGrabbing.Get(i);
            Vector3 position = interactor.GetPosition();
            Quaternion rotation = interactor.GetRotation();
            args[i] = new TransformData(position, rotation);
        }
        OnBeingGrabbed?.Invoke(args);
    }
    
    public void Release(Interactor interactor)
    {
        _isGrabbed = false;
        _interactorsGrabbing.Remove(interactor);
        OnReleased?.Invoke(interactor);
    }

    public bool CanInteract(Interactor interactor)
    {
        Assert.IsNotNull(_interactionBounds);
        if (HasMatchingLayer(interactor))
        {
            return _interactionBounds.IsWithinReach(interactor.GetPosition());
        }

        return false;
    }
    
    private bool HasMatchingLayer(Interactor interactor)
    {
        return (interactor.InteractionLayer & _interactionMask) != 0;
    }
    
    public bool HasInteractorsInside()
    {
        return !_interactorsInside.IsNullOrEmpty();
    }
#endregion

    IEnumerator GrabbedCoroutine()
    {
        while (_isGrabbed)
        {
            HandleBeingGrabbed();
            yield return null;
        }
    }
 
}
