using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    private enum HandType
    {
        Left,
        Right
    }

    [SerializeField] 
    private HandType _type;

    [SerializeField] 
    private Interactor _interactor;

    [SerializeField]
    private GameObject _inactiveMesh;
    [SerializeField] 
    private GameObject _activeMesh;
    [SerializeField] 
    private PlayerInput _input;
    
    void Awake()
    {
        string grabString = _type + "Grab";
        _input.actions[grabString].performed += OnGrabPerformed;
        _input.actions[grabString].canceled += OnGrabCanceled;
    }
    
    [Button]
    private void OnGrabCanceled(InputAction.CallbackContext obj)
    {
        _interactor.Deactivate();
        _inactiveMesh.SetActive(true);
        _activeMesh.SetActive(false);
    }

    [Button]
    private void OnGrabPerformed(InputAction.CallbackContext obj)
    {
        _interactor.Actitvate();
        _inactiveMesh.SetActive(false);
        _activeMesh.SetActive(true);
    }

}
