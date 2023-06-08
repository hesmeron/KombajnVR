using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public struct RailroadedCallback
{
    public enum ComparisonType
    {
        Greater,
        LessThan
    }
    public ComparisonType comparisonType;
    public float CallValue;
    public Action Callback;

    public RailroadedCallback(ComparisonType comparisonType, float callValue, Action callback)
    {
        this.comparisonType = comparisonType;
        CallValue = callValue;
        Callback = callback;
    }

    public void UpdateCallback(float value)
    {
        if (FitsCondition(value))
        {
            Callback?.Invoke();
        }
    }

    private bool FitsCondition(float value)
    {
        switch (comparisonType)
        {
            case ComparisonType.Greater:
                return value > CallValue;
            case ComparisonType.LessThan:
                return value < CallValue;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public class RailroadedGrabController : GrabController
{
    private Interactor _first;
    private Interactor _second;
    private RailroadedData _data;
    private float _fac;

    [System.Serializable]
    public class RailroadedData : ControllerData
    {
        private List<RailroadedCallback> _railroadedCallbacks = new List<RailroadedCallback>();
        [SerializeField] 
        private GrabControlScheme _scheme;
        [SerializeField]
        private Vector3 _boundaryOrigin = Vector3.zero;
        [SerializeField]
        private Vector3 _boundaryEnd = Vector3.zero;

        public List<RailroadedCallback> RailroadedCallbacks => _railroadedCallbacks;

        public Vector3 BoundaryOrigin
        {
            get => _boundaryOrigin;
            set => _boundaryOrigin = value;
        }

        public Vector3 BoundaryEnd
        {
            get => _boundaryEnd;
            set => _boundaryEnd = value;
        }

        public GrabControlScheme Scheme => _scheme;

        public void AddCallback(RailroadedCallback railroadedCallback)
        {
            _railroadedCallbacks.Add(railroadedCallback);
        }

        public override Interaction GetInteraction(InteractionController owner)
        {
            return new RailroadedInteraction(owner, this);
        }
    }

    public RailroadedGrabController(Interactor first, Interactor second, RailroadedData data)
    {
        _first = first;
        _second = second;
        _data = data;
        Assert.IsNotNull(_data);
    }

    public override bool CanBeUsed(int interactorCount)
    {
        return interactorCount is > 0 and < 2;
    }

    protected override Quaternion AdjustRotation(TransformData[] args, Quaternion rotation)
    {
        return Quaternion.LookRotation(_data.BoundaryOrigin - _data.BoundaryEnd);
    }

    protected override Vector3 AdjustPosition(TransformData[] args, Vector3 position)
    {
        Assert.IsNotNull(_data);
        Trigonometry.GetCastPoint(_data.BoundaryOrigin,
                                    _data.BoundaryEnd,
                                    _first.GetPosition(),
                                    out Vector3 resultFirst,
                                    out float firstSignedFac);
        Trigonometry.GetCastPoint(_data.BoundaryOrigin,
                                    _data.BoundaryEnd,
                                    _second.GetPosition(),
                                    out Vector3 resultSecond,
                                    out float secondSignedFac);
        _fac = (firstSignedFac + secondSignedFac) / 2f;

        foreach (var callback in _data.RailroadedCallbacks)
        {
            callback.UpdateCallback(_fac);
        }
        return (resultFirst + resultSecond) / 2f;
    }
}
