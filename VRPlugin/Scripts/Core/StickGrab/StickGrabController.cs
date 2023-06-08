using System;
using UnityEngine;
/*
public class StickGrabController : GrabController
{
    private const int CountOfUsedInteractors = 2;
    private Vector3 _lastOriginPosition;
    private Interactor _top;
    private Interactor _bottom;
    public StickGrabController(Interactor top, Interactor bottom, GrabBehaviour owner) : base(owner)
    {
        _top = top;
        _bottom = bottom;
    }
    public override void OnGrabbed()
    {
        _lastOriginPosition = GetOrigin();
        Owner.Target.position = _lastOriginPosition - Owner.GetOffset();
    }

    public override bool CanBeUsed(int interactorCount)
    {
        return interactorCount == CountOfUsedInteractors;
    }

    protected override Quaternion AdjustRotation(TransformData[] args,Quaternion rotation)
    {
        Vector3 from = _top.transform.position;
        Vector3 to = _bottom.transform.position;
        return Quaternion.LookRotation(to - from);
    }

    protected override Vector3 AdjustPosition(TransformData[] args, Vector3 position)
    {
        return GetOrigin() + Owner.GetOffset();
    }

    private Vector3 GetOrigin()
    {
        Vector3 from = _top.transform.position;
        Vector3 to = _bottom.transform.position;
        return (to + from)/2;
    }
}
*/