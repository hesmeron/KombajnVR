using System;
using UnityEngine;

public abstract class GrabController
{
    public event Action<float> OnValueChanged;
    public virtual void DrawGizmosWhenUsed() { }
    public virtual void OnGrabbed() { }
    public virtual void OnReleased() { }

    public abstract bool CanBeUsed(int interactorCount);

    public TransformData  OnBeingGrabbed(TransformData[] args, Vector3 startPosition, Quaternion startRotation)
    {
        Vector3 position = AdjustPosition(args, startPosition);
        Quaternion rotation = AdjustRotation(args, startRotation);
        return new TransformData(position, rotation);
    }

    protected virtual Quaternion AdjustRotation(TransformData[] args, Quaternion rotation)
    {
        return rotation;
    }

    protected virtual Vector3 AdjustPosition(TransformData[] args, Vector3 position)
    {
        return position;
    }

    protected void InvokeCompletionEvents(float completion)
    {
        OnValueChanged?.Invoke(Mathf.Clamp01(completion));
    }
}
