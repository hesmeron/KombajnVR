using System;
using UnityEngine;

public class InteractionBounds : MonoBehaviour
{
    public enum BoundType
    {
        Box,
        Sphere,
        Frustum
    }
    [SerializeField]
    private float _radius;
    [SerializeField] 
    private Bounds _bounds;
    [SerializeField] 
    private float _maxDistance;
    [SerializeField]
    private float _zMultiplier;
    [SerializeField]
    private BoundType _boundType = BoundType.Sphere;
    
    public BoundType CurrentBoundType => _boundType;
    public Bounds Bounds => _bounds;

    public float MaxDistance => _maxDistance;

    public float ZMultiplier => _zMultiplier;

    public float Radius
    {
        get => _radius;
        set => _radius = value;
    }
    public void EditBounds(Vector3 size, Vector3 center)
    {
        Debug.Log("Edit bounds");
        _bounds = new Bounds(center, size);
    }

    public bool IsWithinReach(Vector3 globalPosition)
    {
        Vector3 position = transform.InverseTransformPoint(globalPosition
        );
        switch (_boundType)
        {
            case BoundType.Box:
                return _bounds.Contains(position);
            case BoundType.Sphere:
                return position.magnitude <= _radius;
            case BoundType.Frustum:
                float distanceFromLine = Mathf.Sqrt((position.x * position.x) + (position.y * position.y));
                return position.z < _maxDistance && distanceFromLine <= position.z * _zMultiplier;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
