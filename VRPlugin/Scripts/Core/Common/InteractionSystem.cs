using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private readonly ManagedList<Interactor> _interactors = new ManagedList<Interactor>();
    private readonly ManagedList<InteractionHandle> _handles = new ManagedList<InteractionHandle>();

    public ManagedList<Interactor> Interactors => _interactors;

    public ManagedList<InteractionHandle> Handles => _handles;

    private Coroutine _checkCoroutine;
    private bool _isSearching = false;
    
    public static InteractionSystem GetInteractionSystemInstance()
    {
        InteractionSystem interactionSystem = FindObjectOfType<InteractionSystem>();
        if (interactionSystem)
        {
            interactionSystem.Initialize();
            return interactionSystem;
        }
        else
        {
            return new GameObject("InteractionSystem").AddComponent<InteractionSystem>();
        }
    }

    private void Initialize()
    {
        _interactors.OnEmptied += OnEmptied;
        _interactors.OnFirstElementAdded += OnFirstElementAdded;
        _handles.OnEmptied += OnEmptied;
        _handles.OnFirstElementAdded += OnFirstElementAdded;
    }

    private void OnFirstElementAdded()
    {
        if (!_isSearching && !_interactors.IsEmpty && !_handles.IsEmpty)
        {
            _isSearching = true;
            _checkCoroutine = StartCoroutine(CheckInteractions());
        }
    }

    private void OnEmptied()
    {
        if (_isSearching)
        {
            _isSearching = false;
            StopCoroutine(_checkCoroutine);
        }
    }
    
    private IEnumerator CheckInteractions()
    {
        while (_isSearching)
        {
            List<Interactor> interactors = _interactors.List.GetRange(0, _interactors.Count);
            List<InteractionHandle> handles = _handles.List.GetRange(0, _handles.Count);
                
            foreach (Interactor interactor in interactors)
            {
                foreach (InteractionHandle interactionHandle in handles)
                {
                    if (interactionHandle.CanInteract(interactor))
                    {
                        interactionHandle.Enter(interactor);
                    }
                }
            }
            yield return null;
        }
    }
}
