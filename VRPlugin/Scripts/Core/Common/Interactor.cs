using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Flags]
public enum InteractionLayer
{
    Default  = 1 << 0, 
    Hands= 1 << 1, 
    Cannon= 1 << 2, 
    Lightmap= 1 << 3, 
    Squishing = 1 << 4, 
}

public class Interactor : MonoBehaviour
{
    public event Action<Interactor> OnActivated;
    public event Action<Interactor> OnPositionChanged;
    public event Action<Interactor> OnReleased;
    
    [SerializeField]
    [SingleEnumFlagSelect(EnumType = typeof(InteractionLayer))]
    private InteractionLayer _interactionLayer;
    
    private InteractionSystem _interactionSystem;

    [SerializeField]
    [ReadOnly]
    private bool _interactive = false;
    private Vector3 _lastPosition;


    public bool Interactive
    {
        get => _interactive;
        set => _interactive = value;
    }

    public InteractionLayer InteractionLayer
    {
        get => _interactionLayer;
        set => _interactionLayer = value;
    }

    #region UnityMethods
    private void Awake()
    {
        _interactionSystem = InteractionSystem.GetInteractionSystemInstance();
        _lastPosition = transform.position;
    }

    private void OnEnable()
    {
        _interactionSystem.Interactors.Add(this);
    }

    private void OnDisable()
    {
        OnReleased?.Invoke(this);
        _interactionSystem.Interactors.Remove(this);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _lastPosition) > Single.Epsilon)
        {
            _lastPosition = transform.position;
            OnPositionChanged?.Invoke(this);
        }
    }
#endregion

#region PublicMethods
    [Button("Activate")]
    public void Actitvate()
    {
        _interactive = true;
        OnActivated?.Invoke(this);
    }

    [Button]
    public void Deactivate()
    {
        _interactive = false;
        OnReleased?.Invoke(this);
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
#endregion
}
