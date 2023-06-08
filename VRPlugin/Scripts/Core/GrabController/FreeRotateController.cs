using System;
using UnityEngine;

public class FreeRotateController : GrabController
{
    private Vector3 _rotationAxis;
    private Vector3 _previousOrigin;
    
    public override void DrawGizmosWhenUsed()
    {
        Gizmos.DrawLine(_previousOrigin, _rotationAxis);
    }

    public override bool CanBeUsed(int interactorCount)
    {
        return interactorCount == 1;
    }

    public FreeRotateController( Vector3 rotationAxis)
    {
        _rotationAxis = rotationAxis;
    }

    protected override Quaternion AdjustRotation(TransformData[] args, Quaternion rotation)
    {
        Vector3 origin = args[0].Position;
        _previousOrigin = origin;
        return Quaternion.LookRotation(_rotationAxis - origin);
    }
}
