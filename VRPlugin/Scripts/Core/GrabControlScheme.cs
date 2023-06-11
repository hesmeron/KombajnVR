using System;
using System.Collections.Generic;
using UnityEngine;

public enum GrabControllerType
{
    Simple,
    Stick,
    Rotation,
    Railroaded,
    FreeRotation
}

[System.Serializable]
public class GrabControlScheme
{
    [SerializeField] 
    private List<GrabControllerType> _possibleControllerTypes = new List<GrabControllerType>();
    [SerializeField]
    private RailroadedGrabController.RailroadedData _railroadedData;
    [SerializeField]
    private RotateGrabController.Data _rotateData;
    [SerializeField] 
    private Vector3 _rotationAxis;

    public RotateGrabController.Data RotateData => _rotateData;

    public void SetRailroadedGrabController(RailroadedGrabController.RailroadedData serializedData)
    {
        _railroadedData = serializedData;
    }
    
    public GrabController GetGrabController(List<Interactor> interactors, GrabBehaviour owner)
    {
        int index = interactors.Count -1;
        if (index >= 0)
        {
            GrabController controller = CreateGrabController(_possibleControllerTypes[index], interactors, owner);
            return controller;
        }
        
        return null;
    }
    
    private GrabController CreateGrabController(GrabControllerType grabControllerType, List<Interactor>grabbingInteractors, GrabBehaviour owner)
    {
        GrabController result;
        switch (grabControllerType)
        {
            case GrabControllerType.Simple:
                result = new SimpleGrabController(grabbingInteractors[0]);
                break;
            case GrabControllerType.Railroaded:
                result = new RailroadedGrabController(grabbingInteractors[0], grabbingInteractors[1], _railroadedData);
                break;
            case GrabControllerType.Rotation:
                 return new RotateGrabController(_rotateData, grabbingInteractors[0].GetPosition(), owner.ReturnValue);
            case GrabControllerType.FreeRotation:
                result = new FreeRotateController(_rotationAxis);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return result;
    }
}