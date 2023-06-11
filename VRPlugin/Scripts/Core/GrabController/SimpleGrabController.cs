using System;
using UnityEngine;

public class SimpleGrabController : GrabController
{
    private Interactor _interactor;
    public SimpleGrabController(Interactor interactor)
    {
        _interactor = interactor;
    }
    public override bool CanBeUsed(int interactorCount)
    {
        return interactorCount == 1;
    }

    protected override Quaternion AdjustRotation(TransformData[] args, Quaternion rotation)
    {
        Vector3 forward = Vector3.forward; //Vector3.Lerp(Owner.Target.forward, Vector3.down, Time.deltaTime);
        return Quaternion.LookRotation(forward);
    }

    protected override Vector3 AdjustPosition(TransformData[] args, Vector3 position)
    {
        return args[0].Position;
    }
}
