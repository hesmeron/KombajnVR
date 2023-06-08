using System;
using UnityEngine;

[System.Serializable]
public class RailoradedWrapper
{
    [SerializeField] 
    private RailroadedGrabController.RailroadedData _railroadedData;

    public RailroadedInteraction GetRailroadedInteraction(InteractionController owner)
    {
        return new RailroadedInteraction(owner, _railroadedData);
    }
    
}
public class RailroadedInteraction : Interaction
{
    private readonly RailroadedGrabController.RailroadedData _railroadedData;
    private Vector3 _origin;
    private Vector3 _end;
    private bool _lastWasLow = false;
    private int _trustsPerformed = 0;

    public RailroadedInteraction(InteractionController owner, RailroadedGrabController.RailroadedData data) : base(owner)
    {
        _railroadedData = data;
    }
    
    private void LowCallback()
    {
        if (!_lastWasLow)
        {
            _lastWasLow = true;
            _trustsPerformed++;
            if (_trustsPerformed >= 3)
            {
                FinishInteraction();
            }
        }

    }
    
    private void HighCallback()
    {
        _lastWasLow = false;
    }

    public override String  InteractionID()
    {
        return typeof(RailroadedInteraction).ToString();
    }

    protected override GrabControlScheme GetGrabControlScheme()
    {
        RailroadedCallback breakCallback = new RailroadedCallback(RailroadedCallback.ComparisonType.Greater,
            1.2f,
            BreakInteraction);
        RailroadedCallback highCallback = new RailroadedCallback(RailroadedCallback.ComparisonType.Greater,
            0.8f,
            HighCallback);
        RailroadedCallback lowCallback = new RailroadedCallback(RailroadedCallback.ComparisonType.LessThan,
            0.2f,
            LowCallback);
        _railroadedData.AddCallback(breakCallback);
        _railroadedData.AddCallback(lowCallback);
        _railroadedData.AddCallback(highCallback);
        UpdateOrigin();
        _railroadedData.Scheme.SetRailroadedGrabController(_railroadedData);
        return _railroadedData.Scheme;
    }

    void UpdateOrigin()
    {
        _origin = _owner.transform.TransformPoint(_railroadedData.BoundaryOrigin);
        _end = _owner.transform.TransformPoint(_railroadedData.BoundaryEnd);
    }
    
}
